using ClosedXML.Excel;
using System;
using System.IO;

namespace ExcelTool.Features.Customer
{
    public class ExcelCustomerImporter
    {
        private readonly CustomerService _customerService;

        public ExcelCustomerImporter()
        {
            _customerService = new CustomerService();
        }

        // ==========================================
        // IMPORT KHÁCH HÀNG TỪ EXCEL
        // ==========================================

        public int Import()
        {
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "Quan Ly Bao Bi Offset V260407.xlsm");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file Excel.",
                    filePath);
            }

            using var workbook =
                new XLWorkbook(filePath);

            var worksheet =
                workbook.Worksheet("Customer");

            int importedCount = 0;

            // Dữ liệu bắt đầu từ dòng 6
            int row = 6;

            while (!string.IsNullOrWhiteSpace(
                worksheet.Cell(row, 4).GetString()))
            {
                var customer = new Customer
                {
                    // D = 4
                    Id = worksheet
                        .Cell(row, 4)
                        .GetString()
                        .Trim(),

                    // E = 5
                    ShortName = worksheet
                        .Cell(row, 5)
                        .GetString()
                        .Trim(),

                    // F = 6
                    FullName = worksheet
                        .Cell(row, 6)
                        .GetString()
                        .Trim(),

                    // G = 7
                    Address = worksheet
                        .Cell(row, 7)
                        .GetString()
                        .Trim(),

                    // H = 8
                    TaxCode = worksheet
                        .Cell(row, 8)
                        .GetString()
                        .Trim(),

                    // I = 9
                    ContactPerson = worksheet
                        .Cell(row, 9)
                        .GetString()
                        .Trim(),

                    // J = 10
                    Email = worksheet
                        .Cell(row, 10)
                        .GetString()
                        .Trim(),

                    // K = 11
                    Phone = worksheet
                        .Cell(row, 11)
                        .GetString()
                        .Trim()
                };

                // Không import dòng thiếu ID
                if (!string.IsNullOrWhiteSpace(customer.Id))
                {
                    // Không thêm khách hàng đã tồn tại
                    if (!_customerService.Exists(customer.Id))
                    {
                        _customerService.Add(customer);
                        importedCount++;
                    }
                }

                row++;
            }

            return importedCount;
        }
    }
}