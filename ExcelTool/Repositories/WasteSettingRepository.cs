using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class WasteSettingRepository
        : BaseRepository<WasteSetting>
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public override List<WasteSetting> GetAll()
        {
            var result =
                new List<WasteSetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    SoTo,
                    Mau01,
                    Mau02,
                    Mau03,
                    Mau04,
                    Mau05,
                    Mau06,
                    Mau07,
                    CanMang,
                    TrangPhu,
                    Be,
                    CanVan,
                    EpNhu,
                    Boi,
                    DongKim,
                    Cat,
                    Dan,
                    SortOrder
                FROM WasteSetting
                ORDER BY SortOrder, Id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

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

        public override WasteSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    SoTo,
                    Mau01,
                    Mau02,
                    Mau03,
                    Mau04,
                    Mau05,
                    Mau06,
                    Mau07,
                    CanMang,
                    TrangPhu,
                    Be,
                    CanVan,
                    EpNhu,
                    Boi,
                    DongKim,
                    Cat,
                    Dan,
                    SortOrder
                FROM WasteSetting
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                id);

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapReaderToEntity(reader);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public override bool Insert(
            WasteSetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                INSERT INTO WasteSetting
                (
                    SoTo,
                    Mau01,
                    Mau02,
                    Mau03,
                    Mau04,
                    Mau05,
                    Mau06,
                    Mau07,
                    CanMang,
                    TrangPhu,
                    Be,
                    CanVan,
                    EpNhu,
                    Boi,
                    DongKim,
                    Cat,
                    Dan,
                    SortOrder
                )
                VALUES
                (
                    $soTo,
                    $mau01,
                    $mau02,
                    $mau03,
                    $mau04,
                    $mau05,
                    $mau06,
                    $mau07,
                    $canMang,
                    $trangPhu,
                    $be,
                    $canVan,
                    $epNhu,
                    $boi,
                    $dongKim,
                    $cat,
                    $dan,
                    $sortOrder
                );
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            AddParameters(
                command,
                entity);

            int affectedRows =
                command.ExecuteNonQuery();

            return affectedRows > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public override bool Update(
            WasteSetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            const string sql = """
                UPDATE WasteSetting
                SET
                    SoTo = $soTo,
                    Mau01 = $mau01,
                    Mau02 = $mau02,
                    Mau03 = $mau03,
                    Mau04 = $mau04,
                    Mau05 = $mau05,
                    Mau06 = $mau06,
                    Mau07 = $mau07,
                    CanMang = $canMang,
                    TrangPhu = $trangPhu,
                    Be = $be,
                    CanVan = $canVan,
                    EpNhu = $epNhu,
                    Boi = $boi,
                    DongKim = $dongKim,
                    Cat = $cat,
                    Dan = $dan,
                    SortOrder = $sortOrder
                WHERE Id = $id;
                """;

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                entity.Id);

            AddParameters(
                command,
                entity);

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
                DELETE FROM WasteSetting
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                id);

            int affectedRows =
                command.ExecuteNonQuery();

            return affectedRows > 0;
        }


        // =========================================================
        // MAP READER → MODEL
        // =========================================================

        private static WasteSetting MapReaderToEntity(
            SqliteDataReader reader)
        {
            return new WasteSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                SoTo =
                    reader["SoTo"]?.ToString() ?? "",

                Mau01 =
                    Convert.ToDecimal(
                        reader["Mau01"]),

                Mau02 =
                    Convert.ToDecimal(
                        reader["Mau02"]),

                Mau03 =
                    Convert.ToDecimal(
                        reader["Mau03"]),

                Mau04 =
                    Convert.ToDecimal(
                        reader["Mau04"]),

                Mau05 =
                    Convert.ToDecimal(
                        reader["Mau05"]),

                Mau06 =
                    Convert.ToDecimal(
                        reader["Mau06"]),

                Mau07 =
                    Convert.ToDecimal(
                        reader["Mau07"]),

                CanMang =
                    Convert.ToDecimal(
                        reader["CanMang"]),

                TrangPhu =
                    Convert.ToDecimal(
                        reader["TrangPhu"]),

                Be =
                    Convert.ToDecimal(
                        reader["Be"]),

                CanVan =
                    Convert.ToDecimal(
                        reader["CanVan"]),

                EpNhu =
                    Convert.ToDecimal(
                        reader["EpNhu"]),

                Boi =
                    Convert.ToDecimal(
                        reader["Boi"]),

                DongKim =
                    Convert.ToDecimal(
                        reader["DongKim"]),

                Cat =
                    Convert.ToDecimal(
                        reader["Cat"]),

                Dan =
                    Convert.ToDecimal(
                        reader["Dan"]),

                SortOrder =
                    reader.GetInt32(
                        reader.GetOrdinal("SortOrder"))
            };
        }


        // =========================================================
        // PARAMETERS
        // =========================================================

        private static void AddParameters(
            SqliteCommand command,
            WasteSetting entity)
        {
            command.Parameters.AddWithValue(
                "$soTo",
                entity.SoTo);

            command.Parameters.AddWithValue(
                "$mau01",
                entity.Mau01);

            command.Parameters.AddWithValue(
                "$mau02",
                entity.Mau02);

            command.Parameters.AddWithValue(
                "$mau03",
                entity.Mau03);

            command.Parameters.AddWithValue(
                "$mau04",
                entity.Mau04);

            command.Parameters.AddWithValue(
                "$mau05",
                entity.Mau05);

            command.Parameters.AddWithValue(
                "$mau06",
                entity.Mau06);

            command.Parameters.AddWithValue(
                "$mau07",
                entity.Mau07);

            command.Parameters.AddWithValue(
                "$canMang",
                entity.CanMang);

            command.Parameters.AddWithValue(
                "$trangPhu",
                entity.TrangPhu);

            command.Parameters.AddWithValue(
                "$be",
                entity.Be);

            command.Parameters.AddWithValue(
                "$canVan",
                entity.CanVan);

            command.Parameters.AddWithValue(
                "$epNhu",
                entity.EpNhu);

            command.Parameters.AddWithValue(
                "$boi",
                entity.Boi);

            command.Parameters.AddWithValue(
                "$dongKim",
                entity.DongKim);

            command.Parameters.AddWithValue(
                "$cat",
                entity.Cat);

            command.Parameters.AddWithValue(
                "$dan",
                entity.Dan);

            command.Parameters.AddWithValue(
                "$sortOrder",
                entity.SortOrder);
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
        DELETE FROM WasteSetting;
        """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();
        }
    }
}