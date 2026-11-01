using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace ExcelTool.Services.Pricing
{
    public class WasteSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly WasteSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public WasteSettingExcelImporter()
        {
            _repository =
                new WasteSettingRepository();
        }


        // =========================================================
        // IMPORT
        // =========================================================

        public int Import(
            string excelFilePath)
        {
            if (string.IsNullOrWhiteSpace(
                    excelFilePath))
            {
                throw new ArgumentException(
                    "Đường dẫn file Excel không được để trống.",
                    nameof(excelFilePath));
            }


            if (!File.Exists(
                    excelFilePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file Excel.",
                    excelFilePath);
            }


            // -----------------------------------------------------
            // ĐỌC EXCEL
            // -----------------------------------------------------

            List<WasteSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // XÓA DỮ LIỆU CŨ
            //
            // Import lại Excel = thay thế dữ liệu cũ
            // -----------------------------------------------------

            _repository.DeleteAll();


            // -----------------------------------------------------
            // INSERT DỮ LIỆU MỚI
            // -----------------------------------------------------

            int importedCount = 0;


            foreach (
                WasteSetting setting
                in settings)
            {
                if (_repository.Insert(
                        setting))
                {
                    importedCount++;
                }
            }


            return importedCount;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<WasteSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<WasteSetting>();


            using var workbook =
                new XLWorkbook(
                    excelFilePath);


            // -----------------------------------------------------
            // SHEET ADMIN
            // -----------------------------------------------------

            if (!workbook.Worksheets.TryGetWorksheet(
                    "Admin",
                    out IXLWorksheet? worksheet))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sheet 'Admin' trong file Excel.");
            }


            // -----------------------------------------------------
            // DỮ LIỆU BẮT ĐẦU TỪ DÒNG 3
            //
            // J  = Số tờ
            // K  = 01 màu
            // L  = 02 màu
            // M  = 03 màu
            // N  = 04 màu
            // O  = 05 màu
            // P  = 06 màu
            // Q  = 07 màu
            // R  = Cán màng
            // S  = Tráng phủ
            // T  = Bế
            // U  = Cán vân
            // V  = Ép nhũ
            // W  = Bồi
            // X  = Đóng kim
            // Y  = Cắt
            // Z  = Dán
            //
            // Đọc cho đến khi cột J không còn dữ liệu.
            // -----------------------------------------------------

            const int startRow = 3;

            int sortOrder = 1;


            for (
                int row = startRow;
                row <= worksheet.LastRowUsed().RowNumber();
                row++)
            {
                // -------------------------------------------------
                // SỐ TỜ
                // -------------------------------------------------

                string soTo =
                    worksheet
                        .Cell(row, "J")
                        .GetString()
                        .Trim();


                // -------------------------------------------------
                // BỎ QUA DÒNG KHÔNG CÓ SỐ TỜ
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                        soTo))
                {
                    continue;
                }


                // -------------------------------------------------
                // TẠO MODEL
                // -------------------------------------------------

                var setting =
                    new WasteSetting
                    {
                        SoTo = soTo,

                        Mau01 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "K")),

                        Mau02 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "L")),

                        Mau03 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "M")),

                        Mau04 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "N")),

                        Mau05 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "O")),

                        Mau06 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "P")),

                        Mau07 =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "Q")),

                        CanMang =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "R")),

                        TrangPhu =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "S")),

                        Be =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "T")),

                        CanVan =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "U")),

                        EpNhu =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "V")),

                        Boi =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "W")),

                        DongKim =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "X")),

                        Cat =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "Y")),

                        Dan =
                            ReadDecimal(
                                worksheet.Cell(
                                    row,
                                    "Z")),

                        SortOrder =
                            sortOrder
                    };


                result.Add(
                    setting);


                sortOrder++;
            }


            return result;
        }


        // =========================================================
        // READ DECIMAL
        // =========================================================

        private static decimal ReadDecimal(IXLCell cell)
        {
            // =========================================================
            // Ô trống
            // =========================================================

            if (cell.IsEmpty())
            {
                return 0;
            }


            // =========================================================
            // LẤY GIÁ TRỊ HIỂN THỊ TRONG EXCEL
            // =========================================================

            string text =
                cell.GetFormattedString()
                    .Trim();


            // =========================================================
            // DẤU "-"
            // =========================================================

            if (string.IsNullOrWhiteSpace(text) ||
                text == "-")
            {
                return 0;
            }


            // =========================================================
            // LOẠI BỎ DẤU PHÂN CÁCH HÀNG NGHÌN
            //
            // Ví dụ:
            // 2,595 → 2595
            // 1,050 → 1050
            // =========================================================

            text =
                text.Replace(",", "")
                    .Replace(" ", "");


            // =========================================================
            // ĐỌC DECIMAL
            // =========================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                return result;
            }


            // =========================================================
            // KHÔNG ĐỌC ĐƯỢC
            // =========================================================

            return 0;
        }
    }
}