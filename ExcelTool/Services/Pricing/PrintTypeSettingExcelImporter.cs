using System;
using System.Collections.Generic;
using System.Globalization;
using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class PrintTypeSettingExcelImporter
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


            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException(
                    "Không tìm thấy file Excel.",
                    filePath);
            }


            var data =
                ReadFromExcel(filePath);


            var repository =
                new PrintTypeSettingRepository();


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

        private List<PrintTypeSetting> ReadFromExcel(
            string filePath)
        {
            var result =
                new List<PrintTypeSetting>();


            using var workbook =
                new XLWorkbook(filePath);


            // =====================================================
            // SHEET ADMIN
            // =====================================================

            IXLWorksheet worksheet =
                workbook.Worksheet("Admin");


            // =====================================================
            // MENU 11 - QUY ƯỚC CÁC KIỂU IN
            //
            // CH = Công nghệ in
            // CI = Kiểu in
            // CJ = Số bộ kẽm
            // CK = Số lượt in
            // CL = Hệ số bù hao
            // CM = Hệ số giấy
            // =====================================================


            int row = 3;


            while (true)
            {
                string printingTechnology =
                    worksheet
                        .Cell(row, "CH")
                        .GetString()
                        .Trim();


                string printType =
                    worksheet
                        .Cell(row, "CI")
                        .GetString()
                        .Trim();


                string plateSetCountText =
                    worksheet
                        .Cell(row, "CJ")
                        .GetString()
                        .Trim();


                string printCountText =
                    worksheet
                        .Cell(row, "CK")
                        .GetString()
                        .Trim();


                string wasteFactorText =
                    worksheet
                        .Cell(row, "CL")
                        .GetString()
                        .Trim();


                string paperFactorText =
                    worksheet
                        .Cell(row, "CM")
                        .GetString()
                        .Trim();


                // =================================================
                // DỪNG KHI DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        printingTechnology) &&
                    string.IsNullOrWhiteSpace(
                        printType) &&
                    string.IsNullOrWhiteSpace(
                        plateSetCountText) &&
                    string.IsNullOrWhiteSpace(
                        printCountText) &&
                    string.IsNullOrWhiteSpace(
                        wasteFactorText) &&
                    string.IsNullOrWhiteSpace(
                        paperFactorText))
                {
                    break;
                }


                // =================================================
                // BỎ QUA DÒNG KHÔNG CÓ KIỂU IN
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        printType))
                {
                    row++;
                    continue;
                }


                // =================================================
                // ĐỌC SỐ BỘ KẼM
                // =================================================

                decimal plateSetCount =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "CJ"),
                        plateSetCountText);


                // =================================================
                // ĐỌC SỐ LƯỢT IN
                // =================================================

                decimal printCount =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "CK"),
                        printCountText);


                // =================================================
                // ĐỌC HỆ SỐ BÙ HAO
                // =================================================

                decimal wasteFactor =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "CL"),
                        wasteFactorText);


                // =================================================
                // ĐỌC HỆ SỐ GIẤY
                // =================================================

                decimal paperFactor =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "CM"),
                        paperFactorText);


                // =================================================
                // TẠO MODEL
                //
                // QUAN TRỌNG:
                // PrintingTechnology lấy trực tiếp từ dòng Excel
                // Không lấy lại giá trị của dòng trước.
                // =================================================

                result.Add(
                    new PrintTypeSetting
                    {
                        PrintingTechnology =
                            printingTechnology,

                        PrintType =
                            printType,

                        PlateSetCount =
                            plateSetCount,

                        PrintCount =
                            printCount,

                        WasteFactor =
                            wasteFactor,

                        PaperFactor =
                            paperFactor,

                        SortOrder =
                            result.Count
                    });


                row++;
            }


            return result;
        }


        // =========================================================
        // READ DECIMAL
        // =========================================================

        private decimal ReadDecimal(
            IXLCell cell,
            string text)
        {
            // =====================================================
            // ƯU TIÊN GIÁ TRỊ SỐ TRỰC TIẾP TỪ EXCEL
            // =====================================================

            if (cell.DataType ==
                XLDataType.Number)
            {
                return Convert.ToDecimal(
                    cell.GetDouble(),
                    CultureInfo.InvariantCulture);
            }


            // =====================================================
            // PARSE TEXT
            // =====================================================

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                return result;
            }


            // =====================================================
            // PARSE THEO CULTURE HIỆN TẠI
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
    }
}