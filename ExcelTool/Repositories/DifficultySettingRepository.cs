using ExcelTool.Database;
using ExcelTool.Models.Pricing;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class DifficultySettingRepository
        : BaseRepository<DifficultySetting>
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public override List<DifficultySetting> GetAll()
        {
            var result =
                new List<DifficultySetting>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    Difficulty,
                    WasteRate,
                    SortOrder
                FROM DifficultySetting
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
                    new DifficultySetting
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        Difficulty =
                            Convert.ToDecimal(
                                reader["Difficulty"]),

                        WasteRate =
                            Convert.ToDecimal(
                                reader["WasteRate"]),

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

        public override DifficultySetting? GetById(
            int id)
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    Difficulty,
                    WasteRate,
                    SortOrder
                FROM DifficultySetting
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

            return new DifficultySetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                Difficulty =
                    Convert.ToDecimal(
                        reader["Difficulty"]),

                WasteRate =
                    Convert.ToDecimal(
                        reader["WasteRate"]),

                SortOrder =
                    reader.GetInt32(
                        reader.GetOrdinal("SortOrder"))
            };
        }


        // =========================================================
        // INSERT
        // =========================================================

        public override bool Insert(
            DifficultySetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                INSERT INTO DifficultySetting
                (
                    Difficulty,
                    WasteRate,
                    SortOrder
                )
                VALUES
                (
                    $difficulty,
                    $wasteRate,
                    $sortOrder
                );
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$difficulty",
                entity.Difficulty);

            command.Parameters.AddWithValue(
                "$wasteRate",
                entity.WasteRate);

            command.Parameters.AddWithValue(
                "$sortOrder",
                entity.SortOrder);

            int affectedRows =
                command.ExecuteNonQuery();

            return affectedRows > 0;
        }


        // =========================================================
        // INSERT MANY
        // =========================================================
        //
        // Dùng cho IMPORT EXCEL
        //
        // Tất cả dữ liệu được insert trong cùng một transaction.
        //
        // Nếu một dòng lỗi:
        //      -> ROLLBACK
        //
        // Nếu tất cả thành công:
        //      -> COMMIT
        //
        // =========================================================

        public int InsertMany(
            IEnumerable<DifficultySetting> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(
                    nameof(entities));


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            using var transaction =
                connection.BeginTransaction();


            try
            {
                const string sql = """
                    INSERT INTO DifficultySetting
                    (
                        Difficulty,
                        WasteRate,
                        SortOrder
                    )
                    VALUES
                    (
                        $difficulty,
                        $wasteRate,
                        $sortOrder
                    );
                    """;


                using var command =
                    connection.CreateCommand();

                command.CommandText = sql;

                command.Transaction =
                    transaction;


                // -------------------------------------------------
                // Tạo parameter một lần
                // -------------------------------------------------

                var difficultyParameter =
                    command.Parameters.Add(
                        "$difficulty",
                        SqliteType.Real);

                var wasteRateParameter =
                    command.Parameters.Add(
                        "$wasteRate",
                        SqliteType.Real);

                var sortOrderParameter =
                    command.Parameters.Add(
                        "$sortOrder",
                        SqliteType.Integer);


                int insertedCount = 0;


                foreach (
                    DifficultySetting entity
                    in entities)
                {
                    if (entity == null)
                        continue;


                    difficultyParameter.Value =
                        entity.Difficulty;

                    wasteRateParameter.Value =
                        entity.WasteRate;

                    sortOrderParameter.Value =
                        entity.SortOrder;


                    int affectedRows =
                        command.ExecuteNonQuery();


                    if (affectedRows > 0)
                    {
                        insertedCount++;
                    }
                }


                // -------------------------------------------------
                // COMMIT
                // -------------------------------------------------

                transaction.Commit();


                return insertedCount;
            }
            catch
            {
                // -------------------------------------------------
                // ROLLBACK
                // -------------------------------------------------

                transaction.Rollback();

                throw;
            }
        }


        // =========================================================
        // CLEAR ALL
        // =========================================================
        //
        // Xóa toàn bộ dữ liệu DifficultySetting.
        //
        // Dùng trước khi IMPORT LẠI toàn bộ dữ liệu từ Excel.
        //
        // =========================================================

        public int ClearAll()
        {
            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                DELETE FROM DifficultySetting;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            return command.ExecuteNonQuery();
        }

        // =========================================================
        // REPLACE ALL
        // =========================================================
        //
        // Dùng cho IMPORT EXCEL.
        //
        // Xóa toàn bộ dữ liệu cũ và insert dữ liệu mới
        // trong CÙNG một transaction.
        //
        // Nếu insert lỗi:
        //      -> ROLLBACK
        //      -> dữ liệu cũ vẫn còn
        //
        // Nếu thành công:
        //      -> COMMIT
        //
        // =========================================================

        public int ReplaceAll(
            IEnumerable<DifficultySetting> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(
                    nameof(entities));


            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();


            using var transaction =
                connection.BeginTransaction();


            try
            {
                // =====================================================
                // XÓA DỮ LIỆU CŨ
                // =====================================================

                const string deleteSql = """
            DELETE FROM DifficultySetting;
            """;


                using var deleteCommand =
                    connection.CreateCommand();

                deleteCommand.CommandText =
                    deleteSql;

                deleteCommand.Transaction =
                    transaction;

                deleteCommand.ExecuteNonQuery();


                // =====================================================
                // INSERT DỮ LIỆU MỚI
                // =====================================================

                const string insertSql = """
            INSERT INTO DifficultySetting
            (
                Difficulty,
                WasteRate,
                SortOrder
            )
            VALUES
            (
                $difficulty,
                $wasteRate,
                $sortOrder
            );
            """;


                using var insertCommand =
                    connection.CreateCommand();

                insertCommand.CommandText =
                    insertSql;

                insertCommand.Transaction =
                    transaction;


                // -----------------------------------------------------
                // PARAMETERS
                // -----------------------------------------------------

                var difficultyParameter =
                    insertCommand.Parameters.Add(
                        "$difficulty",
                        SqliteType.Real);


                var wasteRateParameter =
                    insertCommand.Parameters.Add(
                        "$wasteRate",
                        SqliteType.Real);


                var sortOrderParameter =
                    insertCommand.Parameters.Add(
                        "$sortOrder",
                        SqliteType.Integer);


                // =====================================================
                // INSERT
                // =====================================================

                int insertedCount = 0;


                foreach (
                    DifficultySetting entity
                    in entities)
                {
                    if (entity == null)
                        continue;


                    difficultyParameter.Value =
                        entity.Difficulty;


                    wasteRateParameter.Value =
                        entity.WasteRate;


                    sortOrderParameter.Value =
                        entity.SortOrder;


                    int affectedRows =
                        insertCommand.ExecuteNonQuery();


                    if (affectedRows > 0)
                    {
                        insertedCount++;
                    }
                }


                // =====================================================
                // COMMIT
                // =====================================================

                transaction.Commit();


                return insertedCount;
            }
            catch
            {
                // =====================================================
                // ROLLBACK
                // =====================================================

                transaction.Rollback();

                throw;
            }
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public override bool Update(
            DifficultySetting entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                UPDATE DifficultySetting
                SET
                    Difficulty = $difficulty,
                    WasteRate = $wasteRate,
                    SortOrder = $sortOrder
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                entity.Id);

            command.Parameters.AddWithValue(
                "$difficulty",
                entity.Difficulty);

            command.Parameters.AddWithValue(
                "$wasteRate",
                entity.WasteRate);

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
                DELETE FROM DifficultySetting
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
    }
}