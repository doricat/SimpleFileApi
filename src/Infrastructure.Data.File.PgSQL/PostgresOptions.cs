namespace Infrastructure.Data.File.PgSQL
{
    public class PostgresOptions
    {
        public string BizDatabaseConnectionString { get; set; }

        public string IdSequenceDatabaseConnectionString { get; set; }

        public string IdSequenceName { get; set; }
    }
}