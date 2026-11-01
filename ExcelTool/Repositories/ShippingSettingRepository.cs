using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class ShippingSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<ShippingSetting> GetAll()
        {
            var result =
                new List<ShippingSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    Width,
                    Glue,
                    Lock,
                    SortOrder
                FROM ShippingSettings
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
                    new ShippingSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        Width =
                            reader.IsDBNull(
                                reader.GetOrdinal("Width"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Width")),

                        Glue =
                            reader.IsDBNull(
                                reader.GetOrdinal("Glue"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Glue")),

                        Lock =
                            reader.IsDBNull(
                                reader.GetOrdinal("Lock"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Lock")),

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

        public ShippingSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    Width,
                    Glue,
                    Lock,
                    SortOrder
                FROM ShippingSettings
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


            return new ShippingSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                Width =
                    reader.IsDBNull(
                        reader.GetOrdinal("Width"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Width")),

                Glue =
                    reader.IsDBNull(
                        reader.GetOrdinal("Glue"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Glue")),

                Lock =
                    reader.IsDBNull(
                        reader.GetOrdinal("Lock"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Lock")),

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
            ShippingSetting setting)
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
                INSERT INTO ShippingSettings
                (
                    Width,
                    Glue,
                    Lock,
                    SortOrder
                )
                VALUES
                (
                    @Width,
                    @Glue,
                    @Lock,
                    @SortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@Width",
                setting.Width ?? "");


            command.Parameters.AddWithValue(
                "@Glue",
                setting.Glue);


            command.Parameters.AddWithValue(
                "@Lock",
                setting.Lock);


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            ShippingSetting setting)
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
                UPDATE ShippingSettings
                SET
                    Width = @Width,
                    Glue = @Glue,
                    Lock = @Lock,
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
                "@Width",
                setting.Width ?? "");


            command.Parameters.AddWithValue(
                "@Glue",
                setting.Glue);


            command.Parameters.AddWithValue(
                "@Lock",
                setting.Lock);


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
                DELETE FROM ShippingSettings
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
                DELETE FROM ShippingSettings;
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