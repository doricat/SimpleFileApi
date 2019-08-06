using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.File;
using Microsoft.AspNetCore.Mvc;
using Presentation.File.Service.Api.Web.ViewModels;
using Presentation.Seedwork;

namespace Presentation.File.Service.Api.Web.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public ImagesController(IFileService fileService, IImageFileProcessor imageFileProcessor)
        {
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            ImageFileProcessor = imageFileProcessor ?? throw new ArgumentNullException(nameof(imageFileProcessor));
        }

        public IFileService FileService { get; }

        public IImageFileProcessor ImageFileProcessor { get; }

        /// <summary>
        /// 获取图片文件
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/images/123/?$action=resize(1000,1000).crop(100,100,500,500).rotate(90)
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id, [FromQuery(Name = "$action")] Exp exp)
        {
            var file = await FileService.GetAsync(id);
            if (file == null)
                return NotFound();
            Stream stream;
            if (exp == null)
            {
                stream = file.Stream;
            }
            else
            {
                stream = ImageFileProcessor.Process(file.Stream, file.ContentType, exp.ToExp());
                file.Stream.Dispose();
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, file.ContentType);
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/images/
        ///         content-type:multipart/form-data
        ///         content-type:image/jpg、image/jpeg、image/png
        ///         name="file"
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult<FileCreationOutputModel>), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromForm] ImageFileUploadModel model)
        {
            using (var stream = new MemoryStream())
            {
                await model.File.CopyToAsync(stream);
                var id = await FileService.SaveAsync(stream, model.File.ContentType);
                return StatusCode((int) HttpStatusCode.Created, new ApiResult<FileCreationOutputModel>(new FileCreationOutputModel
                {
                    Id = id,
                    Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/images/{id}"
                }));
            }
        }

        /// <summary>
        /// 对指定的图片进行编辑并返回新的图片信息
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PATCH /api/images/123
        ///         {
        ///             "Action": "resize(1000,1000).crop(100,100,500,500).rotate(90)"
        ///         }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResult<FileCreationOutputModel>), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Patch(long id, [FromBody] ImagePatchInputModel model)
        {
            var file = await FileService.GetAsync(id);
            if (file == null)
                return NotFound();

            var stream = ImageFileProcessor.Process(file.Stream, file.ContentType, model.ToExp());
            var fileId = await FileService.SaveAsync(stream, file.ContentType);
            file.Stream.Dispose();
            stream.Dispose();
            return StatusCode((int) HttpStatusCode.Created, new ApiResult<FileCreationOutputModel>(new FileCreationOutputModel
            {
                Id = fileId,
                Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/images/{fileId}"
            }));
        }
    }
}