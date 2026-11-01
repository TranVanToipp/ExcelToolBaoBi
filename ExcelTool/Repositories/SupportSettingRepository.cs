using ExcelTool.Database;
using ExcelTool.Infrastructure;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class SupportSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<SupportSetting> GetAll()
        {
            var result =
                new List<SupportSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Loai,
                    VndPerKg,
                    VndPerM2,
                    SortOrder
                FROM SupportSettings
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
                    new SupportSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        Loai =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        VndPerKg =
                            reader.IsDBNull(2)
                                ? 0
                                : reader.GetDecimal(2),

                        VndPerM2 =
                            reader.IsDBNull(3)
                                ? 0
                                : reader.GetDecimal(3),

                        SortOrder =
                            reader.IsDBNull(4)
                                ? 0
                                : reader.GetInt32(4)
                    });
            }


            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public SupportSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Loai,
                    VndPerKg,
                    VndPerM2,
                    SortOrder
                FROM SupportSettings
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


            return new SupportSetting
            {
                Id =
                    reader.GetInt32(0),

                Loai =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                VndPerKg =
                    reader.IsDBNull(2)
                        ? 0
                        : reader.GetDecimal(2),

                VndPerM2 =
                    reader.IsDBNull(3)
                        ? 0
                        : reader.GetDecimal(3),

                SortOrder =
                    reader.IsDBNull(4)
                        ? 0
                        : reader.GetInt32(4)
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            SupportSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                INSERT INTO SupportSettings
                (
                    Loai,
                    VndPerKg,
                    VndPerM2,
                    SortOrder
                )
                VALUES
                (
                    $loai,
                    $vndPerKg,
                    $vndPerM2,
                    $sortOrder
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$loai",
                setting.Loai ?? "");

            command.Parameters.AddWithValue(
                "$vndPerKg",
                setting.VndPerKg);

            command.Parameters.AddWithValue(
                "$vndPerM2",
                setting.VndPerM2);

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            SupportSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                UPDATE SupportSettings
                SET
                    Loai = $loai,
                    VndPerKg = $vndPerKg,
                    VndPerM2 = $vndPerM2,
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
                "$loai",
                setting.Loai ?? "");

            command.Parameters.AddWithValue(
                "$vndPerKg",
                setting.VndPerKg);

            command.Parameters.AddWithValue(
                "$vndPerM2",
                setting.VndPerM2);

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
                DELETE FROM SupportSettings
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
                DELETE FROM SupportSettings;
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