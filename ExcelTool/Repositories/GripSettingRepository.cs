using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class GripSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<GripSetting> GetAll()
        {
            var result =
                new List<GripSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    SettingName,
                    Value,
                    SortOrder
                FROM GripSettings
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
                    new GripSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        SettingName =
                            reader.IsDBNull(
                                reader.GetOrdinal("SettingName"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("SettingName")),

                        Value =
                            reader.IsDBNull(
                                reader.GetOrdinal("Value"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Value")),

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

        public GripSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    SettingName,
                    Value,
                    SortOrder
                FROM GripSettings
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


            return new GripSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                SettingName =
                    reader.IsDBNull(
                        reader.GetOrdinal("SettingName"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("SettingName")),

                Value =
                    reader.IsDBNull(
                        reader.GetOrdinal("Value"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Value")),

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
            GripSetting setting)
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
                INSERT INTO GripSettings
                (
                    SettingName,
                    Value,
                    SortOrder
                )
                VALUES
                (
                    @SettingName,
                    @Value,
                    @SortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;


            command.Parameters.AddWithValue(
                "@SettingName",
                setting.SettingName ?? "");


            command.Parameters.AddWithValue(
                "@Value",
                setting.Value);


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            GripSetting setting)
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
                UPDATE GripSettings
                SET
                    SettingName = @SettingName,
                    Value = @Value,
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
                "@SettingName",
                setting.SettingName ?? "");


            command.Parameters.AddWithValue(
                "@Value",
                setting.Value);


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
                DELETE FROM GripSettings
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
                DELETE FROM GripSettings;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;


            command.ExecuteNonQuery();


            return true;
        }
    }
}