using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class PrintTypeSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<PrintTypeSetting> GetAll()
        {
            var result =
                new List<PrintTypeSetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = @"
                SELECT
                    Id,
                    PrintingTechnology,
                    PrintType,
                    PlateSetCount,
                    PrintCount,
                    WasteFactor,
                    PaperFactor,
                    SortOrder
                FROM PrintTypeSettings
                ORDER BY SortOrder, Id;
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            using SqliteDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(
                    Map(reader));
            }

            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public PrintTypeSetting? GetById(int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = @"
                SELECT
                    Id,
                    PrintingTechnology,
                    PrintType,
                    PlateSetCount,
                    PrintCount,
                    WasteFactor,
                    PaperFactor,
                    SortOrder
                FROM PrintTypeSettings
                WHERE Id = @Id
                LIMIT 1;
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Id",
                id);

            using SqliteDataReader reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return Map(reader);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            PrintTypeSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = @"
                INSERT INTO PrintTypeSettings
                (
                    PrintingTechnology,
                    PrintType,
                    PlateSetCount,
                    PrintCount,
                    WasteFactor,
                    PaperFactor,
                    SortOrder
                )
                VALUES
                (
                    @PrintingTechnology,
                    @PrintType,
                    @PlateSetCount,
                    @PrintCount,
                    @WasteFactor,
                    @PaperFactor,
                    @SortOrder
                );
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@PrintingTechnology",
                setting.PrintingTechnology ?? "");

            command.Parameters.AddWithValue(
                "@PrintType",
                setting.PrintType ?? "");

            command.Parameters.AddWithValue(
                "@PlateSetCount",
                setting.PlateSetCount);

            command.Parameters.AddWithValue(
                "@PrintCount",
                setting.PrintCount);

            command.Parameters.AddWithValue(
                "@WasteFactor",
                setting.WasteFactor);

            command.Parameters.AddWithValue(
                "@PaperFactor",
                setting.PaperFactor);

            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            PrintTypeSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = @"
                UPDATE PrintTypeSettings
                SET
                    PrintingTechnology = @PrintingTechnology,
                    PrintType = @PrintType,
                    PlateSetCount = @PlateSetCount,
                    PrintCount = @PrintCount,
                    WasteFactor = @WasteFactor,
                    PaperFactor = @PaperFactor,
                    SortOrder = @SortOrder
                WHERE Id = @Id;
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Id",
                setting.Id);

            command.Parameters.AddWithValue(
                "@PrintingTechnology",
                setting.PrintingTechnology ?? "");

            command.Parameters.AddWithValue(
                "@PrintType",
                setting.PrintType ?? "");

            command.Parameters.AddWithValue(
                "@PlateSetCount",
                setting.PlateSetCount);

            command.Parameters.AddWithValue(
                "@PrintCount",
                setting.PrintCount);

            command.Parameters.AddWithValue(
                "@WasteFactor",
                setting.WasteFactor);

            command.Parameters.AddWithValue(
                "@PaperFactor",
                setting.PaperFactor);

            command.Parameters.AddWithValue(
                "@SortOrder",
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

            const string sql = @"
                DELETE FROM PrintTypeSettings
                WHERE Id = @Id;
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "@Id",
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

            const string sql = @"
                DELETE FROM PrintTypeSettings;
            ";

            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();

            return true;
        }


        // =========================================================
        // MAP
        // =========================================================

        private static PrintTypeSetting Map(
            SqliteDataReader reader)
        {
            return new PrintTypeSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                PrintingTechnology =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "PrintingTechnology"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal(
                                "PrintingTechnology")),

                PrintType =
                    reader.IsDBNull(
                        reader.GetOrdinal("PrintType"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("PrintType")),

                PlateSetCount =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "PlateSetCount"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal(
                                "PlateSetCount")),

                PrintCount =
                    reader.IsDBNull(
                        reader.GetOrdinal("PrintCount"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("PrintCount")),

                WasteFactor =
                    reader.IsDBNull(
                        reader.GetOrdinal("WasteFactor"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("WasteFactor")),

                PaperFactor =
                    reader.IsDBNull(
                        reader.GetOrdinal("PaperFactor"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("PaperFactor")),

                SortOrder =
                    reader.IsDBNull(
                        reader.GetOrdinal("SortOrder"))
                        ? 0
                        : reader.GetInt32(
                            reader.GetOrdinal("SortOrder"))
            };
        }
    }
}