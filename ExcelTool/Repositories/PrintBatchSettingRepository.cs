using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class PrintBatchSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<PrintBatchSetting> GetAll()
        {
            var result =
                new List<PrintBatchSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    LotName,
                    Quantity,
                    Unit,
                    SortOrder
                FROM PrintBatchSettings
                ORDER BY SortOrder, Id;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            using SqliteDataReader reader =
                command.ExecuteReader();


            while (reader.Read())
            {
                result.Add(
                    new PrintBatchSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        LotName =
                            reader.IsDBNull(
                                reader.GetOrdinal("LotName"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("LotName")),

                        Quantity =
                            reader.IsDBNull(
                                reader.GetOrdinal("Quantity"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Quantity")),

                        Unit =
                            reader.IsDBNull(
                                reader.GetOrdinal("Unit"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Unit")),

                        SortOrder =
                            reader.IsDBNull(
                                reader.GetOrdinal("SortOrder"))
                                ? 0
                                : reader.GetInt32(
                                    reader.GetOrdinal("SortOrder"))
                    });
            }


            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public PrintBatchSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    LotName,
                    Quantity,
                    Unit,
                    SortOrder
                FROM PrintBatchSettings
                WHERE Id = @Id
                LIMIT 1;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@Id",
                id);


            using SqliteDataReader reader =
                command.ExecuteReader();


            if (!reader.Read())
            {
                return null;
            }


            return new PrintBatchSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                LotName =
                    reader.IsDBNull(
                        reader.GetOrdinal("LotName"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("LotName")),

                Quantity =
                    reader.IsDBNull(
                        reader.GetOrdinal("Quantity"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Quantity")),

                Unit =
                    reader.IsDBNull(
                        reader.GetOrdinal("Unit"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Unit")),

                SortOrder =
                    reader.IsDBNull(
                        reader.GetOrdinal("SortOrder"))
                        ? 0
                        : reader.GetInt32(
                            reader.GetOrdinal("SortOrder"))
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            PrintBatchSetting setting)
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
                INSERT INTO PrintBatchSettings
                (
                    LotName,
                    Quantity,
                    Unit,
                    SortOrder
                )
                VALUES
                (
                    @LotName,
                    @Quantity,
                    @Unit,
                    @SortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@LotName",
                setting.LotName ?? "");


            command.Parameters.AddWithValue(
                "@Quantity",
                setting.Quantity);


            command.Parameters.AddWithValue(
                "@Unit",
                setting.Unit ?? "");


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            PrintBatchSetting setting)
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
                UPDATE PrintBatchSettings
                SET
                    LotName = @LotName,
                    Quantity = @Quantity,
                    Unit = @Unit,
                    SortOrder = @SortOrder
                WHERE Id = @Id;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@Id",
                setting.Id);


            command.Parameters.AddWithValue(
                "@LotName",
                setting.LotName ?? "");


            command.Parameters.AddWithValue(
                "@Quantity",
                setting.Quantity);


            command.Parameters.AddWithValue(
                "@Unit",
                setting.Unit ?? "");


            command.Parameters.AddWithValue(
                "@SortOrder",
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


            const string sql = @"
                DELETE FROM PrintBatchSettings
                WHERE Id = @Id;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


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
                DELETE FROM PrintBatchSettings;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.ExecuteNonQuery();


            return true;
        }
    }
}