using ExcelTool.Database;
using ExcelTool.Infrastructure;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class MoldPriceSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<MoldPriceSetting> GetAll()
        {
            var result =
                new List<MoldPriceSetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    Id,
                    MoldType,
                    PricePerMold,
                    PricePerSquareCm,
                    SortOrder
                FROM MoldPriceSettings
                ORDER BY SortOrder, Id;
            ";

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(
                    new MoldPriceSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        MoldType =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        PricePerMold =
                            reader.IsDBNull(2)
                                ? 0
                                : reader.GetDecimal(2),

                        PricePerSquareCm =
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

        public MoldPriceSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    Id,
                    MoldType,
                    PricePerMold,
                    PricePerSquareCm,
                    SortOrder
                FROM MoldPriceSettings
                WHERE Id = $id;
            ";

            command.Parameters.AddWithValue(
                "$id",
                id);

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new MoldPriceSetting
            {
                Id =
                    reader.GetInt32(0),

                MoldType =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                PricePerMold =
                    reader.IsDBNull(2)
                        ? 0
                        : reader.GetDecimal(2),

                PricePerSquareCm =
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
            MoldPriceSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO MoldPriceSettings
                (
                    MoldType,
                    PricePerMold,
                    PricePerSquareCm,
                    SortOrder
                )
                VALUES
                (
                    $moldType,
                    $pricePerMold,
                    $pricePerSquareCm,
                    $sortOrder
                );
            ";

            command.Parameters.AddWithValue(
                "$moldType",
                setting.MoldType);

            command.Parameters.AddWithValue(
                "$pricePerMold",
                setting.PricePerMold);

            command.Parameters.AddWithValue(
                "$pricePerSquareCm",
                setting.PricePerSquareCm);

            command.Parameters.AddWithValue(
                "$sortOrder",
                setting.SortOrder);

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            MoldPriceSetting setting)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = @"
                UPDATE MoldPriceSettings
                SET
                    MoldType = $moldType,
                    PricePerMold = $pricePerMold,
                    PricePerSquareCm = $pricePerSquareCm,
                    SortOrder = $sortOrder
                WHERE Id = $id;
            ";

            command.Parameters.AddWithValue(
                "$id",
                setting.Id);

            command.Parameters.AddWithValue(
                "$moldType",
                setting.MoldType);

            command.Parameters.AddWithValue(
                "$pricePerMold",
                setting.PricePerMold);

            command.Parameters.AddWithValue(
                "$pricePerSquareCm",
                setting.PricePerSquareCm);

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

            using var command =
                connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM MoldPriceSettings
                WHERE Id = $id;
            ";

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

            using var command =
                connection.CreateCommand();

            command.CommandText =
                "DELETE FROM MoldPriceSettings;";

            command.ExecuteNonQuery();

            return true;
        }
    }
}