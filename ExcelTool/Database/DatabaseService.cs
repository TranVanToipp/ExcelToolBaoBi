using Microsoft.Data.Sqlite;

namespace ExcelTool.Database
{
    public static class DatabaseService
    {
        private static string ConnectionString =>
            $"Data Source={DatabaseInitializer.DatabasePath}";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }
}