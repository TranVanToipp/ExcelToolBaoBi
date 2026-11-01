using ExcelTool.Database;
using System;
using System.Collections.Generic;

namespace ExcelTool.Features.Quotation
{
    public class ProductService
    {
        // ==========================================
        // LẤY TẤT CẢ SẢN PHẨM
        // ==========================================

        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    ProductCode,
                    ProductName,
                    Unit,
                    UnitPrice,
                    Description
                FROM Product
                ORDER BY ProductName;
                """;

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id =
                        reader["Id"]?.ToString() ?? "",

                    ProductCode =
                        reader["ProductCode"]?.ToString() ?? "",

                    ProductName =
                        reader["ProductName"]?.ToString() ?? "",

                    Unit =
                        reader["Unit"]?.ToString() ?? "",

                    UnitPrice =
                        Convert.ToDecimal(
                            reader["UnitPrice"]),

                    Description =
                        reader["Description"]?.ToString() ?? ""
                });
            }

            return products;
        }


        // ==========================================
        // LẤY SẢN PHẨM THEO ID
        // ==========================================

        public Product? GetById(string id)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    ProductCode,
                    ProductName,
                    Unit,
                    UnitPrice,
                    Description
                FROM Product
                WHERE Id = $id;
                """;

            command.Parameters.AddWithValue(
                "$id",
                id);

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Product
            {
                Id =
                    reader["Id"]?.ToString() ?? "",

                ProductCode =
                    reader["ProductCode"]?.ToString() ?? "",

                ProductName =
                    reader["ProductName"]?.ToString() ?? "",

                Unit =
                    reader["Unit"]?.ToString() ?? "",

                UnitPrice =
                    Convert.ToDecimal(
                        reader["UnitPrice"]),

                Description =
                    reader["Description"]?.ToString() ?? ""
            };
        }
    }
}