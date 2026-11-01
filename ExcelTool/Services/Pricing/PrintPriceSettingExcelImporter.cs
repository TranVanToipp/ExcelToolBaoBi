using System;
using System.Collections.Generic;
using System.Globalization;
using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class PrintPriceSettingExcelImporter
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

            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException(
                    "Không tìm thấy file Excel.",
                    filePath);
            }

            // Đọc Excel trước.
            // Nếu Excel lỗi thì chưa đụng vào DB.
            var data = ReadFromExcel(filePath);

            var repository =
                new PrintPriceSettingRepository();

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

        private List<PrintPriceSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<PrintPriceSetting>();

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

            int headerRow =
                FindHeaderRow(
                    worksheet,
                    "Khổ máy");


            if (headerRow == -1)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dòng tiêu đề của bảng " +
                    "'Đơn giá kẽm in và mực in'.\n\n" +
                    "Không tìm thấy cột 'Khổ máy'.");
            }


            // =====================================================
            // TÌM CÁC CỘT THEO HEADER
            // =====================================================

            int machineSizeColumn =
                -1;

            int pricePerPlateColumn =
                -1;

            int cmykColumn =
                -1;

            int pantoneColumn =
                -1;

            int cmykJobPriceColumn =
                -1;

            int spotColorJobPriceColumn =
                -1;


            foreach (var cell in
                     worksheet
                         .Row(headerRow)
                         .CellsUsed())
            {
                string header =
                    NormalizeHeader(
                        cell.GetString());


                if (IsHeader(
                        header,
                        "khổmáy"))
                {
                    machineSizeColumn =
                        cell.Address.ColumnNumber;
                }
                else if (IsHeader(
                             header,
                             "vnđ/kẽm"))
                {
                    pricePerPlateColumn =
                        cell.Address.ColumnNumber;
                }
                else if (IsHeader(
                             header,
                             "cmyk"))
                {
                    cmykColumn =
                        cell.Address.ColumnNumber;
                }
                else if (IsHeader(
                             header,
                             "patone",
                             "pantone"))
                {
                    pantoneColumn =
                        cell.Address.ColumnNumber;
                }
                else if (IsHeader(
                             header,
                             "giábàicmyk"))
                {
                    cmykJobPriceColumn =
                        cell.Address.ColumnNumber;
                }
                else if (IsHeader(
                             header,
                             "giábàimàupha"))
                {
                    spotColorJobPriceColumn =
                        cell.Address.ColumnNumber;
                }
            }


            // =====================================================
            // KIỂM TRA HEADER
            // =====================================================

            var missingHeaders =
                new List<string>();


            if (machineSizeColumn == -1)
            {
                missingHeaders.Add(
                    "Khổ máy");
            }


            if (pricePerPlateColumn == -1)
            {
                missingHeaders.Add(
                    "VNĐ/kẽm");
            }


            if (cmykColumn == -1)
            {
                missingHeaders.Add(
                    "CMYK");
            }


            if (pantoneColumn == -1)
            {
                missingHeaders.Add(
                    "Patone/Pantone");
            }


            if (cmykJobPriceColumn == -1)
            {
                missingHeaders.Add(
                    "Giá bài CMYK");
            }


            if (spotColorJobPriceColumn == -1)
            {
                missingHeaders.Add(
                    "Giá bài màu pha");
            }


            if (missingHeaders.Count > 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy các cột sau trong bảng " +
                    "'Đơn giá kẽm in và mực in':\n\n" +
                    string.Join(
                        "\n",
                        missingHeaders));
            }


            // =====================================================
            // ĐỌC DATA
            // =====================================================

            int firstDataRow =
                headerRow + 1;


            bool hasData =
                false;


            foreach (var row in
                     worksheet.RowsUsed())
            {
                int rowNumber =
                    row.RowNumber();


                // =================================================
                // CHỈ ĐỌC PHÍA DƯỚI HEADER
                // =================================================

                if (rowNumber < firstDataRow)
                {
                    continue;
                }


                // =================================================
                // BỎ QUA ROW HIDDEN
                //
                // File Excel hiện tại của bạn dùng các dòng hidden
                // để chứa dữ liệu không hiển thị trong bảng.
                // =================================================

                if (row.IsHidden)
                {
                    continue;
                }


                // =================================================
                // ĐỌC CÁC CELL
                // =================================================

                string machineSize =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        machineSizeColumn);


                string pricePerPlateText =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        pricePerPlateColumn);


                string cmykText =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        cmykColumn);


                string pantoneText =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        pantoneColumn);


                string cmykJobPriceText =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        cmykJobPriceColumn);


                string spotColorJobPriceText =
                    GetCellText(
                        worksheet,
                        rowNumber,
                        spotColorJobPriceColumn);


                // =================================================
                // KIỂM TRA DÒNG TRỐNG
                // =================================================

                bool isEmptyRow =
                    string.IsNullOrWhiteSpace(
                        machineSize) &&

                    string.IsNullOrWhiteSpace(
                        pricePerPlateText) &&

                    string.IsNullOrWhiteSpace(
                        cmykText) &&

                    string.IsNullOrWhiteSpace(
                        pantoneText) &&

                    string.IsNullOrWhiteSpace(
                        cmykJobPriceText) &&

                    string.IsNullOrWhiteSpace(
                        spotColorJobPriceText);


                if (isEmptyRow)
                {
                    // Nếu đã đọc được dữ liệu rồi mà gặp
                    // một dòng trống thì kết thúc bảng.
                    if (hasData)
                    {
                        break;
                    }

                    continue;
                }


                // =================================================
                // DÒNG KHÔNG CÓ KHỔ MÁY
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        machineSize))
                {
                    continue;
                }


                hasData = true;


                // =================================================
                // ĐỌC SỐ
                // =================================================

                decimal pricePerPlate =
                    ReadDecimal(
                        worksheet.Cell(
                            rowNumber,
                            pricePerPlateColumn));


                decimal cmyk =
                    ReadDecimal(
                        worksheet.Cell(
                            rowNumber,
                            cmykColumn));


                decimal pantone =
                    ReadDecimal(
                        worksheet.Cell(
                            rowNumber,
                            pantoneColumn));


                decimal cmykJobPrice =
                    ReadDecimal(
                        worksheet.Cell(
                            rowNumber,
                            cmykJobPriceColumn));


                decimal spotColorJobPrice =
                    ReadDecimal(
                        worksheet.Cell(
                            rowNumber,
                            spotColorJobPriceColumn));


                // =================================================
                // TẠO MODEL
                // =================================================

                result.Add(
                    new PrintPriceSetting
                    {
                        MachineSize =
                            machineSize,

                        PricePerPlate =
                            pricePerPlate,

                        Cmyk =
                            cmyk,

                        Pantone =
                            pantone,

                        CmykJobPrice =
                            cmykJobPrice,

                        SpotColorJobPrice =
                            spotColorJobPrice,

                        SortOrder =
                            result.Count
                    });
            }


            return result;
        }


        // =========================================================
        // FIND HEADER ROW
        // =========================================================

        private int FindHeaderRow(
            IXLWorksheet worksheet,
            string headerName)
        {
            string normalizedTarget =
                NormalizeHeader(
                    headerName);


            foreach (var row in
                     worksheet.RowsUsed())
            {
                foreach (var cell in
                         row.CellsUsed())
                {
                    string value =
                        NormalizeHeader(
                            cell.GetString());


                    if (value ==
                        normalizedTarget)
                    {
                        return row.RowNumber();
                    }
                }
            }


            return -1;
        }


        // =========================================================
        // CHECK HEADER
        // =========================================================

        private bool IsHeader(
    string actualHeader,
    params string[] expectedHeaders)
        {
            if (string.IsNullOrWhiteSpace(actualHeader))
            {
                return false;
            }

            foreach (string expectedHeader
                     in expectedHeaders)
            {
                string normalizedExpected =
                    NormalizeHeader(
                        expectedHeader);

                if (string.IsNullOrWhiteSpace(
                    normalizedExpected))
                {
                    continue;
                }

                // ---------------------------------------------------------
                // Khớp chính xác
                // ---------------------------------------------------------

                if (actualHeader ==
                    normalizedExpected)
                {
                    return true;
                }


                // ---------------------------------------------------------
                // Cho phép header có thêm thông tin phía sau
                //
                // Ví dụ:
                //
                // CMYK
                //     ↓
                // CMYK (VNĐ/kẽm)
                //
                // Patone
                //     ↓
                // Patone (VNĐ/màu)
                //
                // Giá bài CMYK
                //     ↓
                // Giá bài CMYK (VNĐ/kẽm)
                // ---------------------------------------------------------

                if (actualHeader.StartsWith(
                    normalizedExpected,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        // =========================================================
        // NORMALIZE HEADER
        // =========================================================

        private string NormalizeHeader(
            string value)
        {
            if (string.IsNullOrWhiteSpace(
                    value))
            {
                return "";
            }


            return value
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "")
                .Replace(" ", "")
                .Replace("　", "")
                .Trim()
                .ToLowerInvariant();
        }


        // =========================================================
        // GET CELL TEXT
        // =========================================================

        private string GetCellText(
            IXLWorksheet worksheet,
            int row,
            int column)
        {
            if (column <= 0)
            {
                return "";
            }


            return worksheet
                .Cell(
                    row,
                    column)
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
            // EXCEL LƯU DẠNG NUMBER
            // =====================================================

            if (cell.DataType ==
                XLDataType.Number)
            {
                return Convert.ToDecimal(
                    cell.GetDouble());
            }


            // =====================================================
            // EXCEL LƯU DẠNG TEXT
            // =====================================================

            string text =
                cell.GetString()
                    .Trim();


            if (string.IsNullOrWhiteSpace(
                    text))
            {
                return 0;
            }


            // =====================================================
            // CULTURE HIỆN TẠI
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out decimal result))
            {
                return result;
            }


            // =====================================================
            // INVARIANT CULTURE
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out result))
            {
                return result;
            }


            return 0;
        }
    }
}