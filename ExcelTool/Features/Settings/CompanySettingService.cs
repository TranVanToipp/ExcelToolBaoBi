using ExcelTool.Database;
using Microsoft.Data.Sqlite;

namespace ExcelTool.Features.Settings
{
    public class CompanySettingService
    {
        // ==========================================
        // LẤY THÔNG TIN CÔNG TY
        // ==========================================

        public CompanySetting Get()
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                SELECT
                    Id,
                    CompanyName,
                    Address,
                    TaxCode,
                    Phone,
                    Email,
                    Website,
                    LogoPath
                FROM CompanySetting
                WHERE Id = 1;
                """;

            using var reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return new CompanySetting
                {
                    Id = 1
                };
            }

            return new CompanySetting
            {
                Id =
                    reader.GetInt32(
                        reader.GetOrdinal("Id")),

                CompanyName =
                    reader["CompanyName"]?.ToString()
                    ?? "",

                Address =
                    reader["Address"]?.ToString()
                    ?? "",

                TaxCode =
                    reader["TaxCode"]?.ToString()
                    ?? "",

                Phone =
                    reader["Phone"]?.ToString()
                    ?? "",

                Email =
                    reader["Email"]?.ToString()
                    ?? "",

                Website =
                    reader["Website"]?.ToString()
                    ?? "",

                LogoPath =
                    reader["LogoPath"]?.ToString()
                    ?? ""
            };
        }


        // ==========================================
        // LƯU THÔNG TIN CÔNG TY
        // ==========================================

        public void Save(
            CompanySetting setting)
        {
            using var connection =
                DatabaseService.GetConnection();

            connection.Open();

            using var command =
                connection.CreateCommand();

            command.CommandText = """
                INSERT INTO CompanySetting
                (
                    Id,
                    CompanyName,
                    Address,
                    TaxCode,
                    Phone,
                    Email,
                    Website,
                    LogoPath
                )
                VALUES
                (
                    1,
                    $companyName,
                    $address,
                    $taxCode,
                    $phone,
                    $email,
                    $website,
                    $logoPath
                )
                ON CONFLICT(Id)
                DO UPDATE SET
                    CompanyName = excluded.CompanyName,
                    Address = excluded.Address,
                    TaxCode = excluded.TaxCode,
                    Phone = excluded.Phone,
                    Email = excluded.Email,
                    Website = excluded.Website,
                    LogoPath = excluded.LogoPath;
                """;

            command.Parameters.AddWithValue(
                "$companyName",
                setting.CompanyName ?? "");

            command.Parameters.AddWithValue(
                "$address",
                setting.Address ?? "");

            command.Parameters.AddWithValue(
                "$taxCode",
                setting.TaxCode ?? "");

            command.Parameters.AddWithValue(
                "$phone",
                setting.Phone ?? "");

            command.Parameters.AddWithValue(
                "$email",
                setting.Email ?? "");

            command.Parameters.AddWithValue(
                "$website",
                setting.Website ?? "");

            command.Parameters.AddWithValue(
                "$logoPath",
                setting.LogoPath ?? "");

            command.ExecuteNonQuery();
        }
    }
}