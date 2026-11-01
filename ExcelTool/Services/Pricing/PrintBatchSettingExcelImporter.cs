using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class PrintBatchSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly PrintBatchSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PrintBatchSettingExcelImporter()
        {
            _repository =
                new PrintBatchSettingRepository();
        }


        // =========================================================
        // IMPORT
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


            List<PrintBatchSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            if (settings.Count == 0)
            {
                return 0;
            }


            // =====================================================
            // XÓA DỮ LIỆU CŨ
            // =====================================================

            _repository.DeleteAll();


            // =====================================================
            // INSERT DỮ LIỆU MỚI
            // =====================================================

            int importedCount = 0;


            foreach (
                PrintBatchSetting setting
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

        private List<PrintBatchSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<PrintBatchSetting>();


            using var workbook =
                new XLWorkbook(
                    excelFilePath);


            // =====================================================
            // SHEET ADMIN
            // =====================================================

            if (!workbook.Worksheets.TryGetWorksheet(
                    "Admin",
                    out IXLWorksheet? worksheet))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sheet 'Admin' trong file Excel.");
            }


            // =====================================================
            // CẤU TRÚC EXCEL
            //
            // DU = Lô
            // DV = Số lượng
            // DW = Đvt
            //
            // Dữ liệu bắt đầu từ dòng 3
            // =====================================================

            const int startRow = 3;


            int lastRow =
                worksheet.LastRowUsed()?.RowNumber()
                ?? 0;


            int sortOrder = 1;


            // =====================================================
            // ĐỌC TỪNG DÒNG
            // =====================================================

            for (
                int row = startRow;
                row <= lastRow;
                row++)
            {
                // =================================================
                // DU - LÔ
                // =================================================

                string lotName =
                    worksheet.Cell(
                        row,
                        "DU")
                    .GetString()
                    .Trim();


                // =================================================
                // DV - SỐ LƯỢNG
                // =================================================

                string quantityText =
                    worksheet.Cell(
                        row,
                        "DV")
                    .GetString()
                    .Trim();


                // =================================================
                // DW - ĐƠN VỊ TÍNH
                // =================================================

                string unit =
                    worksheet.Cell(
                        row,
                        "DW")
                    .GetString()
                    .Trim();


                // =================================================
                // BỎ QUA DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        lotName)
                    &&
                    string.IsNullOrWhiteSpace(
                        quantityText)
                    &&
                    string.IsNullOrWhiteSpace(
                        unit))
                {
                    continue;
                }


                // =================================================
                // CHỈ LẤY DÒNG CÓ TÊN LÔ
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        lotName))
                {
                    continue;
                }


                // =================================================
                // ĐỌC SỐ LƯỢNG
                // =================================================

                decimal quantity = 0;

                if (!string.IsNullOrWhiteSpace(quantityText))
                {
                    if (!decimal.TryParse(
                            quantityText,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out quantity))
                    {
                        if (worksheet.Cell(row, "DV").DataType
                            == XLDataType.Number)
                        {
                            quantity =
                                (decimal)worksheet.Cell(
                                    row,
                                    "DV")
                                .GetDouble();
                        }
                    }
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new PrintBatchSetting
                    {
                        Id = 0,

                        LotName =
                            lotName,

                        Quantity =
                            quantity,

                        Unit =
                            unit,

                        SortOrder =
                            sortOrder
                    };


                // =================================================
                // THÊM
                // =================================================

                result.Add(
                    setting);


                sortOrder++;
            }


            return result;
        }
    }
}