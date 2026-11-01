using ExcelTool.Database;
using ExcelTool.Infrastructure;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class UnitSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<UnitSetting> GetAll()
        {
            var result =
                new List<UnitSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Unit,
                    SortOrder
                FROM UnitSettings
                ORDER BY SortOrder, Id;
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            using var reader =
                command.ExecuteReader();


            while (reader.Read())
            {
                result.Add(
                    new UnitSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        Unit =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        SortOrder =
                            reader.IsDBNull(2)
                                ? 0
                                : reader.GetInt32(2)
                    });
            }


            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public UnitSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Unit,
                    SortOrder
                FROM UnitSettings
                WHERE Id = $id
                LIMIT 1;
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.Parameters.AddWithValue(
                "$id",
                id);


            using var reader =
                command.ExecuteReader();


            if (!reader.Read())
            {
                return null;
            }


            return new UnitSetting
            {
                Id =
                    reader.GetInt32(0),

                Unit =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                SortOrder =
                    reader.IsDBNull(2)
                        ? 0
                        : reader.GetInt32(2)
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            UnitSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                INSERT INTO UnitSettings
                (
                    Unit,
                    SortOrder
                )
                VALUES
                (
                    $unit,
                    $sortOrder
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$unit",
                setting.Unit ?? "");

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            UnitSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                UPDATE UnitSettings
                SET
                    Unit = $unit,
                    SortOrder = $sortOrder
                WHERE Id = $id;
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$id",
                setting.Id);

            command.Parameters.AddWithValue(
                "$unit",
                setting.Unit ?? "");

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                DELETE FROM UnitSettings
                WHERE Id = $id;
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$id",
                id);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public bool DeleteAll()
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                DELETE FROM UnitSettings;
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.ExecuteNonQuery();


            return true;
        }
    }
}