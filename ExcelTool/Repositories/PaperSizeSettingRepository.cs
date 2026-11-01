using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using ExcelTool.Database;
using ExcelTool.Models.Pricing;

namespace ExcelTool.Repositories
{
    public class PaperSizeSettingRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<PaperSizeSetting> GetAll()
        {
            var result =
                new List<PaperSizeSetting>();


            using SqliteConnection connection =
    new SqliteConnection(
        $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                SELECT
                    Id,
                    PrintSize,
                    PrinterSize,
                    PlateSize,
                    SortOrder
                FROM PaperSizeSetting
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
                    new PaperSizeSetting
                    {
                        Id =
                            reader.GetInt32(0),

                        PrintSize =
                            reader.IsDBNull(1)
                                ? ""
                                : reader.GetString(1),

                        PrinterSize =
                            reader.IsDBNull(2)
                                ? ""
                                : reader.GetString(2),

                        PlateSize =
                            reader.IsDBNull(3)
                                ? ""
                                : reader.GetString(3),

                        SortOrder =
                            reader.IsDBNull(4)
                                ? 0
                                : reader.GetInt32(4)
                    });
            }


            return result;
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            PaperSizeSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(
                    nameof(setting));


            using SqliteConnection connection =
    new SqliteConnection(
        $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                INSERT INTO PaperSizeSetting
                (
                    PrintSize,
                    PrinterSize,
                    PlateSize,
                    SortOrder
                )
                VALUES
                (
                    $printSize,
                    $printerSize,
                    $plateSize,
                    $sortOrder
                );
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;


            command.Parameters.AddWithValue(
                "$printSize",
                setting.PrintSize ?? "");

            command.Parameters.AddWithValue(
                "$printerSize",
                setting.PrinterSize ?? "");

            command.Parameters.AddWithValue(
                "$plateSize",
                setting.PlateSize ?? "");

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
            using SqliteConnection connection =
    new SqliteConnection(
        $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                DELETE FROM PaperSizeSetting
                WHERE Id = $id;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;


            command.Parameters.AddWithValue(
                "$id",
                id);


            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // DELETE ALL
        // DÙNG KHI IMPORT LẠI EXCEL
        // =========================================================

        public void DeleteAll()
        {
            using SqliteConnection connection =
    new SqliteConnection(
        $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                DELETE FROM PaperSizeSetting;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();
        }


        // =========================================================
        // EXISTS
        // =========================================================

        public bool Exists(
            string printSize,
            string printerSize,
            string plateSize)
        {
            using SqliteConnection connection =
    new SqliteConnection(
        $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            const string sql = @"
                SELECT COUNT(1)
                FROM PaperSizeSetting
                WHERE PrintSize = $printSize
                  AND PrinterSize = $printerSize
                  AND PlateSize = $plateSize;
            ";


            using SqliteCommand command =
                connection.CreateCommand();

            command.CommandText = sql;


            command.Parameters.AddWithValue(
                "$printSize",
                printSize ?? "");

            command.Parameters.AddWithValue(
                "$printerSize",
                printerSize ?? "");

            command.Parameters.AddWithValue(
                "$plateSize",
                plateSize ?? "");


            long count =
                (long)(command.ExecuteScalar() ?? 0);


            return count > 0;
        }
    }
}