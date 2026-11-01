using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class ReprintSettingExcelImporter
    {
        private readonly ReprintSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ReprintSettingExcelImporter()
        {
            _repository =
                new ReprintSettingRepository();
        }


        // =========================================================
        // IMPORT CÀI ĐẶT TÁI BẢN TỪ EXCEL
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

            List<ReprintSetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // -----------------------------------------------------
            // LƯU VÀO SQLITE
            // -----------------------------------------------------

            int importedCount = 0;


            foreach (
                ReprintSetting setting
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

        private List<ReprintSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<ReprintSetting>();


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
            // G = Check tái bản
            // H = Thay đổi khuôn
            //
            // Dòng 2 là tiêu đề.
            // Dữ liệu bắt đầu từ dòng 3.
            // -----------------------------------------------------

            const int startRow = 3;
            const int endRow = 11;


            int sortOrder = 1;


            for (
                int row = startRow;
                row <= endRow;
                row++)
            {
                // -------------------------------------------------
                // CHECK TÁI BẢN
                // -------------------------------------------------

                string checkReprint =
                    worksheet.Cell(
                        row,
                        "G")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // THAY ĐỔI KHUÔN
                // -------------------------------------------------

                string moldChange =
                    worksheet.Cell(
                        row,
                        "H")
                    .GetString()
                    .Trim();


                // -------------------------------------------------
                // BỎ QUA DÒNG TRỐNG
                // -------------------------------------------------

                if (string.IsNullOrWhiteSpace(
                        checkReprint)
                    &&
                    string.IsNullOrWhiteSpace(
                        moldChange))
                {
                    continue;
                }


                // -------------------------------------------------
                // TẠO MODEL
                // -------------------------------------------------

                result.Add(
                    new ReprintSetting
                    {
                        CheckReprint =
                            checkReprint,

                        MoldChange =
                            moldChange,

                        SortOrder =
                            sortOrder
                    });


                sortOrder++;
            }


            return result;
        }
    }
}