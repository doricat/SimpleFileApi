using System.IO;
using System.Reflection;
using Domain.Seedwork;
using Infrastructure.Data.File.PgSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Presentation.File.Service.Api.Web.Extensions;
using Presentation.Seedwork;
using Swashbuckle.AspNetCore.Swagger;

namespace Presentation.File.Service.Api.Web
{
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
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
                .AddSingleton<IIdentityGenerator<long>, PostgresIdentityGenerator>();

            services.AddSwaggerGen(x =>
            {
                x.EnableAnnotations();
                x.SwaggerDoc("v1", new Info { Title = "file service api", Version = "v1.0" });
                x.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, 
                    typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseSwagger(x => x.RouteTemplate = "api-docs/{documentName}/swagger.json");
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/api-docs/v1/swagger.json", "file service api"));
            app.UseMvc();
        }
    }
}