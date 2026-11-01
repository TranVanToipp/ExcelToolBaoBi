using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class UnitSettingExcelImporter
    {
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

            var data =
                ReadFromExcel(filePath);


            if (data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu 'Đơn vị tính' trong file Excel.");
            }


            // =====================================================
            // REPOSITORY
            // =====================================================

            var repository =
                new UnitSettingRepository();


            // =====================================================
            // XÓA DỮ LIỆU CŨ
            // =====================================================

            repository.DeleteAll();


            // =====================================================
            // INSERT DỮ LIỆU MỚI
            // =====================================================

            int sortOrder = 0;


            foreach (var setting in data)
            {
                setting.SortOrder =
                    sortOrder++;


                repository.Insert(
                    setting);
            }


            return data.Count;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<UnitSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<UnitSetting>();


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
            // TÌM KHU VỰC
            //
            // Không fix cứng dòng/cột.
            //
            // Tìm:
            //
            // 13. ĐƠN VỊ TÍNH
            //
            // =====================================================

            var sectionCell =
                FindSection(
                    worksheet,
                    "13.",
                    "ĐƠN VỊ TÍNH");


            if (sectionCell == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy khu vực '13. ĐƠN VỊ TÍNH' trong sheet 'Admin'.");
            }


            int sectionRow =
                sectionCell.Address.RowNumber;


            // =====================================================
            // TÌM HEADER "Đơn giá"
            //
            // Tìm trong các dòng bên dưới tiêu đề.
            //
            // Không fix cứng cột.
            // =====================================================

            var headerCell =
                FindHeader(
                    worksheet,
                    sectionRow,
                    "Đơn giá");


            if (headerCell == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy cột 'Đơn giá' trong khu vực '13. ĐƠN VỊ TÍNH'.");
            }


            int headerRow =
                headerCell.Address.RowNumber;

            int dataColumn =
                headerCell.Address.ColumnNumber;


            // =====================================================
            // ĐỌC DỮ LIỆU
            // =====================================================

            int row =
                headerRow + 1;


            int emptyRowCount =
                0;


            while (row <= worksheet.LastRowUsed().RowNumber())
            {
                string unit =
                    worksheet
                        .Cell(
                            row,
                            dataColumn)
                        .GetString()
                        .Trim();


                // =================================================
                // BỎ QUA DÒNG TRỐNG
                //
                // Cho phép một vài dòng trống.
                // Dừng khi gặp nhiều dòng trống liên tiếp.
                // =================================================

                if (string.IsNullOrWhiteSpace(unit))
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
                // BỎ QUA CÁC DÒNG KHÔNG PHẢI DỮ LIỆU
                //
                // Nếu gặp lại tiêu đề/menu khác thì dừng.
                // =================================================

                if (IsSectionTitle(unit))
                {
                    break;
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                result.Add(
                    new UnitSetting
                    {
                        Unit =
                            unit,

                        SortOrder =
                            result.Count
                    });


                row++;
            }


            return result;
        }


        // =========================================================
        // FIND SECTION
        // =========================================================

        private IXLCell? FindSection(
            IXLWorksheet worksheet,
            string sectionNumber,
            string sectionName)
        {
            var usedRange =
                worksheet.RangeUsed();


            if (usedRange == null)
            {
                return null;
            }


            foreach (var row in usedRange.Rows())
            {
                foreach (var cell in row.Cells())
                {
                    string text =
                        Normalize(
                            cell.GetString());


                    if (!text.Contains(
                            Normalize(sectionNumber)) ||
                        !text.Contains(
                            Normalize(sectionName)))
                    {
                        continue;
                    }


                    return cell;
                }
            }


            return null;
        }


        // =========================================================
        // FIND HEADER
        // =========================================================

        private IXLCell? FindHeader(
            IXLWorksheet worksheet,
            int sectionRow,
            string headerName)
        {
            var usedRange =
                worksheet.RangeUsed();


            if (usedRange == null)
            {
                return null;
            }


            int startRow =
                sectionRow + 1;


            int lastRow =
                usedRange.LastRow().RowNumber();


            // =====================================================
            // Chỉ tìm header bên dưới section.
            //
            // Không giới hạn cột.
            // Không giả định header nằm ở cột nào.
            // =====================================================

            for (
                int row = startRow;
                row <= lastRow;
                row++)
            {
                foreach (
                    var cell in worksheet.Row(row).Cells())
                {
                    string text =
                        Normalize(
                            cell.GetString());


                    if (text.Equals(
                            Normalize(headerName)))
                    {
                        return cell;
                    }
                }


                // =================================================
                // Nếu đi vào section khác thì dừng.
                // =================================================

                string firstCellText =
                    Normalize(
                        worksheet
                            .Cell(
                                row,
                                1)
                            .GetString());


                if (IsSectionTitle(
                        firstCellText))
                {
                    break;
                }
            }


            return null;
        }


        // =========================================================
        // NORMALIZE TEXT
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
        // KIỂM TRA SECTION
        // =========================================================

        private bool IsSectionTitle(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }


            string value =
                text.Trim();


            // =====================================================
            // Ví dụ:
            //
            // 0. THÔNG BÁO
            // 1. THÔNG TIN CÔNG TY
            // 12. GIÁ KHUÔN
            // 13. ĐƠN VỊ TÍNH
            // =====================================================

            int dotIndex =
                value.IndexOf('.');


            if (dotIndex <= 0)
            {
                return false;
            }


            string numberPart =
                value.Substring(
                    0,
                    dotIndex)
                .Trim();


            return int.TryParse(
                numberPart,
                out _);
        }
    }
}