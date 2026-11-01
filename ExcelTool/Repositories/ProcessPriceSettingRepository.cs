using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class ProcessPriceSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<ProcessPriceSetting> GetAll()
        {
            var result =
                new List<ProcessPriceSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    Process,
                    Stage,
                    UnitPrice,
                    Unit,
                    MinimumBatchPrice,
                    MinimumQuantity,
                    SortOrder
                FROM ProcessPriceSettings
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
                    new ProcessPriceSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        Process =
                            reader.IsDBNull(
                                reader.GetOrdinal("Process"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Process")),

                        Stage =
                            reader.IsDBNull(
                                reader.GetOrdinal("Stage"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Stage")),

                        UnitPrice =
                            reader.IsDBNull(
                                reader.GetOrdinal("UnitPrice"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("UnitPrice")),

                        Unit =
                            reader.IsDBNull(
                                reader.GetOrdinal("Unit"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Unit")),

                        MinimumBatchPrice =
                            reader.IsDBNull(
                                reader.GetOrdinal(
                                    "MinimumBatchPrice"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal(
                                        "MinimumBatchPrice")),

                        MinimumQuantity =
                            reader.IsDBNull(
                                reader.GetOrdinal(
                                    "MinimumQuantity"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal(
                                        "MinimumQuantity")),

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

        public ProcessPriceSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    Process,
                    Stage,
                    UnitPrice,
                    Unit,
                    MinimumBatchPrice,
                    MinimumQuantity,
                    SortOrder
                FROM ProcessPriceSettings
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


            return new ProcessPriceSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                Process =
                    reader.IsDBNull(
                        reader.GetOrdinal("Process"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Process")),

                Stage =
                    reader.IsDBNull(
                        reader.GetOrdinal("Stage"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Stage")),

                UnitPrice =
                    reader.IsDBNull(
                        reader.GetOrdinal("UnitPrice"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("UnitPrice")),

                Unit =
                    reader.IsDBNull(
                        reader.GetOrdinal("Unit"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Unit")),

                MinimumBatchPrice =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "MinimumBatchPrice"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal(
                                "MinimumBatchPrice")),

                MinimumQuantity =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "MinimumQuantity"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal(
                                "MinimumQuantity")),

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
            ProcessPriceSetting setting)
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
                INSERT INTO ProcessPriceSettings
                (
                    Process,
                    Stage,
                    UnitPrice,
                    Unit,
                    MinimumBatchPrice,
                    MinimumQuantity,
                    SortOrder
                )
                VALUES
                (
                    @Process,
                    @Stage,
                    @UnitPrice,
                    @Unit,
                    @MinimumBatchPrice,
                    @MinimumQuantity,
                    @SortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@Process",
                setting.Process ?? "");


            command.Parameters.AddWithValue(
                "@Stage",
                setting.Stage ?? "");


            command.Parameters.AddWithValue(
                "@UnitPrice",
                setting.UnitPrice);


            command.Parameters.AddWithValue(
                "@Unit",
                setting.Unit ?? "");


            command.Parameters.AddWithValue(
                "@MinimumBatchPrice",
                setting.MinimumBatchPrice);


            command.Parameters.AddWithValue(
                "@MinimumQuantity",
                setting.MinimumQuantity);


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            ProcessPriceSetting setting)
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
                UPDATE ProcessPriceSettings
                SET
                    Process = @Process,
                    Stage = @Stage,
                    UnitPrice = @UnitPrice,
                    Unit = @Unit,
                    MinimumBatchPrice = @MinimumBatchPrice,
                    MinimumQuantity = @MinimumQuantity,
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
                "@Process",
                setting.Process ?? "");


            command.Parameters.AddWithValue(
                "@Stage",
                setting.Stage ?? "");


            command.Parameters.AddWithValue(
                "@UnitPrice",
                setting.UnitPrice);


            command.Parameters.AddWithValue(
                "@Unit",
                setting.Unit ?? "");


            command.Parameters.AddWithValue(
                "@MinimumBatchPrice",
                setting.MinimumBatchPrice);


            command.Parameters.AddWithValue(
                "@MinimumQuantity",
                setting.MinimumQuantity);


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
                DELETE FROM ProcessPriceSettings
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
                DELETE FROM ProcessPriceSettings;
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