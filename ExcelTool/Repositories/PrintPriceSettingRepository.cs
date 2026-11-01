using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class PrintPriceSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<PrintPriceSetting> GetAll()
        {
            var result =
                new List<PrintPriceSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    MachineSize,
                    PricePerPlate,
                    Cmyk,
                    Pantone,
                    CmykJobPrice,
                    SpotColorJobPrice,
                    SortOrder
                FROM PrintPriceSettings
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
                    new PrintPriceSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        MachineSize =
                            reader.IsDBNull(
                                reader.GetOrdinal("MachineSize"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("MachineSize")),

                        PricePerPlate =
                            reader.IsDBNull(
                                reader.GetOrdinal("PricePerPlate"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("PricePerPlate")),

                        Cmyk =
                            reader.IsDBNull(
                                reader.GetOrdinal("Cmyk"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Cmyk")),

                        Pantone =
                            reader.IsDBNull(
                                reader.GetOrdinal("Pantone"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("Pantone")),

                        CmykJobPrice =
                            reader.IsDBNull(
                                reader.GetOrdinal("CmykJobPrice"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("CmykJobPrice")),

                        SpotColorJobPrice =
                            reader.IsDBNull(
                                reader.GetOrdinal("SpotColorJobPrice"))
                                ? 0
                                : reader.GetDecimal(
                                    reader.GetOrdinal("SpotColorJobPrice")),

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

        public PrintPriceSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    MachineSize,
                    PricePerPlate,
                    Cmyk,
                    Pantone,
                    CmykJobPrice,
                    SpotColorJobPrice,
                    SortOrder
                FROM PrintPriceSettings
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


            return new PrintPriceSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                MachineSize =
                    reader.IsDBNull(
                        reader.GetOrdinal("MachineSize"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("MachineSize")),

                PricePerPlate =
                    reader.IsDBNull(
                        reader.GetOrdinal("PricePerPlate"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("PricePerPlate")),

                Cmyk =
                    reader.IsDBNull(
                        reader.GetOrdinal("Cmyk"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Cmyk")),

                Pantone =
                    reader.IsDBNull(
                        reader.GetOrdinal("Pantone"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("Pantone")),

                CmykJobPrice =
                    reader.IsDBNull(
                        reader.GetOrdinal("CmykJobPrice"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("CmykJobPrice")),

                SpotColorJobPrice =
                    reader.IsDBNull(
                        reader.GetOrdinal("SpotColorJobPrice"))
                        ? 0
                        : reader.GetDecimal(
                            reader.GetOrdinal("SpotColorJobPrice")),

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
            PrintPriceSetting setting)
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
                INSERT INTO PrintPriceSettings
                (
                    MachineSize,
                    PricePerPlate,
                    Cmyk,
                    Pantone,
                    CmykJobPrice,
                    SpotColorJobPrice,
                    SortOrder
                )
                VALUES
                (
                    @MachineSize,
                    @PricePerPlate,
                    @Cmyk,
                    @Pantone,
                    @CmykJobPrice,
                    @SpotColorJobPrice,
                    @SortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@MachineSize",
                setting.MachineSize ?? "");


            command.Parameters.AddWithValue(
                "@PricePerPlate",
                setting.PricePerPlate);


            command.Parameters.AddWithValue(
                "@Cmyk",
                setting.Cmyk);


            command.Parameters.AddWithValue(
                "@Pantone",
                setting.Pantone);


            command.Parameters.AddWithValue(
                "@CmykJobPrice",
                setting.CmykJobPrice);


            command.Parameters.AddWithValue(
                "@SpotColorJobPrice",
                setting.SpotColorJobPrice);


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            PrintPriceSetting setting)
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
                UPDATE PrintPriceSettings
                SET
                    MachineSize = @MachineSize,
                    PricePerPlate = @PricePerPlate,
                    Cmyk = @Cmyk,
                    Pantone = @Pantone,
                    CmykJobPrice = @CmykJobPrice,
                    SpotColorJobPrice = @SpotColorJobPrice,
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
                "@MachineSize",
                setting.MachineSize ?? "");


            command.Parameters.AddWithValue(
                "@PricePerPlate",
                setting.PricePerPlate);


            command.Parameters.AddWithValue(
                "@Cmyk",
                setting.Cmyk);


            command.Parameters.AddWithValue(
                "@Pantone",
                setting.Pantone);


            command.Parameters.AddWithValue(
                "@CmykJobPrice",
                setting.CmykJobPrice);


            command.Parameters.AddWithValue(
                "@SpotColorJobPrice",
                setting.SpotColorJobPrice);


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
                DELETE FROM PrintPriceSettings
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
                DELETE FROM PrintPriceSettings;
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