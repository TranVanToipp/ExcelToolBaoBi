using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class PaperSizeSettingExcelImporter
    {
        private readonly PaperSizeSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperSizeSettingExcelImporter()
        {
            _repository =
                new PaperSizeSettingRepository();
        }


        // =========================================================
        // IMPORT EXCEL
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
            // ĐỌC DỮ LIỆU EXCEL
            // -----------------------------------------------------

            List<PaperSizeSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // XÓA DỮ LIỆU CŨ
            //
            // Import lại Excel = thay toàn bộ dữ liệu cũ
            // -----------------------------------------------------

            _repository.DeleteAll();


            // -----------------------------------------------------
            // INSERT DỮ LIỆU MỚI
            // -----------------------------------------------------

            int importedCount = 0;


            foreach (
                PaperSizeSetting setting
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
        // READ FROM EXCEL
        // =========================================================

        private List<PaperSizeSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<PaperSizeSetting>();


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
            // MENU 5
            //
            // AQ = Khổ tờ in
            // AR = Máy in khổ
            // AS = Khổ kẽm
            //
            // Dòng 2 = Header
            // Dữ liệu bắt đầu từ dòng 3
            // -----------------------------------------------------

            const int startRow = 3;


            int sortOrder = 1;


            // -----------------------------------------------------
            // TỰ XÁC ĐỊNH DÒNG CUỐI
            //
            // Không cố định endRow để sau này Excel thêm
            // dữ liệu vẫn có thể import.
            // -----------------------------------------------------

            int lastRow =
                worksheet.LastRowUsed()
                    ?.RowNumber()
                ?? 0;


            for (
                int row = startRow;
                row <= lastRow;
                row++)
            {
                // -------------------------------------------------
                // KHỔ TỜ IN
                // -------------------------------------------------

                string printSize =
                    worksheet.Cell(
                        row,
                        "AQ")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // MÁY IN KHỔ
                // -------------------------------------------------

                string printerSize =
                    worksheet.Cell(
                        row,
                        "AR")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // KHỔ KẼM
                // -------------------------------------------------

                string plateSize =
                    worksheet.Cell(
                        row,
                        "AS")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // BỎ QUA DÒNG TRỐNG
                // -------------------------------------------------

                if (
                    string.IsNullOrWhiteSpace(
                        printSize)
                    &&
                    string.IsNullOrWhiteSpace(
                        printerSize)
                    &&
                    string.IsNullOrWhiteSpace(
                        plateSize))
                {
                    continue;
                }


                // -------------------------------------------------
                // TẠO MODEL
                // -------------------------------------------------

                result.Add(
                    new PaperSizeSetting
                    {
                        PrintSize =
                            printSize,

                        PrinterSize =
                            printerSize,

                        PlateSize =
                            plateSize,

                        SortOrder =
                            sortOrder
                    });


                sortOrder++;
            }


            return result;
        }
    }
}