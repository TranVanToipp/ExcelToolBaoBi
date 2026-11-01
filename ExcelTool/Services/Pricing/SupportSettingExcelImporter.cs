using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class SupportSettingExcelImporter
    {
        // =========================================================
        // CỘT EXCEL - GIÁ SUPPORT
        //
        // DG = Loại
        // DH = VNĐ/kg
        // DI = VNĐ/m²
        // =========================================================

        private const int LoaiColumn = 111;       // DG
        private const int VndPerKgColumn = 112;   // DH
        private const int VndPerM2Column = 113;   // DI


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


            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Không tìm thấy file Excel.",
                    filePath);
            }


            // =====================================================
            // ĐỌC EXCEL
            // =====================================================

            List<SupportSetting> data =
                ReadFromExcel(filePath);


            if (data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu Giá Support trong file Excel.");
            }


            // =====================================================
            // REPOSITORY
            // =====================================================

            var repository =
                new SupportSettingRepository();


            // =====================================================
            // XÓA DỮ LIỆU CŨ
            // =====================================================

            repository.DeleteAll();


            // =====================================================
            // INSERT DỮ LIỆU MỚI
            // =====================================================

            int sortOrder = 0;


            foreach (SupportSetting setting in data)
            {
                setting.SortOrder =
                    sortOrder++;


                bool success =
                    repository.Insert(setting);


                if (!success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu Giá Support: {setting.Loai}");
                }
            }


            return data.Count;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<SupportSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<SupportSetting>();


            // =====================================================
            // MỞ FILE EXCEL
            // =====================================================

            using var workbook =
                new XLWorkbook(filePath);


            // =====================================================
            // SHEET ADMIN
            // =====================================================

            IXLWorksheet worksheet;

            try
            {
                worksheet =
                    workbook.Worksheet("Admin");
            }
            catch
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sheet 'Admin' trong file Excel.");
            }


            // =====================================================
            // KIỂM TRA SHEET
            // =====================================================

            var usedRange =
                worksheet.RangeUsed();


            if (usedRange == null)
            {
                return result;
            }


            // =====================================================
            // TÌM SECTION "GIÁ SUPPORT"
            // =====================================================

            IXLCell? sectionCell =
                FindSection(
                    worksheet,
                    "GIÁ SUPPORT");


            if (sectionCell == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy khu vực 'GIÁ SUPPORT' trong sheet 'Admin'.");
            }


            int sectionRow =
                sectionCell.Address.RowNumber;


            // =====================================================
            // HEADER
            //
            // Ví dụ:
            //
            // DG1 = 15. GIÁ SUPPORT
            // DG2 = Loại
            // DH2 = VNĐ/kg
            // DI2 = VNĐ/m²
            //
            // =====================================================

            int headerRow =
                sectionRow + 1;


            string loaiHeader =
                worksheet
                    .Cell(
                        headerRow,
                        LoaiColumn)
                    .GetString()
                    .Trim();


            string vndPerKgHeader =
                worksheet
                    .Cell(
                        headerRow,
                        VndPerKgColumn)
                    .GetString()
                    .Trim();


            string vndPerM2Header =
                worksheet
                    .Cell(
                        headerRow,
                        VndPerM2Column)
                    .GetString()
                    .Trim();


            // =====================================================
            // KIỂM TRA HEADER
            // =====================================================

            if (!IsLoaiHeader(loaiHeader))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'Loại' tại ô " +
                    $"{worksheet.Cell(headerRow, LoaiColumn).Address}.");
            }


            if (!IsVndPerKgHeader(
                    Normalize(vndPerKgHeader)))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'VNĐ/kg' tại ô " +
                    $"{worksheet.Cell(headerRow, VndPerKgColumn).Address}.");
            }


            if (!IsVndPerM2Header(
                    Normalize(vndPerM2Header)))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'VNĐ/m²' tại ô " +
                    $"{worksheet.Cell(headerRow, VndPerM2Column).Address}.");
            }


            // =====================================================
            // BẮT ĐẦU ĐỌC DATA
            // =====================================================

            int row =
                headerRow + 1;


            int lastRow =
                usedRange.LastRow()
                    .RowNumber();


            int emptyRowCount = 0;


            while (row <= lastRow)
            {
                // =================================================
                // CELL
                // =================================================

                IXLCell loaiCell =
                    worksheet.Cell(
                        row,
                        LoaiColumn);


                IXLCell vndPerKgCell =
                    worksheet.Cell(
                        row,
                        VndPerKgColumn);


                IXLCell vndPerM2Cell =
                    worksheet.Cell(
                        row,
                        VndPerM2Column);


                // =================================================
                // ĐỌC LOẠI
                // =================================================

                string loai =
                    loaiCell
                        .GetString()
                        .Trim();


                // =================================================
                // ĐỌC TEXT ĐỂ KIỂM TRA DÒNG TRỐNG
                // =================================================

                string vndPerKgText =
                    vndPerKgCell
                        .GetString()
                        .Trim();


                string vndPerM2Text =
                    vndPerM2Cell
                        .GetString()
                        .Trim();


                // =================================================
                // DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(loai) &&
                    string.IsNullOrWhiteSpace(vndPerKgText) &&
                    string.IsNullOrWhiteSpace(vndPerM2Text))
                {
                    emptyRowCount++;


                    if (emptyRowCount >= 2)
                    {
                        break;
                    }


                    row++;

                    continue;
                }


                emptyRowCount = 0;


                // =================================================
                // SECTION KHÁC
                //
                // Ví dụ:
                //
                // 16. GIÁ KEO DÁN
                //
                // =================================================

                if (IsSectionTitle(loai))
                {
                    break;
                }


                // =================================================
                // NẾU KHÔNG CÓ LOẠI
                // =================================================

                if (string.IsNullOrWhiteSpace(loai))
                {
                    row++;

                    continue;
                }


                // =================================================
                // ĐỌC VNĐ/KG
                // =================================================

                decimal vndPerKg =
                    ParseDecimal(
                        vndPerKgCell);


                // =================================================
                // ĐỌC VNĐ/M²
                // =================================================

                decimal vndPerM2 =
                    ParseDecimal(
                        vndPerM2Cell);


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new SupportSetting
                    {
                        Loai =
                            loai,

                        VndPerKg =
                            vndPerKg,

                        VndPerM2 =
                            vndPerM2,

                        SortOrder =
                            result.Count
                    };


                result.Add(
                    setting);


                row++;
            }


            return result;
        }


        // =========================================================
        // FIND SECTION
        // =========================================================

        private IXLCell? FindSection(
            IXLWorksheet worksheet,
            string sectionName)
        {
            var usedRange =
                worksheet.RangeUsed();


            if (usedRange == null)
            {
                return null;
            }


            string target =
                Normalize(sectionName);


            foreach (var row in usedRange.Rows())
            {
                foreach (var cell in row.Cells())
                {
                    string text =
                        Normalize(
                            cell.GetString());


                    if (text.Contains(target))
                    {
                        return cell;
                    }
                }
            }


            return null;
        }


        // =========================================================
        // PARSE DECIMAL
        // =========================================================

        private decimal ParseDecimal(
            IXLCell cell)
        {
            // =====================================================
            // CELL RỖNG
            // =====================================================

            if (cell.IsEmpty())
            {
                return 0m;
            }


            // =====================================================
            // ĐỌC TRỰC TIẾP DECIMAL
            // =====================================================

            if (cell.TryGetValue<decimal>(
                    out decimal decimalValue))
            {
                return decimalValue;
            }


            // =====================================================
            // ĐỌC DOUBLE
            // =====================================================

            if (cell.TryGetValue<double>(
                    out double doubleValue))
            {
                return Convert.ToDecimal(
                    doubleValue);
            }


            // =====================================================
            // ĐỌC STRING
            // =====================================================

            string text =
                cell.GetString()
                    .Trim();


            if (string.IsNullOrWhiteSpace(text))
            {
                return 0m;
            }


            // =====================================================
            // LOẠI BỎ KÝ HIỆU TIỀN
            // =====================================================

            text =
                text
                    .Replace("VNĐ", "")
                    .Replace("VND", "")
                    .Replace("₫", "")
                    .Replace("đ", "")
                    .Replace("Đ", "")
                    .Replace(" ", "")
                    .Trim();


            // =====================================================
            // FORMAT VIỆT NAM
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    new CultureInfo("vi-VN"),
                    out decimal value))
            {
                return value;
            }


            // =====================================================
            // FORMAT INVARIANT
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value))
            {
                return value;
            }


            // =====================================================
            // KHÔNG ĐỌC ĐƯỢC
            // =====================================================

            throw new FormatException(
                $"Không thể đọc giá trị '{text}' " +
                $"tại ô {cell.Address}.");
        }


        // =========================================================
        // HEADER - LOẠI
        // =========================================================

        private bool IsLoaiHeader(
            string text)
        {
            return
                Normalize(text) == "LOẠI";
        }


        // =========================================================
        // HEADER - VNĐ/KG
        // =========================================================

        private bool IsVndPerKgHeader(
            string text)
        {
            string normalized =
                Normalize(text);


            return
                normalized == "VNĐ/KG" ||
                normalized == "VND/KG" ||
                normalized == "VNĐ / KG" ||
                normalized == "VND / KG";
        }


        // =========================================================
        // HEADER - VNĐ/M²
        // =========================================================

        private bool IsVndPerM2Header(
            string text)
        {
            string normalized =
                Normalize(text);


            return
                normalized == "VNĐ/M²" ||
                normalized == "VND/M²" ||
                normalized == "VNĐ/M2" ||
                normalized == "VND/M2" ||
                normalized == "VNĐ / M²" ||
                normalized == "VND / M²";
        }


        // =========================================================
        // NORMALIZE
        // =========================================================

        private string Normalize(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }


            string normalized =
                text
                    .Trim()
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ");


            while (
                normalized.Contains("  "))
            {
                normalized =
                    normalized.Replace(
                        "  ",
                        " ");
            }


            return normalized
                .ToUpperInvariant();
        }


        // =========================================================
        // SECTION TITLE
        // =========================================================

        private bool IsSectionTitle(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }


            int dotIndex =
                text.IndexOf('.');


            if (dotIndex <= 0)
            {
                return false;
            }


            string numberPart =
                text
                    .Substring(
                        0,
                        dotIndex)
                    .Trim();


            return int.TryParse(
                numberPart,
                out _);
        }
    }
}