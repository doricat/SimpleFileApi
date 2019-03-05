using System;
using System.Threading.Tasks;
using Dapper;
using Domain.Seedwork;
using Npgsql;

namespace Infrastructure.Data.File.PgSQL
{
    public class PostgresIdentityGenerator : IIdentityGenerator<long>
    {
        public PostgresIdentityGenerator(PostgresOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public PostgresOptions Options { get; }

        public async Task<long> GenerateAsync()
        {
            using (var conn = new NpgsqlConnection(Options.IdSequenceDatabaseConnectionString))
            {
                await conn.OpenAsync();
                return await conn.QueryFirstAsync<long>($"SELECT nextval('{Options.IdSequenceName}')");
            }
        }
    }
}