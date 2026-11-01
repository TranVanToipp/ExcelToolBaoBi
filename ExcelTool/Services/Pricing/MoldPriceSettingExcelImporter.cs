using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class MoldPriceSettingExcelImporter
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

            var data =
                ReadFromExcel(filePath);

            if (data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu trong bảng 'GIÁ KHUÔN'.");
            }

            var repository =
                new MoldPriceSettingRepository();

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

                repository.Insert(setting);
            }

            return data.Count;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<MoldPriceSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<MoldPriceSetting>();

            using var workbook =
                new XLWorkbook(filePath);

            // =====================================================
            // SHEET ADMIN
            // =====================================================

            IXLWorksheet worksheet =
                workbook.Worksheet("Admin");

            // =====================================================
            // TÌM HEADER
            // =====================================================

            HeaderInfo header =
                FindHeader(worksheet);

            // =====================================================
            // VÙNG DỮ LIỆU
            // =====================================================

            IXLRange? usedRange =
                worksheet.RangeUsed();

            if (usedRange == null)
            {
                throw new InvalidOperationException(
                    "Sheet Admin không có dữ liệu.");
            }

            int lastRow =
                usedRange.LastRow().RowNumber();

            // =====================================================
            // ĐỌC CÁC DÒNG SAU HEADER
            // =====================================================

            int emptyRowCount = 0;

            for (
                int row = header.HeaderRow + 1;
                row <= lastRow;
                row++)
            {
                string moldType =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.MoldTypeColumn));

                string pricePerMoldText =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.PricePerMoldColumn));

                string pricePerSquareCmText =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.PricePerSquareCmColumn));

                // =================================================
                // KIỂM TRA DÒNG TRỐNG
                // =================================================

                bool isEmpty =
                    string.IsNullOrWhiteSpace(moldType) &&
                    string.IsNullOrWhiteSpace(pricePerMoldText) &&
                    string.IsNullOrWhiteSpace(pricePerSquareCmText);

                if (isEmpty)
                {
                    emptyRowCount++;

                    // Nếu gặp 1 dòng trống sau khi đã có dữ liệu
                    // thì xem như kết thúc bảng.
                    if (result.Count > 0)
                    {
                        break;
                    }

                    continue;
                }

                emptyRowCount = 0;

                // =================================================
                // BỎ QUA DÒNG KHÔNG CÓ KHUÔN MỚI
                // =================================================

                if (string.IsNullOrWhiteSpace(moldType))
                {
                    continue;
                }

                // =================================================
                // ĐỌC GIÁ VNĐ/KHUÔN
                // =================================================

                decimal pricePerMold =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            header.PricePerMoldColumn));

                // =================================================
                // ĐỌC GIÁ VNĐ/CM²
                // =================================================

                decimal pricePerSquareCm =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            header.PricePerSquareCmColumn));

                // =================================================
                // TẠO MODEL
                // =================================================

                result.Add(
                    new MoldPriceSetting
                    {
                        MoldType =
                            moldType,

                        PricePerMold =
                            pricePerMold,

                        PricePerSquareCm =
                            pricePerSquareCm,

                        SortOrder =
                            result.Count
                    });
            }

            return result;
        }


        // =========================================================
        // FIND HEADER
        // =========================================================

        private HeaderInfo FindHeader(
            IXLWorksheet worksheet)
        {
            IXLRange? rangeUsed =
                worksheet.RangeUsed();

            if (rangeUsed == null)
            {
                throw new InvalidOperationException(
                    "Sheet Admin không có dữ liệu.");
            }

            foreach (
                IXLRangeRow row
                in rangeUsed.RowsUsed())
            {
                var cells =
                    row.CellsUsed()
                       .OrderBy(
                           x => x.Address.ColumnNumber)
                       .ToList();

                IXLCell? moldTypeCell =
                    FindExactHeader(
                        cells,
                        IsMoldTypeHeader);

                IXLCell? pricePerMoldCell =
                    FindExactHeader(
                        cells,
                        IsPricePerMoldHeader);

                IXLCell? pricePerSquareCmCell =
                    FindExactHeader(
                        cells,
                        IsPricePerSquareCmHeader);

                // =================================================
                // PHẢI TÌM ĐỦ 3 HEADER
                // =================================================

                if (moldTypeCell == null ||
                    pricePerMoldCell == null ||
                    pricePerSquareCmCell == null)
                {
                    continue;
                }

                int moldTypeColumn =
                    moldTypeCell.Address.ColumnNumber;

                int pricePerMoldColumn =
                    pricePerMoldCell.Address.ColumnNumber;

                int pricePerSquareCmColumn =
                    pricePerSquareCmCell.Address.ColumnNumber;

                // =================================================
                // ĐÚNG THỨ TỰ NHƯ EXCEL
                //
                // Khuôn mới
                // VNĐ/khuôn
                // VNĐ/cm²
                // =================================================

                if (!(moldTypeColumn <
                      pricePerMoldColumn &&
                      pricePerMoldColumn <
                      pricePerSquareCmColumn))
                {
                    continue;
                }

                return new HeaderInfo
                {
                    HeaderRow =
                        row.RowNumber(),

                    MoldTypeColumn =
                        moldTypeColumn,

                    PricePerMoldColumn =
                        pricePerMoldColumn,

                    PricePerSquareCmColumn =
                        pricePerSquareCmColumn
                };
            }

            // =====================================================
            // KHÔNG TÌM THẤY HEADER
            // =====================================================

            throw new InvalidOperationException(
                "Không tìm thấy đầy đủ các cột trong bảng " +
                "'GIÁ KHUÔN':\n\n" +

                "Khuôn mới\n" +
                "VNĐ/khuôn\n" +
                "VNĐ/cm²");
        }


        // =========================================================
        // FIND EXACT HEADER
        // =========================================================

        private IXLCell? FindExactHeader(
            List<IXLCell> cells,
            Func<string, bool> matcher)
        {
            foreach (IXLCell cell in cells)
            {
                string header =
                    NormalizeHeader(
                        cell.GetString());

                if (matcher(header))
                {
                    return cell;
                }
            }

            return null;
        }


        // =========================================================
        // HEADER: KHUÔN MỚI
        // =========================================================

        private bool IsMoldTypeHeader(
            string header)
        {
            return header ==
                   "khuon moi";
        }


        // =========================================================
        // HEADER: VNĐ/KHUÔN
        // =========================================================

        private bool IsPricePerMoldHeader(
            string header)
        {
            return header ==
                       "vnd khuon" ||

                   header ==
                       "vnd khuon moi";
        }


        // =========================================================
        // HEADER: VNĐ/CM²
        // =========================================================

        private bool IsPricePerSquareCmHeader(
            string header)
        {
            return header ==
                       "vnd cm2" ||

                   header ==
                       "vnd cm";
        }


        // =========================================================
        // NORMALIZE HEADER
        // =========================================================

        private string NormalizeHeader(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            value =
                value
                    .Replace('Đ', 'D')
                    .Replace('đ', 'd');

            string normalized =
                value
                    .Trim()
                    .Normalize(
                        NormalizationForm.FormD);

            var builder =
                new StringBuilder();

            foreach (char character
                     in normalized)
            {
                UnicodeCategory category =
                    CharUnicodeInfo.GetUnicodeCategory(
                        character);

                if (category ==
                    UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }

                if (char.IsLetterOrDigit(
                        character))
                {
                    builder.Append(
                        char.ToLowerInvariant(
                            character));
                }
                else
                {
                    builder.Append(' ');
                }
            }

            return string.Join(
                " ",
                builder
                    .ToString()
                    .Split(
                        new[]
                        {
                            ' ',
                            '\t',
                            '\r',
                            '\n'
                        },
                        StringSplitOptions.RemoveEmptyEntries));
        }


        // =========================================================
        // GET CELL TEXT
        // =========================================================

        private string GetCellText(
            IXLCell cell)
        {
            return cell
                .GetString()
                .Trim();
        }


        // =========================================================
        // READ DECIMAL
        // =========================================================

        private decimal ReadDecimal(
            IXLCell cell)
        {
            // =====================================================
            // EXCEL LÀ NUMBER
            // =====================================================

            if (cell.DataType ==
                XLDataType.Number)
            {
                return Convert.ToDecimal(
                    cell.GetDouble());
            }

            // =====================================================
            // EXCEL LÀ TEXT
            // =====================================================

            string text =
                cell
                    .GetString()
                    .Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            // =====================================================
            // VIỆT NAM
            //
            // 2,500,000
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.GetCultureInfo("vi-VN"),
                    out decimal result))
            {
                return result;
            }

            // =====================================================
            // INVARIANT
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result))
            {
                return result;
            }

            // =====================================================
            // CURRENT CULTURE
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out result))
            {
                return result;
            }

            // =====================================================
            // KHÔNG ĐỌC ĐƯỢC
            // =====================================================

            return 0;
        }


        // =========================================================
        // HEADER INFO
        // =========================================================

        private class HeaderInfo
        {
            public int HeaderRow { get; set; }

            public int MoldTypeColumn { get; set; }

            public int PricePerMoldColumn { get; set; }

            public int PricePerSquareCmColumn { get; set; }
        }
    }
}