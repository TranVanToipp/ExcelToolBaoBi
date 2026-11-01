using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class GripSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly GripSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public GripSettingExcelImporter()
        {
            _repository =
                new GripSettingRepository();
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
            // ĐỌC EXCEL
            // -----------------------------------------------------

            List<GripSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // NẾU KHÔNG CÓ DỮ LIỆU
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
                GripSetting setting
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

        private List<GripSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<GripSetting>();


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
            // CẤU TRÚC EXCEL
            //
            // AU = Thiết lập
            // AV = Giá trị
            //
            // Dòng 2 = tiêu đề
            // Dữ liệu bắt đầu từ dòng 3
            // -----------------------------------------------------

            const int startRow = 3;


            int sortOrder = 1;


            // -----------------------------------------------------
            // ĐỌC TỪ DÒNG 3 TRỞ ĐI
            // -----------------------------------------------------

            for (
                int row = startRow;
                row <= worksheet.LastRowUsed().RowNumber();
                row++)
            {
                // -------------------------------------------------
                // TÊN THIẾT LẬP
                // -------------------------------------------------

                string settingName =
                    worksheet.Cell(
                        row,
                        "AU")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // GIÁ TRỊ
                // -------------------------------------------------

                string valueText =
                    worksheet.Cell(
                        row,
                        "AV")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // BỎ QUA DÒNG TRỐNG
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                        settingName)
                    &&
                    string.IsNullOrWhiteSpace(
                        valueText))
                {
                    continue;
                }


                // -------------------------------------------------
                // KIỂM TRA TÊN
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                        settingName))
                {
                    continue;
                }


                // -------------------------------------------------
                // ĐỌC DECIMAL
                // -------------------------------------------------

                decimal value;


                if (!decimal.TryParse(
                        valueText,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out value))
                {
                    // Thử parse theo culture hiện tại
                    if (!decimal.TryParse(
                            valueText,
                            out value))
                    {
                        throw new InvalidOperationException(
                            $"Giá trị '{valueText}' tại ô AV{row} " +
                            $"không phải là số hợp lệ.");
                    }
                }


                // -------------------------------------------------
                // TẠO MODEL
                // -------------------------------------------------

                result.Add(
                    new GripSetting
                    {
                        SettingName =
                            settingName,

                        Value =
                            value,

                        SortOrder =
                            sortOrder
                    });


                sortOrder++;
            }


            return result;
        }
    }
}