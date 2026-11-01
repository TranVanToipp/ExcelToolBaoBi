using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class ReprintSettingRepository
        : BaseRepository<ReprintSetting>
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public override List<ReprintSetting> GetAll()
        {
            var result =
                new List<ReprintSetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    CheckReprint,
                    MoldChange,
                    SortOrder
                FROM ReprintSetting
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
                    new ReprintSetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        CheckReprint =
                            reader["CheckReprint"]?.ToString()
                            ?? "",

                        MoldChange =
                            reader["MoldChange"]?.ToString()
                            ?? "",

                        SortOrder =
                            reader.GetInt32(
                                reader.GetOrdinal("SortOrder"))
                    });
            }

            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public override ReprintSetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    CheckReprint,
                    MoldChange,
                    SortOrder
                FROM ReprintSetting
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
                return null;

            return new ReprintSetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                CheckReprint =
                    reader["CheckReprint"]?.ToString()
                    ?? "",

                MoldChange =
                    reader["MoldChange"]?.ToString()
                    ?? "",

                SortOrder =
                    reader.GetInt32(
                        reader.GetOrdinal("SortOrder"))
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public override bool Insert(
            ReprintSetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                INSERT INTO ReprintSetting
                (
                    CheckReprint,
                    MoldChange,
                    SortOrder
                )
                VALUES
                (
                    $checkReprint,
                    $moldChange,
                    $sortOrder
                );
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText =
                sql;

            command.Parameters.AddWithValue(
                "$checkReprint",
                entity.CheckReprint);

            command.Parameters.AddWithValue(
                "$moldChange",
                entity.MoldChange);

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
            ReprintSetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                UPDATE ReprintSetting
                SET
                    CheckReprint = $checkReprint,
                    MoldChange = $moldChange,
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
                "$checkReprint",
                entity.CheckReprint);

            command.Parameters.AddWithValue(
                "$moldChange",
                entity.MoldChange);

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
                DELETE FROM ReprintSetting
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
    }
}