using Domain.Seedwork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.File.Service.Api.Web.Extensions;
using Presentation.Seedwork;

namespace Presentation.File.Service.Api.Web
{
    public class StartupLocalFileSystem
    {
        public StartupLocalFileSystem(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(ApiModelStateCheckFilterAttribute));
                x.Filters.Add(typeof(ApiExceptionFilterAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });

            services
                .AddLocalStorageFileServiceConfiguration(Configuration)
                .AddPostgresConfiguration(Configuration)
                .AddApplicationLayerServices(Configuration)
                .AddRepositories(Configuration)
                .AddSingleton<IIdentityGenerator<long>, DefaultIdentityGenerator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}