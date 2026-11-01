using ExcelTool.Database;
using System.Collections.Generic;

namespace ExcelTool.Features.Customer
{
    public class CustomerService
    {
        // ==========================================
        // LẤY TẤT CẢ KHÁCH HÀNG
        // ==========================================

        public List<Customer> GetAll()
        {
            var customers = new List<Customer>();

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    ShortName,
                    FullName,
                    Address,
                    TaxCode,
                    ContactPerson,
                    Email,
                    Phone
                FROM Customer
                ORDER BY CAST(Id AS INTEGER);
                """;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = reader["Id"]?.ToString() ?? "",
                    ShortName = reader["ShortName"]?.ToString() ?? "",
                    FullName = reader["FullName"]?.ToString() ?? "",
                    Address = reader["Address"]?.ToString() ?? "",
                    TaxCode = reader["TaxCode"]?.ToString() ?? "",
                    ContactPerson = reader["ContactPerson"]?.ToString() ?? "",
                    Email = reader["Email"]?.ToString() ?? "",
                    Phone = reader["Phone"]?.ToString() ?? ""
                });
            }

            return customers;
        }


        // ==========================================
        // THÊM KHÁCH HÀNG
        // ==========================================

        public void Add(Customer customer)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                INSERT INTO Customer
                (
                    Id,
                    ShortName,
                    FullName,
                    Address,
                    TaxCode,
                    ContactPerson,
                    Email,
                    Phone
                )
                VALUES
                (
                    $id,
                    $shortName,
                    $fullName,
                    $address,
                    $taxCode,
                    $contactPerson,
                    $email,
                    $phone
                );
                """;

            command.Parameters.AddWithValue("$id", customer.Id);
            command.Parameters.AddWithValue("$shortName", customer.ShortName);
            command.Parameters.AddWithValue("$fullName", customer.FullName);
            command.Parameters.AddWithValue("$address", customer.Address);
            command.Parameters.AddWithValue("$taxCode", customer.TaxCode);
            command.Parameters.AddWithValue("$contactPerson", customer.ContactPerson);
            command.Parameters.AddWithValue("$email", customer.Email);
            command.Parameters.AddWithValue("$phone", customer.Phone);

            command.ExecuteNonQuery();
        }


        // ==========================================
        // CẬP NHẬT KHÁCH HÀNG
        // ==========================================

        public void Update(Customer customer)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                UPDATE Customer
                SET
                    ShortName = $shortName,
                    FullName = $fullName,
                    Address = $address,
                    TaxCode = $taxCode,
                    ContactPerson = $contactPerson,
                    Email = $email,
                    Phone = $phone
                WHERE Id = $id;
                """;

            command.Parameters.AddWithValue("$id", customer.Id);
            command.Parameters.AddWithValue("$shortName", customer.ShortName);
            command.Parameters.AddWithValue("$fullName", customer.FullName);
            command.Parameters.AddWithValue("$address", customer.Address);
            command.Parameters.AddWithValue("$taxCode", customer.TaxCode);
            command.Parameters.AddWithValue("$contactPerson", customer.ContactPerson);
            command.Parameters.AddWithValue("$email", customer.Email);
            command.Parameters.AddWithValue("$phone", customer.Phone);

            command.ExecuteNonQuery();
        }


        // ==========================================
        // XÓA KHÁCH HÀNG
        // ==========================================

        public void Delete(string id)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                DELETE FROM Customer
                WHERE Id = $id;
                """;

            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }


        // ==========================================
        // KIỂM TRA KHÁCH HÀNG
        // ==========================================

        public bool Exists(string id)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT COUNT(*)
                FROM Customer
                WHERE Id = $id;
                """;

            command.Parameters.AddWithValue("$id", id);

            long count =
                (long)command.ExecuteScalar()!;

            return count > 0;
        }


        // ==========================================
        // TÌM KHÁCH HÀNG
        // ==========================================

        public List<Customer> Search(string keyword)
        {
            var customers = new List<Customer>();

            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    ShortName,
                    FullName,
                    Address,
                    TaxCode,
                    ContactPerson,
                    Email,
                    Phone
                FROM Customer
                WHERE
                    Id LIKE $keyword
                    OR ShortName LIKE $keyword
                    OR FullName LIKE $keyword
                    OR Address LIKE $keyword
                    OR TaxCode LIKE $keyword
                    OR ContactPerson LIKE $keyword
                    OR Email LIKE $keyword
                    OR Phone LIKE $keyword
                ORDER BY CAST(Id AS INTEGER);
                """;

            command.Parameters.AddWithValue(
                "$keyword",
                "%" + keyword + "%");

            using var reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    Id = reader["Id"]?.ToString() ?? "",
                    ShortName = reader["ShortName"]?.ToString() ?? "",
                    FullName = reader["FullName"]?.ToString() ?? "",
                    Address = reader["Address"]?.ToString() ?? "",
                    TaxCode = reader["TaxCode"]?.ToString() ?? "",
                    ContactPerson = reader["ContactPerson"]?.ToString() ?? "",
                    Email = reader["Email"]?.ToString() ?? "",
                    Phone = reader["Phone"]?.ToString() ?? ""
                });
            }

            return customers;
        }

        // ==========================================
        // LẤY ID TIẾP THEO
        // ==========================================

        public string GetNextId()
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
        SELECT COALESCE(
            MAX(CAST(Id AS INTEGER)),
            0
        ) + 1
        FROM Customer;
        """;

            long nextId =
                (long)command.ExecuteScalar()!;

            return nextId.ToString();
        }
    }
}