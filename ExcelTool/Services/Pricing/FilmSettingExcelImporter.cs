using System;
using System.Globalization;

using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class FilmSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly FilmSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FilmSettingExcelImporter()
        {
            _repository =
                new FilmSettingRepository();
        }


        // =========================================================
        // IMPORT
        // =========================================================

        public int Import(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    "Đường dẫn file Excel không hợp lệ.",
                    nameof(filePath));
            }


            using var workbook =
                new XLWorkbook(filePath);


            var worksheet =
                FindWorksheet(workbook);


            if (worksheet == null)
            {
                throw new Exception(
                    "Không tìm thấy sheet chứa dữ liệu Quy đổi trọng lượng màng.");
            }


            int headerRow =
                FindHeaderRow(worksheet);


            if (headerRow == 0)
            {
                throw new Exception(
                    "Không tìm thấy dòng tiêu đề của bảng Quy đổi trọng lượng màng.");
            }


            int loaiMangColumn =
                FindColumn(
                    worksheet,
                    headerRow,
                    "Loại màng");

            int dayColumn =
                FindColumn(
                    worksheet,
                    headerRow,
                    "Dày (micron)");

            int daiColumn =
                FindColumn(
                    worksheet,
                    headerRow,
                    "Dài (m)");

            int trongLuongColumn =
                FindColumn(
                    worksheet,
                    headerRow,
                    "Trọng lượng (kg/m)");

            int m2PerKgColumn =
                FindColumn(
                    worksheet,
                    headerRow,
                    "m²/kg");


            if (loaiMangColumn == 0 ||
                dayColumn == 0 ||
                daiColumn == 0 ||
                trongLuongColumn == 0 ||
                m2PerKgColumn == 0)
            {
                throw new Exception(
                    "Không tìm đủ các cột:\n" +
                    "Loại màng\n" +
                    "Dày (micron)\n" +
                    "Dài (m)\n" +
                    "Trọng lượng (kg/m)\n" +
                    "m²/kg");
            }


            // =====================================================
            // XÓA DATA CŨ
            // =====================================================

            _repository.DeleteAll();


            int importedCount = 0;

            int sortOrder = 0;

            int lastRow =
                worksheet.LastRowUsed()?.RowNumber() ?? 0;


            // =====================================================
            // ĐỌC DATA
            // =====================================================

            for (
                int row = headerRow + 1;
                row <= lastRow;
                row++)
            {
                string loaiMang =
                    GetString(
                        worksheet.Cell(
                            row,
                            loaiMangColumn));


                decimal dayMicron =
                    GetDecimal(
                        worksheet.Cell(
                            row,
                            dayColumn));


                decimal daiM =
                    GetDecimal(
                        worksheet.Cell(
                            row,
                            daiColumn));


                decimal trongLuongKgPerM =
                    GetDecimal(
                        worksheet.Cell(
                            row,
                            trongLuongColumn));


                decimal m2PerKg =
                    GetDecimal(
                        worksheet.Cell(
                            row,
                            m2PerKgColumn));


                // =================================================
                // BỎ QUA DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(loaiMang) &&
                    dayMicron == 0 &&
                    daiM == 0 &&
                    trongLuongKgPerM == 0 &&
                    m2PerKg == 0)
                {
                    continue;
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new FilmSetting
                    {
                        LoaiMang =
                            loaiMang,

                        DayMicron =
                            dayMicron,

                        DaiM =
                            daiM,

                        TrongLuongKgPerM =
                            trongLuongKgPerM,

                        M2PerKg =
                            m2PerKg,

                        SortOrder =
                            sortOrder++
                    };


                // =================================================
                // INSERT
                // =================================================

                if (_repository.Insert(setting))
                {
                    importedCount++;
                }
            }


            return importedCount;
        }


        // =========================================================
        // FIND WORKSHEET
        // =========================================================

        private static IXLWorksheet? FindWorksheet(
            XLWorkbook workbook)
        {
            foreach (var worksheet in workbook.Worksheets)
            {
                int headerRow =
                    FindHeaderRow(worksheet);


                if (headerRow == 0)
                {
                    continue;
                }


                int loaiMangColumn =
                    FindColumn(
                        worksheet,
                        headerRow,
                        "Loại màng");


                int dayColumn =
                    FindColumn(
                        worksheet,
                        headerRow,
                        "Dày (micron)");


                int daiColumn =
                    FindColumn(
                        worksheet,
                        headerRow,
                        "Dài (m)");


                if (loaiMangColumn != 0 &&
                    dayColumn != 0 &&
                    daiColumn != 0)
                {
                    return worksheet;
                }
            }


            return null;
        }


        // =========================================================
        // FIND HEADER ROW
        // =========================================================

        private static int FindHeaderRow(
            IXLWorksheet worksheet)
        {
            var usedRange =
                worksheet.RangeUsed();


            if (usedRange == null)
            {
                return 0;
            }


            int firstRow =
                usedRange.RangeAddress.FirstAddress.RowNumber;

            int lastRow =
                usedRange.RangeAddress.LastAddress.RowNumber;


            for (
                int row = firstRow;
                row <= lastRow;
                row++)
            {
                int loaiMangColumn =
                    FindColumn(
                        worksheet,
                        row,
                        "Loại màng");


                int dayColumn =
                    FindColumn(
                        worksheet,
                        row,
                        "Dày (micron)");


                int daiColumn =
                    FindColumn(
                        worksheet,
                        row,
                        "Dài (m)");


                if (loaiMangColumn != 0 &&
                    dayColumn != 0 &&
                    daiColumn != 0)
                {
                    return row;
                }
            }


            return 0;
        }


        // =========================================================
        // FIND COLUMN
        // =========================================================

        private static int FindColumn(
            IXLWorksheet worksheet,
            int headerRow,
            string header)
        {
            var row =
                worksheet.Row(headerRow);


            foreach (var cell in row.CellsUsed())
            {
                string value =
                    cell.GetString().Trim();


                if (Normalize(value) ==
                    Normalize(header))
                {
                    return cell.Address.ColumnNumber;
                }
            }


            return 0;
        }


        // =========================================================
        // GET STRING
        // =========================================================

        private static string GetString(
            IXLCell cell)
        {
            try
            {
                return cell.GetString().Trim();
            }
            catch
            {
                return cell.Value.ToString().Trim();
            }
        }


        // =========================================================
        // GET DECIMAL
        // =========================================================

        private static decimal GetDecimal(
            IXLCell cell)
        {
            if (cell == null)
            {
                return 0;
            }


            try
            {
                // Excel lưu dạng số
                if (cell.DataType ==
                    XLDataType.Number)
                {
                    double number =
                        cell.GetDouble();

                    return Convert.ToDecimal(
                        number);
                }
            }
            catch
            {
                // Tiếp tục thử parse dạng text.
            }


            string text;

            try
            {
                text =
                    cell.Value
                        .ToString()
                        .Trim();
            }
            catch
            {
                text =
                    cell.GetString()
                        .Trim();
            }


            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }


            // =====================================================
            // XỬ LÝ SỐ DẠNG VIỆT NAM
            // Ví dụ:
            // 54,6
            // 109,9
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.GetCultureInfo("vi-VN"),
                    out decimal vietnameseValue))
            {
                return vietnameseValue;
            }


            // =====================================================
            // XỬ LÝ SỐ DẠNG QUỐC TẾ
            // Ví dụ:
            // 54.6
            // 109.9
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal invariantValue))
            {
                return invariantValue;
            }


            // =====================================================
            // KHÔNG PARSE ĐƯỢC
            // =====================================================

            return 0;
        }


        // =========================================================
        // NORMALIZE HEADER
        // =========================================================

        private static string Normalize(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }


            return value
                .Trim()
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("²", "2")
                .ToLowerInvariant();
        }
    }
}