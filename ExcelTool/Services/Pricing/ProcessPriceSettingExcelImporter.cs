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
    public class ProcessPriceSettingExcelImporter
    {
        // =========================================================
        // IMPORT
        // =========================================================

        public int Import(string filePath)
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

            var data = ReadFromExcel(filePath);

            if (data.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu của bảng " +
                    "'DANH MỤC CÔNG ĐOẠN SẢN XUẤT VÀ ĐƠN GIÁ'.");
            }

            var repository =
                new ProcessPriceSettingRepository();

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
                setting.SortOrder = sortOrder++;

                repository.Insert(setting);
            }

            return data.Count;
        }


        // =========================================================
        // READ EXCEL
        // =========================================================

        private List<ProcessPriceSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<ProcessPriceSetting>();

            using var workbook =
                new XLWorkbook(filePath);

            IXLWorksheet worksheet =
                workbook.Worksheet("Admin");


            // =====================================================
            // TÌM HEADER CỦA ĐÚNG BẢNG
            // =====================================================

            HeaderInfo header =
                FindHeader(worksheet);


            // =====================================================
            // XÁC ĐỊNH DÒNG CUỐI CỦA SHEET
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
            // ĐỌC TỪ DÒNG SAU HEADER
            // =====================================================

            for (
                int row = header.HeaderRow + 1;
                row <= lastRow;
                row++)
            {
                string process =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.ProcessColumn));


                string stage =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.StageColumn));


                string unitPriceText =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.UnitPriceColumn));


                string unit =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.UnitColumn));


                string minimumBatchPriceText =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.MinimumBatchPriceColumn));


                string minimumQuantityText =
                    GetCellText(
                        worksheet.Cell(
                            row,
                            header.MinimumQuantityColumn));


                // =================================================
                // KIỂM TRA DÒNG CÓ DỮ LIỆU
                // =================================================

                bool hasData =
                    !string.IsNullOrWhiteSpace(process) ||
                    !string.IsNullOrWhiteSpace(stage) ||
                    !string.IsNullOrWhiteSpace(unitPriceText) ||
                    !string.IsNullOrWhiteSpace(unit) ||
                    !string.IsNullOrWhiteSpace(minimumBatchPriceText) ||
                    !string.IsNullOrWhiteSpace(minimumQuantityText);


                if (!hasData)
                {
                    continue;
                }


                // =================================================
                // BỎ QUA DÒNG KHÔNG PHẢI DỮ LIỆU
                // =================================================

                if (string.IsNullOrWhiteSpace(process) &&
                    string.IsNullOrWhiteSpace(stage))
                {
                    continue;
                }


                // =================================================
                // ĐỌC GIÁ TRỊ SỐ
                // =================================================

                decimal unitPrice =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            header.UnitPriceColumn));


                decimal minimumBatchPrice =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            header.MinimumBatchPriceColumn));


                decimal minimumQuantity =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            header.MinimumQuantityColumn));


                // =================================================
                // TẠO MODEL
                // =================================================

                result.Add(
                    new ProcessPriceSetting
                    {
                        Process =
                            process,

                        Stage =
                            stage,

                        UnitPrice =
                            unitPrice,

                        Unit =
                            unit,

                        MinimumBatchPrice =
                            minimumBatchPrice,

                        MinimumQuantity =
                            minimumQuantity,

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


            // =====================================================
            // TÌM TỪNG DÒNG TRONG SHEET
            // =====================================================

            foreach (
                IXLRangeRow row
                in rangeUsed.RowsUsed())
            {
                var cells =
                    row.CellsUsed()
                       .OrderBy(
                           x => x.Address.ColumnNumber)
                       .ToList();


                if (cells.Count == 0)
                {
                    continue;
                }


                // =================================================
                // TÌM HEADER THEO TÊN CHÍNH XÁC
                // =================================================

                IXLCell? processCell =
                    FindExactHeader(
                        cells,
                        IsProcessHeader);


                IXLCell? stageCell =
                    FindExactHeader(
                        cells,
                        IsStageHeader);


                IXLCell? unitPriceCell =
                    FindExactHeader(
                        cells,
                        IsUnitPriceHeader);


                IXLCell? unitCell =
                    FindExactHeader(
                        cells,
                        IsUnitHeader);


                IXLCell? minimumBatchPriceCell =
                    FindExactHeader(
                        cells,
                        IsMinimumBatchPriceHeader);


                IXLCell? minimumQuantityCell =
                    FindExactHeader(
                        cells,
                        IsMinimumQuantityHeader);


                // =================================================
                // PHẢI TÌM ĐỦ 6 HEADER
                // =================================================

                if (processCell == null ||
                    stageCell == null ||
                    unitPriceCell == null ||
                    unitCell == null ||
                    minimumBatchPriceCell == null ||
                    minimumQuantityCell == null)
                {
                    continue;
                }


                // =================================================
                // KIỂM TRA CÁC HEADER CÓ THUỘC CÙNG BẢNG
                //
                // Header của bảng này phải nằm liên tiếp.
                // Không quan trọng bắt đầu từ cột nào.
                // =================================================

                int[] columns =
                {
                    processCell.Address.ColumnNumber,
                    stageCell.Address.ColumnNumber,
                    unitPriceCell.Address.ColumnNumber,
                    unitCell.Address.ColumnNumber,
                    minimumBatchPriceCell.Address.ColumnNumber,
                    minimumQuantityCell.Address.ColumnNumber
                };


                bool correctOrder =
                    columns[0] < columns[1] &&
                    columns[1] < columns[2] &&
                    columns[2] < columns[3] &&
                    columns[3] < columns[4] &&
                    columns[4] < columns[5];


                if (!correctOrder)
                {
                    continue;
                }


                // =================================================
                // KIỂM TRA KHOẢNG CÁCH
                //
                // Không bắt buộc tuyệt đối phải liền nhau,
                // nhưng không được cách quá xa.
                // =================================================

                int minColumn =
                    columns.Min();

                int maxColumn =
                    columns.Max();

                int columnDistance =
                    maxColumn - minColumn;


                if (columnDistance > 10)
                {
                    continue;
                }


                return new HeaderInfo
                {
                    HeaderRow =
                        row.RowNumber(),

                    ProcessColumn =
                        processCell.Address.ColumnNumber,

                    StageColumn =
                        stageCell.Address.ColumnNumber,

                    UnitPriceColumn =
                        unitPriceCell.Address.ColumnNumber,

                    UnitColumn =
                        unitCell.Address.ColumnNumber,

                    MinimumBatchPriceColumn =
                        minimumBatchPriceCell.Address.ColumnNumber,

                    MinimumQuantityColumn =
                        minimumQuantityCell.Address.ColumnNumber
                };
            }


            // =====================================================
            // KHÔNG TÌM THẤY
            // =====================================================

            throw new InvalidOperationException(
                "Không tìm thấy đầy đủ các cột sau trong bảng " +
                "'DANH MỤC CÔNG ĐOẠN SẢN XUẤT VÀ ĐƠN GIÁ':\n\n" +

                "Quá trình\n" +
                "Công đoạn\n" +
                "Đơn giá (VNĐ)\n" +
                "Đvt\n" +
                "Giá lô min (VNĐ)\n" +
                "Số lượng tối thiểu");
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
                string normalized =
                    NormalizeHeader(
                        cell.GetString());

                if (matcher(normalized))
                {
                    return cell;
                }
            }

            return null;
        }


        // =========================================================
        // HEADER - QUÁ TRÌNH
        // =========================================================

        private bool IsProcessHeader(
            string header)
        {
            return header == "qua trinh";
        }


        // =========================================================
        // HEADER - CÔNG ĐOẠN
        // =========================================================

        private bool IsStageHeader(
            string header)
        {
            return header == "cong doan";
        }


        // =========================================================
        // HEADER - ĐƠN GIÁ
        // =========================================================

        private bool IsUnitPriceHeader(
            string header)
        {
            return
                header == "don gia vnd" ||
                header == "don gia";
        }


        // =========================================================
        // HEADER - ĐVT
        // =========================================================

        private bool IsUnitHeader(
            string header)
        {
            return header == "dvt";
        }


        // =========================================================
        // HEADER - GIÁ LÔ MIN
        // =========================================================

        private bool IsMinimumBatchPriceHeader(
            string header)
        {
            return
                header == "gia lo min vnd" ||
                header == "gia lo min";
        }


        // =========================================================
        // HEADER - SỐ LƯỢNG TỐI THIỂU
        // =========================================================

        private bool IsMinimumQuantityHeader(
            string header)
        {
            return header == "so luong toi thieu";
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


            // =====================================================
            // XỬ LÝ Đ / đ
            // =====================================================

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


            foreach (char character in normalized)
            {
                UnicodeCategory category =
                    CharUnicodeInfo.GetUnicodeCategory(
                        character);


                // =================================================
                // BỎ DẤU
                // =================================================

                if (category ==
                    UnicodeCategory.NonSpacingMark)
                {
                    continue;
                }


                // =================================================
                // CHỮ / SỐ
                // =================================================

                if (char.IsLetterOrDigit(character))
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
            if (cell == null)
            {
                return "";
            }

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
            // EXCEL LƯU TRỰC TIẾP DẠNG NUMBER
            // =====================================================

            if (cell.DataType ==
                XLDataType.Number)
            {
                return Convert.ToDecimal(
                    cell.GetDouble());
            }


            // =====================================================
            // ĐỌC TEXT
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
            // THỬ CULTURE VIỆT NAM TRƯỚC
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
            // THỬ INVARIANT
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
            // THỬ CURRENT CULTURE
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out result))
            {
                return result;
            }


            return 0;
        }


        // =========================================================
        // HEADER INFO
        // =========================================================

        private class HeaderInfo
        {
            public int HeaderRow { get; set; }

            public int ProcessColumn { get; set; }

            public int StageColumn { get; set; }

            public int UnitPriceColumn { get; set; }

            public int UnitColumn { get; set; }

            public int MinimumBatchPriceColumn { get; set; }

            public int MinimumQuantityColumn { get; set; }
        }
    }
}