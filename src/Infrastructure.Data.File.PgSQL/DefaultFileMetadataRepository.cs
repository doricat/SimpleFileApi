using System;
using System.Threading.Tasks;
using Dapper;
using Domain.File;
using Npgsql;

namespace Infrastructure.Data.File.PgSQL
{
    public class DefaultFileMetadataRepository : IFileMetadataRepository
    {
        public DefaultFileMetadataRepository(PostgresOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public PostgresOptions Options { get; }

        public async Task SaveAsync(FileMetadata file)
        {
            using (var conn = new NpgsqlConnection(Options.BizDatabaseConnectionString))
            {
                await conn.OpenAsync();
                await conn.ExecuteAsync("INSERT INTO file_metadata(id, file_name, content_type, size) VALUES(@id, @filename, @content_type, @size)",
                    new
                    {
                        id = file.Id,
                        filename = file.Filename,
                        content_type = file.ContentType,
                        size = file.Size
                    });
            }
        }

        public async Task<FileMetadata> GetAsync(long id)
        {
            using (var conn = new NpgsqlConnection(Options.BizDatabaseConnectionString))
            {
                await conn.OpenAsync();
                var entity = await conn.QueryFirstOrDefaultAsync<FileMetadata>(
                    "SELECT id, file_name AS Filename, content_type AS ContentType, size, created_at AS CreatedAt FROM file_metadata WHERE id = @id", new {id});
                return entity;
            }
        }
    }
}