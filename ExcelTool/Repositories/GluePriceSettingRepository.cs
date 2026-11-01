using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class GluePriceSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<GluePriceSetting> GetAll()
        {
            var result =
                new List<GluePriceSetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    LoaiKeo,
                    VndPerKg,
                    VndPer10Cm,
                    SortOrder
                FROM GluePriceSettings
                ORDER BY SortOrder, Id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(
                    new GluePriceSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        LoaiKeo =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        VndPerKg =
                            reader.IsDBNull(2)
                                ? 0
                                : reader.GetDecimal(2),

                        VndPer10Cm =
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

        public GluePriceSetting? GetById(int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    LoaiKeo,
                    VndPerKg,
                    VndPer10Cm,
                    SortOrder
                FROM GluePriceSettings
                WHERE Id = $id
                LIMIT 1;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                id);

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new GluePriceSetting
            {
                Id =
                    reader.GetInt32(0),

                LoaiKeo =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                VndPerKg =
                    reader.IsDBNull(2)
                        ? 0
                        : reader.GetDecimal(2),

                VndPer10Cm =
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
            GluePriceSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                INSERT INTO GluePriceSettings
                (
                    LoaiKeo,
                    VndPerKg,
                    VndPer10Cm,
                    SortOrder
                )
                VALUES
                (
                    $loaiKeo,
                    $vndPerKg,
                    $vndPer10Cm,
                    $sortOrder
                );
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$loaiKeo",
                setting.LoaiKeo ?? "");

            command.Parameters.AddWithValue(
                "$vndPerKg",
                setting.VndPerKg);

            command.Parameters.AddWithValue(
                "$vndPer10Cm",
                setting.VndPer10Cm);

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            GluePriceSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                UPDATE GluePriceSettings
                SET
                    LoaiKeo = $loaiKeo,
                    VndPerKg = $vndPerKg,
                    VndPer10Cm = $vndPer10Cm,
                    SortOrder = $sortOrder
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                setting.Id);

            command.Parameters.AddWithValue(
                "$loaiKeo",
                setting.LoaiKeo ?? "");

            command.Parameters.AddWithValue(
                "$vndPerKg",
                setting.VndPerKg);

            command.Parameters.AddWithValue(
                "$vndPer10Cm",
                setting.VndPer10Cm);

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                DELETE FROM GluePriceSettings
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

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
                DELETE FROM GluePriceSettings;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();

            return true;
        }
    }
}