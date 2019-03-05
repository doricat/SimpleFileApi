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
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public FilesController(IFileService fileService)
        {
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public IFileService FileService { get; }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/files/123
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            var file = await FileService.GetAsync(id);
            if (file == null)
                return NotFound();
            file.Stream.Seek(0, SeekOrigin.Begin);
            return File(file.Stream, file.ContentType);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/files/
        ///         content-type:mulipart/from-data
        ///         name="file"
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult<FileCreationOutputModel>), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiError), (int) HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromForm] FileUploadModel model)
        {
            using (var stream = new MemoryStream())
            {
                await model.File.CopyToAsync(stream);
                var id = await FileService.SaveAsync(stream, model.File.ContentType);
                return StatusCode((int) HttpStatusCode.Created, new ApiResult<FileCreationOutputModel>(new FileCreationOutputModel
                {
                    Id = id,
                    Location = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/api/files/{id}"
                }));
            }
        }
    }
}