using ClosedXML.Excel;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class DifficultySettingExcelImporter
    {
        private readonly DifficultySettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public DifficultySettingExcelImporter()
        {
            _repository =
                new DifficultySettingRepository();
        }


        // =========================================================
        // IMPORT ĐỘ KHÓ TỪ EXCEL
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


            // =====================================================
            // ĐỌC EXCEL
            // =====================================================

            List<DifficultySetting> settings =
                ReadFromExcel(
                    excelFilePath);


            // =====================================================
            // KIỂM TRA DỮ LIỆU
            // =====================================================

            if (settings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy dữ liệu độ khó trong file Excel.");
            }


            // =====================================================
            // THAY THẾ TOÀN BỘ DỮ LIỆU
            // =====================================================

            int importedCount =
                _repository.ReplaceAll(
                    settings);


            return importedCount;
        }


        // =========================================================
        // ĐỌC EXCEL
        // =========================================================

        private List<DifficultySetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<DifficultySetting>();


            // =====================================================
            // MỞ WORKBOOK
            // =====================================================

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
            // VÙNG DỮ LIỆU
            //
            // CF = Độ khó SP
            // CL = Hệ số bù hao
            //
            // Dữ liệu bắt đầu từ dòng 3.
            // =====================================================

            const int startRow = 3;

            const int endRow = 23;


            int sortOrder = 1;


            // =====================================================
            // ĐỌC TỪNG DÒNG
            // =====================================================

            for (
                int row = startRow;
                row <= endRow;
                row++)
            {
                IXLCell difficultyCell =
                    worksheet.Cell(
                        row,
                        "CF");


                IXLCell wasteRateCell =
                    worksheet.Cell(
                        row,
                        "CL");


                // =================================================
                // ĐỘ KHÓ
                // =================================================

                if (difficultyCell.IsEmpty())
                {
                    continue;
                }


                if (!difficultyCell.TryGetValue<decimal>(
                        out decimal difficulty))
                {
                    continue;
                }


                // =================================================
                // TỶ LỆ BÙ HAO
                // =================================================

                decimal wasteRate = 0;


                if (!wasteRateCell.IsEmpty())
                {
                    if (!wasteRateCell.TryGetValue<decimal>(
                            out wasteRate))
                    {
                        wasteRate = 0;
                    }
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                result.Add(
                    new DifficultySetting
                    {
                        Difficulty =
                            difficulty,

                        WasteRate =
                            wasteRate,

                        SortOrder =
                            sortOrder
                    });


                sortOrder++;
            }


            return result;
        }
    }
}