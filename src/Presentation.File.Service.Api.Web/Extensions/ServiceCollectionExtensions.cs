using System.IO;
using Application.File;
using Domain.File;
using Domain.Seedwork;
using Infrastructure.Data.File.PgSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.File.Service.Api.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalStorageFileServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new LocalStorageFileServiceOptions {RootDirectory = configuration.GetValue<string>("RootDirectory")};
            if (!Directory.Exists(options.RootDirectory))
                Directory.CreateDirectory(options.RootDirectory);

            services.AddSingleton(options);
            return services;
        }

        public static IServiceCollection AddPostgresConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new PostgresOptions
            {
                BizDatabaseConnectionString = configuration.GetConnectionString("FileMetadataDatabase")
            });

            services.AddOptions<LocalIdentityGeneratorOptions>().Configure(o => { o.MachineTag = 1; });

            return services;
        }

        public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IFileStorageDirectorySelector, SimpleFileStorageDirectorySelector>();
            services.AddScoped<ILocalFileStorageService, DefaultLocalFileStorageService>();
            services.AddScoped<IImageFileProcessor, DefaultImageFileProcessor>();
            services.AddScoped<IImageSharpProcessActionAdapter, DefaultImageSharpProcessActionAdapter>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileMetadataRepository, DefaultFileMetadataRepository>();

            return services;
        }
    }
}