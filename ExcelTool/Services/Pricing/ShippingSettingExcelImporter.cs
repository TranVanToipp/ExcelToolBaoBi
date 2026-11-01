using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class ShippingSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly ShippingSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ShippingSettingExcelImporter()
        {
            _repository =
                new ShippingSettingRepository();
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


            // -----------------------------------------------------
            // ĐỌC DỮ LIỆU TỪ EXCEL
            // -----------------------------------------------------

            List<ShippingSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // KHÔNG CÓ DỮ LIỆU
            // -----------------------------------------------------

            if (settings.Count == 0)
            {
                return 0;
            }


            // -----------------------------------------------------
            // XÓA DỮ LIỆU CŨ
            // -----------------------------------------------------

            _repository.DeleteAll();


            // -----------------------------------------------------
            // INSERT DỮ LIỆU MỚI
            // -----------------------------------------------------

            int importedCount = 0;


            foreach (
                ShippingSetting setting
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
        // ĐỌC DỮ LIỆU TỪ EXCEL
        // =========================================================

        private List<ShippingSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<ShippingSetting>();


            // -----------------------------------------------------
            // MỞ FILE EXCEL
            // -----------------------------------------------------

            using var workbook =
                new XLWorkbook(
                    excelFilePath);


            // -----------------------------------------------------
            // LẤY SHEET ADMIN
            // -----------------------------------------------------

            if (!workbook.Worksheets.TryGetWorksheet(
                    "Admin",
                    out IXLWorksheet? worksheet))
            {
                throw new InvalidOperationException(
                    "Không tìm thấy sheet 'Admin' trong file Excel.");
            }


            // -----------------------------------------------------
            // CẤU TRÚC EXCEL
            //
            // AX = Chiều rộng
            // AY = Tai dán
            // AZ = Khóa gài
            //
            // Dòng 2 = tiêu đề
            // Dòng 3 = bắt đầu dữ liệu
            // -----------------------------------------------------

            const int startRow = 3;


            int sortOrder = 1;


            // -----------------------------------------------------
            // XÁC ĐỊNH DÒNG CUỐI
            // -----------------------------------------------------

            int lastRow =
                worksheet.LastRowUsed()?.RowNumber()
                ?? 0;


            // -----------------------------------------------------
            // ĐỌC TỪNG DÒNG
            // -----------------------------------------------------

            for (
                int row = startRow;
                row <= lastRow;
                row++)
            {
                // =================================================
                // AX - CHIỀU RỘNG
                // =================================================

                string width =
                    worksheet.Cell(
                        row,
                        "AX")
                    .GetString()
                    .Trim();


                // =================================================
                // AY - TAI DÁN
                // =================================================

                string glueText =
                    worksheet.Cell(
                        row,
                        "AY")
                    .GetString()
                    .Trim();


                // =================================================
                // AZ - KHÓA GÀI
                // =================================================

                string lockText =
                    worksheet.Cell(
                        row,
                        "AZ")
                    .GetString()
                    .Trim();


                // =================================================
                // BỎ QUA DÒNG TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        width)
                    &&
                    string.IsNullOrWhiteSpace(
                        glueText)
                    &&
                    string.IsNullOrWhiteSpace(
                        lockText))
                {
                    continue;
                }


                // =================================================
                // KIỂM TRA CHIỀU RỘNG
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        width))
                {
                    continue;
                }


                // =================================================
                // ĐỌC TAI DÁN
                // =================================================

                decimal glue;


                if (!decimal.TryParse(
                        glueText,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out glue))
                {
                    if (!decimal.TryParse(
                            glueText,
                            out glue))
                    {
                        throw new InvalidOperationException(
                            $"Giá trị '{glueText}' tại ô AY{row} " +
                            $"không phải là số hợp lệ.");
                    }
                }


                // =================================================
                // ĐỌC KHÓA GÀI
                // =================================================

                decimal lockValue;


                if (!decimal.TryParse(
                        lockText,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out lockValue))
                {
                    if (!decimal.TryParse(
                            lockText,
                            out lockValue))
                    {
                        throw new InvalidOperationException(
                            $"Giá trị '{lockText}' tại ô AZ{row} " +
                            $"không phải là số hợp lệ.");
                    }
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new ShippingSetting
                    {
                        Id = 0,

                        Width =
                            width,

                        Glue =
                            glue,

                        Lock =
                            lockValue,

                        SortOrder =
                            sortOrder
                    };


                // =================================================
                // THÊM VÀO DANH SÁCH
                // =================================================

                result.Add(
                    setting);


                sortOrder++;
            }


            return result;
        }
    }
}