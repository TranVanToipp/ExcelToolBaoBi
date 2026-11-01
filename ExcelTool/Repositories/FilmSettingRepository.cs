using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class FilmSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<FilmSetting> GetAll()
        {
            var result =
                new List<FilmSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    LoaiMang,
                    DayMicron,
                    DaiM,
                    TrongLuongKgPerM,
                    M2PerKg,
                    SortOrder
                FROM FilmSettings
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
                    new FilmSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        LoaiMang =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        DayMicron =
                            reader.IsDBNull(2)
                                ? 0
                                : reader.GetDecimal(2),

                        DaiM =
                            reader.IsDBNull(3)
                                ? 0
                                : reader.GetDecimal(3),

                        TrongLuongKgPerM =
                            reader.IsDBNull(4)
                                ? 0
                                : reader.GetDecimal(4),

                        M2PerKg =
                            reader.IsDBNull(5)
                                ? 0
                                : reader.GetDecimal(5),

                        SortOrder =
                            reader.IsDBNull(6)
                                ? 0
                                : reader.GetInt32(6)
                    });
            }


            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public FilmSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    LoaiMang,
                    DayMicron,
                    DaiM,
                    TrongLuongKgPerM,
                    M2PerKg,
                    SortOrder
                FROM FilmSettings
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


            return new FilmSetting
            {
                Id =
                    reader.GetInt32(0),

                LoaiMang =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                DayMicron =
                    reader.IsDBNull(2)
                        ? 0
                        : reader.GetDecimal(2),

                DaiM =
                    reader.IsDBNull(3)
                        ? 0
                        : reader.GetDecimal(3),

                TrongLuongKgPerM =
                    reader.IsDBNull(4)
                        ? 0
                        : reader.GetDecimal(4),

                M2PerKg =
                    reader.IsDBNull(5)
                        ? 0
                        : reader.GetDecimal(5),

                SortOrder =
                    reader.IsDBNull(6)
                        ? 0
                        : reader.GetInt32(6)
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            FilmSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                INSERT INTO FilmSettings
                (
                    LoaiMang,
                    DayMicron,
                    DaiM,
                    TrongLuongKgPerM,
                    M2PerKg,
                    SortOrder
                )
                VALUES
                (
                    $loaiMang,
                    $dayMicron,
                    $daiM,
                    $trongLuongKgPerM,
                    $m2PerKg,
                    $sortOrder
                );
                """;


            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$loaiMang",
                setting.LoaiMang ?? "");

            command.Parameters.AddWithValue(
                "$dayMicron",
                setting.DayMicron);

            command.Parameters.AddWithValue(
                "$daiM",
                setting.DaiM);

            command.Parameters.AddWithValue(
                "$trongLuongKgPerM",
                setting.TrongLuongKgPerM);

            command.Parameters.AddWithValue(
                "$m2PerKg",
                setting.M2PerKg);

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            FilmSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = """
                UPDATE FilmSettings
                SET
                    LoaiMang = $loaiMang,
                    DayMicron = $dayMicron,
                    DaiM = $daiM,
                    TrongLuongKgPerM = $trongLuongKgPerM,
                    M2PerKg = $m2PerKg,
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
                "$loaiMang",
                setting.LoaiMang ?? "");

            command.Parameters.AddWithValue(
                "$dayMicron",
                setting.DayMicron);

            command.Parameters.AddWithValue(
                "$daiM",
                setting.DaiM);

            command.Parameters.AddWithValue(
                "$trongLuongKgPerM",
                setting.TrongLuongKgPerM);

            command.Parameters.AddWithValue(
                "$m2PerKg",
                setting.M2PerKg);

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
                DELETE FROM FilmSettings
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
                DELETE FROM FilmSettings;
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