using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class BomSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<BomSetting> GetAll()
        {
            var result =
                new List<BomSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    ProductType,
                    Specification,
                    Formula,
                    PrintingType,
                    Process1,
                    Process2,
                    SortOrder
                FROM BomSettings
                ORDER BY SortOrder, Id;
                """;


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            using SqliteDataReader reader =
                command.ExecuteReader();


            while (reader.Read())
            {
                result.Add(
                    new BomSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        ProductType =
                            reader.IsDBNull(
                                reader.GetOrdinal("ProductType"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("ProductType")),

                        Specification =
                            reader.IsDBNull(
                                reader.GetOrdinal("Specification"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Specification")),

                        Formula =
                            reader.IsDBNull(
                                reader.GetOrdinal("Formula"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Formula")),

                        PrintingType =
                            reader.IsDBNull(
                                reader.GetOrdinal("PrintingType"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("PrintingType")),

                        Process1 =
                            reader.IsDBNull(
                                reader.GetOrdinal("Process1"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Process1")),

                        Process2 =
                            reader.IsDBNull(
                                reader.GetOrdinal("Process2"))
                                ? ""
                                : reader.GetString(
                                    reader.GetOrdinal("Process2")),

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

        public BomSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    ProductType,
                    Specification,
                    Formula,
                    PrintingType,
                    Process1,
                    Process2,
                    SortOrder
                FROM BomSettings
                WHERE Id = @Id
                LIMIT 1;
                """;


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


            return new BomSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                ProductType =
                    reader.IsDBNull(
                        reader.GetOrdinal("ProductType"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("ProductType")),

                Specification =
                    reader.IsDBNull(
                        reader.GetOrdinal("Specification"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Specification")),

                Formula =
                    reader.IsDBNull(
                        reader.GetOrdinal("Formula"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Formula")),

                PrintingType =
                    reader.IsDBNull(
                        reader.GetOrdinal("PrintingType"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("PrintingType")),

                Process1 =
                    reader.IsDBNull(
                        reader.GetOrdinal("Process1"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Process1")),

                Process2 =
                    reader.IsDBNull(
                        reader.GetOrdinal("Process2"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("Process2")),

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
            BomSetting setting)
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


            const string sql = """
                INSERT INTO BomSettings
                (
                    ProductType,
                    Specification,
                    Formula,
                    PrintingType,
                    Process1,
                    Process2,
                    SortOrder
                )
                VALUES
                (
                    @ProductType,
                    @Specification,
                    @Formula,
                    @PrintingType,
                    @Process1,
                    @Process2,
                    @SortOrder
                );
                """;


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@ProductType",
                setting.ProductType ?? "");


            command.Parameters.AddWithValue(
                "@Specification",
                setting.Specification ?? "");


            command.Parameters.AddWithValue(
                "@Formula",
                setting.Formula ?? "");


            command.Parameters.AddWithValue(
                "@PrintingType",
                setting.PrintingType ?? "");


            command.Parameters.AddWithValue(
                "@Process1",
                setting.Process1 ?? "");


            command.Parameters.AddWithValue(
                "@Process2",
                setting.Process2 ?? "");


            command.Parameters.AddWithValue(
                "@SortOrder",
                setting.SortOrder);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            BomSetting setting)
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


            const string sql = """
                UPDATE BomSettings
                SET
                    ProductType = @ProductType,
                    Specification = @Specification,
                    Formula = @Formula,
                    PrintingType = @PrintingType,
                    Process1 = @Process1,
                    Process2 = @Process2,
                    SortOrder = @SortOrder
                WHERE Id = @Id;
                """;


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "@Id",
                setting.Id);


            command.Parameters.AddWithValue(
                "@ProductType",
                setting.ProductType ?? "");


            command.Parameters.AddWithValue(
                "@Specification",
                setting.Specification ?? "");


            command.Parameters.AddWithValue(
                "@Formula",
                setting.Formula ?? "");


            command.Parameters.AddWithValue(
                "@PrintingType",
                setting.PrintingType ?? "");


            command.Parameters.AddWithValue(
                "@Process1",
                setting.Process1 ?? "");


            command.Parameters.AddWithValue(
                "@Process2",
                setting.Process2 ?? "");


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


            const string sql = """
                DELETE FROM BomSettings
                WHERE Id = @Id;
                """;


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


            const string sql = """
                DELETE FROM BomSettings;
                """;


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText =
                sql;


            command.ExecuteNonQuery();


            return true;
        }
    }
}