using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class PaperPriceSettingRepository
        : BaseRepository<PaperPriceSetting>
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public override List<PaperPriceSetting> GetAll()
        {
            var result =
                new List<PaperPriceSetting>();


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Loai,
                    MaGiay,
                    TenGiay,
                    VndPerM2,
                    VndPerTon,
                    Size79x109,
                    Size65x86,
                    Size65x84,
                    Size80x120,
                    Size79x54,
                    Size39x54,
                    Size60x80,
                    Size65x43,
                    Size60x42,
                    SortOrder
                FROM PaperPriceSetting
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
                    MapReaderToEntity(reader));
            }


            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public override PaperPriceSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                SELECT
                    Id,
                    Loai,
                    MaGiay,
                    TenGiay,
                    VndPerM2,
                    VndPerTon,
                    Size79x109,
                    Size65x86,
                    Size65x84,
                    Size80x120,
                    Size79x54,
                    Size39x54,
                    Size60x80,
                    Size65x43,
                    Size60x42,
                    SortOrder
                FROM PaperPriceSetting
                WHERE Id = $id;
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


            return MapReaderToEntity(reader);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public override bool Insert(
            PaperPriceSetting entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(
                    nameof(entity));
            }


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                INSERT INTO PaperPriceSetting
                (
                    Loai,
                    MaGiay,
                    TenGiay,
                    VndPerM2,
                    VndPerTon,
                    Size79x109,
                    Size65x86,
                    Size65x84,
                    Size80x120,
                    Size79x54,
                    Size39x54,
                    Size60x80,
                    Size65x43,
                    Size60x42,
                    SortOrder
                )
                VALUES
                (
                    $loai,
                    $maGiay,
                    $tenGiay,
                    $vndPerM2,
                    $vndPerTon,
                    $size79x109,
                    $size65x86,
                    $size65x84,
                    $size80x120,
                    $size79x54,
                    $size39x54,
                    $size60x80,
                    $size65x43,
                    $size60x42,
                    $sortOrder
                );
                """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$loai",
                entity.Loai);


            command.Parameters.AddWithValue(
                "$maGiay",
                entity.MaGiay);


            command.Parameters.AddWithValue(
                "$tenGiay",
                entity.TenGiay);


            command.Parameters.AddWithValue(
                "$vndPerM2",
                entity.VndPerM2);


            command.Parameters.AddWithValue(
                "$vndPerTon",
                entity.VndPerTon);


            command.Parameters.AddWithValue(
                "$size79x109",
                entity.Size79x109);


            command.Parameters.AddWithValue(
                "$size65x86",
                entity.Size65x86);


            command.Parameters.AddWithValue(
                "$size65x84",
                entity.Size65x84);


            command.Parameters.AddWithValue(
                "$size80x120",
                entity.Size80x120);


            command.Parameters.AddWithValue(
                "$size79x54",
                entity.Size79x54);


            command.Parameters.AddWithValue(
                "$size39x54",
                entity.Size39x54);


            command.Parameters.AddWithValue(
                "$size60x80",
                entity.Size60x80);


            command.Parameters.AddWithValue(
                "$size65x43",
                entity.Size65x43);


            command.Parameters.AddWithValue(
                "$size60x42",
                entity.Size60x42);


            command.Parameters.AddWithValue(
                "$sortOrder",
                entity.SortOrder);


            int affectedRows =
                command.ExecuteNonQuery();


            return affectedRows > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public override bool Update(
            PaperPriceSetting entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(
                    nameof(entity));
            }


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                UPDATE PaperPriceSetting
                SET
                    Loai = $loai,
                    MaGiay = $maGiay,
                    TenGiay = $tenGiay,
                    VndPerM2 = $vndPerM2,
                    VndPerTon = $vndPerTon,
                    Size79x109 = $size79x109,
                    Size65x86 = $size65x86,
                    Size65x84 = $size65x84,
                    Size80x120 = $size80x120,
                    Size79x54 = $size79x54,
                    Size39x54 = $size39x54,
                    Size60x80 = $size60x80,
                    Size65x43 = $size65x43,
                    Size60x42 = $size60x42,
                    SortOrder = $sortOrder
                WHERE Id = $id;
                """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$id",
                entity.Id);


            command.Parameters.AddWithValue(
                "$loai",
                entity.Loai);


            command.Parameters.AddWithValue(
                "$maGiay",
                entity.MaGiay);


            command.Parameters.AddWithValue(
                "$tenGiay",
                entity.TenGiay);


            command.Parameters.AddWithValue(
                "$vndPerM2",
                entity.VndPerM2);


            command.Parameters.AddWithValue(
                "$vndPerTon",
                entity.VndPerTon);


            command.Parameters.AddWithValue(
                "$size79x109",
                entity.Size79x109);


            command.Parameters.AddWithValue(
                "$size65x86",
                entity.Size65x86);


            command.Parameters.AddWithValue(
                "$size65x84",
                entity.Size65x84);


            command.Parameters.AddWithValue(
                "$size80x120",
                entity.Size80x120);


            command.Parameters.AddWithValue(
                "$size79x54",
                entity.Size79x54);


            command.Parameters.AddWithValue(
                "$size39x54",
                entity.Size39x54);


            command.Parameters.AddWithValue(
                "$size60x80",
                entity.Size60x80);


            command.Parameters.AddWithValue(
                "$size65x43",
                entity.Size65x43);


            command.Parameters.AddWithValue(
                "$size60x42",
                entity.Size60x42);


            command.Parameters.AddWithValue(
                "$sortOrder",
                entity.SortOrder);


            int affectedRows =
                command.ExecuteNonQuery();


            return affectedRows > 0;
        }


        // =========================================================
        // DELETE
        // =========================================================

        public override bool Delete(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                DELETE FROM PaperPriceSetting
                WHERE Id = $id;
                """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.Parameters.AddWithValue(
                "$id",
                id);


            int affectedRows =
                command.ExecuteNonQuery();


            return affectedRows > 0;
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public void DeleteAll()
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");


            connection.Open();


            const string sql = """
                DELETE FROM PaperPriceSetting;
                """;


            using var command =
                connection.CreateCommand();


            command.CommandText =
                sql;


            command.ExecuteNonQuery();
        }


        // =========================================================
        // MAP READER → MODEL
        // =========================================================

        private static PaperPriceSetting MapReaderToEntity(
            SqliteDataReader reader)
        {
            return new PaperPriceSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),


                Loai =
                    reader["Loai"]?.ToString()
                    ?? "",


                MaGiay =
                    reader["MaGiay"]?.ToString()
                    ?? "",


                TenGiay =
                    reader["TenGiay"]?.ToString()
                    ?? "",


                VndPerM2 =
                    Convert.ToDecimal(
                        reader["VndPerM2"]),


                VndPerTon =
                    Convert.ToDecimal(
                        reader["VndPerTon"]),


                Size79x109 =
                    Convert.ToDecimal(
                        reader["Size79x109"]),


                Size65x86 =
                    Convert.ToDecimal(
                        reader["Size65x86"]),


                Size65x84 =
                    Convert.ToDecimal(
                        reader["Size65x84"]),


                Size80x120 =
                    Convert.ToDecimal(
                        reader["Size80x120"]),


                Size79x54 =
                    Convert.ToDecimal(
                        reader["Size79x54"]),


                Size39x54 =
                    Convert.ToDecimal(
                        reader["Size39x54"]),


                Size60x80 =
                    Convert.ToDecimal(
                        reader["Size60x80"]),


                Size65x43 =
                    Convert.ToDecimal(
                        reader["Size65x43"]),


                Size60x42 =
                    Convert.ToDecimal(
                        reader["Size60x42"]),


                SortOrder =
                    reader.GetInt32(
                        reader.GetOrdinal("SortOrder"))
            };
        }
    }
}