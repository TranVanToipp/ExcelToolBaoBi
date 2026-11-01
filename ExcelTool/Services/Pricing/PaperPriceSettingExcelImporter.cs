using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class PaperPriceSettingExcelImporter
    {
        private readonly PaperPriceSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperPriceSettingExcelImporter()
        {
            _repository =
                new PaperPriceSettingRepository();
        }


        // =========================================================
        // IMPORT GIÁ GIẤY TỪ EXCEL
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

            List<PaperPriceSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // XÓA DỮ LIỆU CŨ
            // -----------------------------------------------------

            _repository.DeleteAll();


            // -----------------------------------------------------
            // INSERT DỮ LIỆU MỚI
            // -----------------------------------------------------

            int importedCount = 0;


            foreach (
                PaperPriceSetting setting
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
        // ĐỌC EXCEL
        // =========================================================

        private List<PaperPriceSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<PaperPriceSetting>();


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
            // VÙNG DỮ LIỆU
            //
            // AB = Loại
            // AC = Mã giấy
            // AD = Tên giấy
            // AE = VNĐ/m²
            // AF = VNĐ/tấn
            // AG = 79x109
            // AH = 65x86
            // AI = 65x84
            // AJ = 80x120
            // AK = 79x54
            // AL = 39x54
            // AM = 60x80
            // AN = 65x43
            // AO = 60x42
            //
            // Dữ liệu bắt đầu từ dòng 3.
            // -----------------------------------------------------

            const int startRow = 3;


            int endRow =
                worksheet.LastRowUsed()
                    ?.RowNumber()
                ?? startRow - 1;


            int sortOrder = 1;


            for (
                int row = startRow;
                row <= endRow;
                row++)
            {
                // -------------------------------------------------
                // LOẠI
                // -------------------------------------------------

                string loai =
                    worksheet.Cell(
                            row,
                            "AB")
                        .GetString()
                        .Trim();


                // -------------------------------------------------
                // MÃ GIẤY
                // -------------------------------------------------

                string maGiay =
                    worksheet.Cell(
                            row,
                            "AC")
                        .GetString()
                        .Trim();


                // -------------------------------------------------
                // TÊN GIẤY
                // -------------------------------------------------

                string tenGiay =
                    worksheet.Cell(
                            row,
                            "AD")
                        .GetString()
                        .Trim();


                // -------------------------------------------------
                // BỎ QUA DÒNG TRỐNG
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(loai) &&
                    string.IsNullOrWhiteSpace(maGiay) &&
                    string.IsNullOrWhiteSpace(tenGiay))
                {
                    continue;
                }


                // -------------------------------------------------
                // GIÁ
                // -------------------------------------------------

                decimal vndPerM2 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AE"));


                decimal vndPerTon =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AF"));


                // -------------------------------------------------
                // KHỔ GIẤY
                // -------------------------------------------------

                decimal size79x109 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AG"));


                decimal size65x86 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AH"));


                decimal size65x84 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AI"));


                decimal size80x120 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AJ"));


                decimal size79x54 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AK"));


                decimal size39x54 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AL"));


                decimal size60x80 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AM"));


                decimal size65x43 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AN"));


                decimal size60x42 =
                    ReadDecimal(
                        worksheet.Cell(
                            row,
                            "AO"));


                // -------------------------------------------------
                // TẠO MODEL
                // -------------------------------------------------

                result.Add(
                    new PaperPriceSetting
                    {
                        Loai =
                            loai,

                        MaGiay =
                            maGiay,

                        TenGiay =
                            tenGiay,

                        VndPerM2 =
                            vndPerM2,

                        VndPerTon =
                            vndPerTon,

                        Size79x109 =
                            size79x109,

                        Size65x86 =
                            size65x86,

                        Size65x84 =
                            size65x84,

                        Size80x120 =
                            size80x120,

                        Size79x54 =
                            size79x54,

                        Size39x54 =
                            size39x54,

                        Size60x80 =
                            size60x80,

                        Size65x43 =
                            size65x43,

                        Size60x42 =
                            size60x42,

                        SortOrder =
                            sortOrder
                    });


                sortOrder++;
            }


            return result;
        }


        // =========================================================
        // ĐỌC DECIMAL
        // =========================================================
        //
        // Ưu tiên giá trị đang hiển thị theo format của Excel.
        //
        // Ví dụ:
        //
        // Excel hiển thị:
        // 2,595
        //
        // Giá trị thực bên trong:
        // 2594.9267578125
        //
        // Kết quả import:
        // 2595
        //
        // =========================================================

        private static decimal ReadDecimal(
            IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return 0;
            }


            string text =
                cell.GetFormattedString()
                    .Trim();


            // -----------------------------------------------------
            // DẤU -
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(text) ||
                text == "-")
            {
                return 0;
            }


            // -----------------------------------------------------
            // LOẠI BỎ DẤU PHÂN CÁCH HÀNG NGHÌN
            // -----------------------------------------------------

            text =
                text.Replace(",", "")
                    .Replace(" ", "")
                    .Trim();


            // -----------------------------------------------------
            // ĐỌC DECIMAL
            // -----------------------------------------------------

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                return result;
            }


            // -----------------------------------------------------
            // THỬ CULTURE HIỆN TẠI
            // -----------------------------------------------------

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