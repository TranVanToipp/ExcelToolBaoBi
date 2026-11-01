using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class GluePriceSettingExcelImporter
    {
        // =========================================================
        // CỘT EXCEL - GIÁ KEO DÁN
        //
        // DK = Loại keo
        // DL = VNĐ/kg
        // DM = VNĐ/10cm
        // =========================================================

        private const int LoaiKeoColumn = 115;      // DK
        private const int VndPerKgColumn = 116;     // DL
        private const int VndPer10CmColumn = 117;   // DM


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

            List<GluePriceSetting> data =
                ReadFromExcel(filePath);


            if (data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu Giá keo dán trong file Excel.");
            }


            // =====================================================
            // REPOSITORY
            // =====================================================

            var repository =
                new GluePriceSettingRepository();


            // =====================================================
            // XÓA DỮ LIỆU CŨ
            // =====================================================

            repository.DeleteAll();


            // =====================================================
            // INSERT DỮ LIỆU MỚI
            // =====================================================

            int sortOrder = 0;


            foreach (GluePriceSetting setting in data)
            {
                setting.SortOrder =
                    sortOrder++;


                bool success =
                    repository.Insert(setting);


                if (!success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu Giá keo dán: {setting.LoaiKeo}");
                }
            }


            return data.Count;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<GluePriceSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<GluePriceSetting>();


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
            // TÌM SECTION "GIÁ KEO DÁN"
            // =====================================================

            IXLCell? sectionCell =
                FindSection(
                    worksheet,
                    "GIÁ KEO DÁN");


            if (sectionCell == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy khu vực 'GIÁ KEO DÁN' trong sheet 'Admin'.");
            }


            int sectionRow =
                sectionCell.Address.RowNumber;


            // =====================================================
            // HEADER
            //
            // DK = Loại keo
            // DL = VNĐ/kg
            // DM = VNĐ/10cm
            // =====================================================

            int headerRow =
                sectionRow + 1;


            string loaiKeoHeader =
                worksheet
                    .Cell(
                        headerRow,
                        LoaiKeoColumn)
                    .GetString()
                    .Trim();


            string vndPerKgHeader =
                worksheet
                    .Cell(
                        headerRow,
                        VndPerKgColumn)
                    .GetString()
                    .Trim();


            string vndPer10CmHeader =
                worksheet
                    .Cell(
                        headerRow,
                        VndPer10CmColumn)
                    .GetString()
                    .Trim();


            // =====================================================
            // KIỂM TRA HEADER
            // =====================================================

            if (!IsLoaiKeoHeader(
                    loaiKeoHeader))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'Loại keo' tại ô " +
                    $"{worksheet.Cell(headerRow, LoaiKeoColumn).Address}.");
            }


            if (!IsVndPerKgHeader(
                    vndPerKgHeader))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'VNĐ/kg' tại ô " +
                    $"{worksheet.Cell(headerRow, VndPerKgColumn).Address}.");
            }


            if (!IsVndPer10CmHeader(
                    vndPer10CmHeader))
            {
                throw new InvalidOperationException(
                    $"Không tìm thấy header 'VNĐ/10cm' tại ô " +
                    $"{worksheet.Cell(headerRow, VndPer10CmColumn).Address}.");
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

                IXLCell loaiKeoCell =
                    worksheet.Cell(
                        row,
                        LoaiKeoColumn);


                IXLCell vndPerKgCell =
                    worksheet.Cell(
                        row,
                        VndPerKgColumn);


                IXLCell vndPer10CmCell =
                    worksheet.Cell(
                        row,
                        VndPer10CmColumn);


                // =================================================
                // ĐỌC LOẠI KEO
                // =================================================

                string loaiKeo =
                    loaiKeoCell
                        .GetString()
                        .Trim();


                // =================================================
                // ĐỌC TEXT ĐỂ KIỂM TRA DÒNG TRỐNG
                // =================================================

                string vndPerKgText =
                    vndPerKgCell
                        .GetString()
                        .Trim();


                string vndPer10CmText =
                    vndPer10CmCell
                        .GetString()
                        .Trim();


                // =================================================
                // DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(loaiKeo) &&
                    string.IsNullOrWhiteSpace(vndPerKgText) &&
                    string.IsNullOrWhiteSpace(vndPer10CmText))
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
                // 17. QUY ĐỔI MÀNG
                // =================================================

                if (IsSectionTitle(loaiKeo))
                {
                    break;
                }


                // =================================================
                // NẾU KHÔNG CÓ LOẠI KEO
                // =================================================

                if (string.IsNullOrWhiteSpace(loaiKeo))
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
                // ĐỌC VNĐ/10CM
                // =================================================

                decimal vndPer10Cm =
                    ParseDecimal(
                        vndPer10CmCell);


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new GluePriceSetting
                    {
                        LoaiKeo =
                            loaiKeo,

                        VndPerKg =
                            vndPerKg,

                        VndPer10Cm =
                            vndPer10Cm,

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
            // LOẠI BỎ KÝ HIỆU
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
        // HEADER - LOẠI KEO
        // =========================================================

        private bool IsLoaiKeoHeader(
            string text)
        {
            string normalized =
                Normalize(text);


            return
                normalized == "LOẠI KEO" ||
                normalized == "LOAI KEO";
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
        // HEADER - VNĐ/10CM
        // =========================================================

        private bool IsVndPer10CmHeader(
            string text)
        {
            string normalized =
                Normalize(text);


            return
                normalized == "VNĐ/10CM" ||
                normalized == "VND/10CM" ||
                normalized == "VNĐ / 10CM" ||
                normalized == "VND / 10CM";
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