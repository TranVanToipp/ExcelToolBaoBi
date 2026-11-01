using ExcelTool.Database;
using ExcelTool.Models;
using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;

namespace ExcelTool.Repositories
{
    public class ProductRepository
    {
        // =========================================================
        // GET ALL
        // =========================================================

        public List<Product> GetAll()
        {
            var result =
                new List<Product>();

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    ProductCode,
                    CreatedDate,
                    CustomerId,
                    ProductName,
                    ProductType,
                    ReprintType,
                    Specification,
                    SpreadSize,
                    Quantity,
                    Difficulty,
                    QuotationDeadline,
                    Note,
                    Unit,
                    UnitPrice,
                    Description
                FROM Product
                ORDER BY CreatedDate DESC, Id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(
                    Map(reader));
            }

            return result;
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public Product? GetById(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                SELECT
                    Id,
                    ProductCode,
                    CreatedDate,
                    CustomerId,
                    ProductName,
                    ProductType,
                    ReprintType,
                    Specification,
                    SpreadSize,
                    Quantity,
                    Difficulty,
                    QuotationDeadline,
                    Note,
                    Unit,
                    UnitPrice,
                    Description
                FROM Product
                WHERE Id = $id
                LIMIT 1;
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
            {
                return null;
            }

            return Map(reader);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(
                    nameof(product));
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                INSERT INTO Product
                (
                    Id,
                    ProductCode,
                    CreatedDate,
                    CustomerId,
                    ProductName,
                    ProductType,
                    ReprintType,
                    Specification,
                    SpreadSize,
                    Quantity,
                    Difficulty,
                    QuotationDeadline,
                    Note,
                    Unit,
                    UnitPrice,
                    Description
                )
                VALUES
                (
                    $id,
                    $productCode,
                    $createdDate,
                    $customerId,
                    $productName,
                    $productType,
                    $reprintType,
                    $specification,
                    $spreadSize,
                    $quantity,
                    $difficulty,
                    $quotationDeadline,
                    $note,
                    $unit,
                    $unitPrice,
                    $description
                );
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                product.Id ?? "");

            command.Parameters.AddWithValue(
                "$productCode",
                product.ProductCode ?? "");

            command.Parameters.AddWithValue(
                "$createdDate",
                product.CreatedDate.ToString(
                    "yyyy-MM-dd HH:mm:ss"));

            command.Parameters.AddWithValue(
                "$customerId",
                product.CustomerId ?? "");

            command.Parameters.AddWithValue(
                "$productName",
                product.ProductName ?? "");

            command.Parameters.AddWithValue(
                "$productType",
                product.ProductType ?? "");

            command.Parameters.AddWithValue(
                "$reprintType",
                product.ReprintType ?? "");

            command.Parameters.AddWithValue(
                "$specification",
                product.Specification ?? "");

            command.Parameters.AddWithValue(
                "$spreadSize",
                product.SpreadSize ?? "");

            command.Parameters.AddWithValue(
                "$quantity",
                product.Quantity);

            command.Parameters.AddWithValue(
                "$difficulty",
                product.Difficulty);

            command.Parameters.AddWithValue(
                "$quotationDeadline",
                product.QuotationDeadline.HasValue
                    ? product.QuotationDeadline.Value
                        .ToString("yyyy-MM-dd")
                    : (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "$note",
                product.Note ?? "");

            command.Parameters.AddWithValue(
                "$unit",
                product.Unit ?? "");

            command.Parameters.AddWithValue(
                "$unitPrice",
                product.UnitPrice);

            command.Parameters.AddWithValue(
                "$description",
                product.Description ?? "");

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(
                    nameof(product));
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                UPDATE Product
                SET
                    ProductCode = $productCode,
                    CreatedDate = $createdDate,
                    CustomerId = $customerId,
                    ProductName = $productName,
                    ProductType = $productType,
                    ReprintType = $reprintType,
                    Specification = $specification,
                    SpreadSize = $spreadSize,
                    Quantity = $quantity,
                    Difficulty = $difficulty,
                    QuotationDeadline = $quotationDeadline,
                    Note = $note,
                    Unit = $unit,
                    UnitPrice = $unitPrice,
                    Description = $description
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.Parameters.AddWithValue(
                "$id",
                product.Id ?? "");

            command.Parameters.AddWithValue(
                "$productCode",
                product.ProductCode ?? "");

            command.Parameters.AddWithValue(
                "$createdDate",
                product.CreatedDate.ToString(
                    "yyyy-MM-dd HH:mm:ss"));

            command.Parameters.AddWithValue(
                "$customerId",
                product.CustomerId ?? "");

            command.Parameters.AddWithValue(
                "$productName",
                product.ProductName ?? "");

            command.Parameters.AddWithValue(
                "$productType",
                product.ProductType ?? "");

            command.Parameters.AddWithValue(
                "$reprintType",
                product.ReprintType ?? "");

            command.Parameters.AddWithValue(
                "$specification",
                product.Specification ?? "");

            command.Parameters.AddWithValue(
                "$spreadSize",
                product.SpreadSize ?? "");

            command.Parameters.AddWithValue(
                "$quantity",
                product.Quantity);

            command.Parameters.AddWithValue(
                "$difficulty",
                product.Difficulty);

            command.Parameters.AddWithValue(
                "$quotationDeadline",
                product.QuotationDeadline.HasValue
                    ? product.QuotationDeadline.Value
                        .ToString("yyyy-MM-dd")
                    : (object)DBNull.Value);

            command.Parameters.AddWithValue(
                "$note",
                product.Note ?? "");

            command.Parameters.AddWithValue(
                "$unit",
                product.Unit ?? "");

            command.Parameters.AddWithValue(
                "$unitPrice",
                product.UnitPrice);

            command.Parameters.AddWithValue(
                "$description",
                product.Description ?? "");

            return command.ExecuteNonQuery() > 0;
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            using var connection =
                new SqliteConnection(
                    $"Data Source={DatabaseInitializer.DatabasePath}");

            connection.Open();

            const string sql = """
                DELETE FROM Product
                WHERE Id = $id;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

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

            const string sql = """
                DELETE FROM Product;
                """;

            using var command =
                connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();

            return true;
        }


        // =========================================================
        // MAP READER → MODEL
        // =========================================================

        private static Product Map(
            SqliteDataReader reader)
        {
            return new Product
            {
                Id =
                    reader.IsDBNull(0)
                        ? ""
                        : reader.GetString(0),

                ProductCode =
                    reader.IsDBNull(1)
                        ? ""
                        : reader.GetString(1),

                CreatedDate =
                    ParseDateTime(
                        reader.IsDBNull(2)
                            ? null
                            : reader.GetString(2)),

                CustomerId =
                    reader.IsDBNull(3)
                        ? ""
                        : reader.GetString(3),

                ProductName =
                    reader.IsDBNull(4)
                        ? ""
                        : reader.GetString(4),

                ProductType =
                    reader.IsDBNull(5)
                        ? ""
                        : reader.GetString(5),

                ReprintType =
                    reader.IsDBNull(6)
                        ? ""
                        : reader.GetString(6),

                Specification =
                    reader.IsDBNull(7)
                        ? ""
                        : reader.GetString(7),

                SpreadSize =
                    reader.IsDBNull(8)
                        ? ""
                        : reader.GetString(8),

                Quantity =
                    reader.IsDBNull(9)
                        ? 0
                        : reader.GetInt32(9),

                Difficulty =
                    reader.IsDBNull(10)
                        ? 0
                        : reader.GetInt32(10),

                QuotationDeadline =
                    ParseNullableDateTime(
                        reader.IsDBNull(11)
                            ? null
                            : reader.GetString(11)),

                Note =
                    reader.IsDBNull(12)
                        ? ""
                        : reader.GetString(12),

                Unit =
                    reader.IsDBNull(13)
                        ? ""
                        : reader.GetString(13),

                UnitPrice =
                    reader.IsDBNull(14)
                        ? 0
                        : Convert.ToDecimal(
                            reader.GetValue(14)),

                Description =
                    reader.IsDBNull(15)
                        ? ""
                        : reader.GetString(15)
            };
        }


        // =========================================================
        // PARSE DATETIME
        // =========================================================

        private static DateTime ParseDateTime(
            string? value)
        {
            if (DateTime.TryParse(
                    value,
                    out DateTime result))
            {
                return result;
            }

            return DateTime.Now;
        }


        // =========================================================
        // PARSE NULLABLE DATETIME
        // =========================================================

        private static DateTime? ParseNullableDateTime(
            string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (DateTime.TryParse(
                    value,
                    out DateTime result))
            {
                return result;
            }

            return null;
        }
    }
}