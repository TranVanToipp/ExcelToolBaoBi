using ClosedXML.Excel;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Services.Pricing
{
    public class BomSettingExcelImporter
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly BomSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public BomSettingExcelImporter()
        {
            _repository =
                new BomSettingRepository();
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

            List<BomSetting> settings =
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
                BomSetting setting
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

        private List<BomSetting> ReadFromExcel(
            string excelFilePath)
        {
            var result =
                new List<BomSetting>();


            // -----------------------------------------------------
            // MỞ FILE EXCEL
            // -----------------------------------------------------

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


            // =====================================================
            // CẤU TRÚC BOM TRONG EXCEL
            //
            // BD = Loại sản phẩm
            // BE = Quy cách
            // BF = Công thức
            // BO = Loại in
            // BR = Công đoạn 1
            // BS = Công đoạn 2
            //
            // Dữ liệu bắt đầu từ dòng 12
            // =====================================================

            const int startRow = 3;


            int sortOrder = 1;


            // -----------------------------------------------------
            // DÒNG CUỐI
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
                // BD - LOẠI SẢN PHẨM
                // =================================================

                string productType =
                    worksheet.Cell(
                        row,
                        "BD")
                    .GetString()
                    .Trim();


                // =================================================
                // BE - QUY CÁCH
                // =================================================

                string specification =
                    worksheet.Cell(
                        row,
                        "BE")
                    .GetString()
                    .Trim();


                // =================================================
                // BF - CÔNG THỨC
                // =================================================

                string formula =
                    worksheet.Cell(
                        row,
                        "BF")
                    .GetString()
                    .Trim();


                // =================================================
                // BO - LOẠI IN
                // =================================================

                string printingType =
                    worksheet.Cell(
                        row,
                        "BO")
                    .GetString()
                    .Trim();


                // =================================================
                // BR - CÔNG ĐOẠN 1
                // =================================================

                string process1 =
                    worksheet.Cell(
                        row,
                        "BR")
                    .GetString()
                    .Trim();


                // =================================================
                // BS - CÔNG ĐOẠN 2
                // =================================================

                string process2 =
                    worksheet.Cell(
                        row,
                        "BS")
                    .GetString()
                    .Trim();


                // =================================================
                // BỎ QUA DÒNG HOÀN TOÀN TRỐNG
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        productType)
                    &&
                    string.IsNullOrWhiteSpace(
                        specification)
                    &&
                    string.IsNullOrWhiteSpace(
                        formula)
                    &&
                    string.IsNullOrWhiteSpace(
                        printingType)
                    &&
                    string.IsNullOrWhiteSpace(
                        process1)
                    &&
                    string.IsNullOrWhiteSpace(
                        process2))
                {
                    continue;
                }


                // =================================================
                // NẾU KHÔNG CÓ TÊN SẢN PHẨM
                // THÌ KHÔNG PHẢI DÒNG BOM
                // =================================================

                if (string.IsNullOrWhiteSpace(
                        productType))
                {
                    continue;
                }


                // =================================================
                // TẠO MODEL
                // =================================================

                var setting =
                    new BomSetting
                    {
                        Id = 0,

                        ProductType =
                            productType,

                        Specification =
                            specification,

                        Formula =
                            formula,

                        PrintingType =
                            printingType,

                        Process1 =
                            process1,

                        Process2 =
                            process2,

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