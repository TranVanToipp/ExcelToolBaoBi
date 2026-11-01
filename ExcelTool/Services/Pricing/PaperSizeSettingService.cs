using System;
using System.Collections.Generic;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class PaperSizeSettingService
    {
        private readonly PaperSizeSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperSizeSettingService()
        {
            _repository =
                new PaperSizeSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<PaperSizeSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // ADD
        // =========================================================

        public bool Add(
            string printSize,
            string printerSize,
            string plateSize)
        {
            printSize =
                printSize?.Trim() ?? "";

            printerSize =
                printerSize?.Trim() ?? "";

            plateSize =
                plateSize?.Trim() ?? "";


            if (
                string.IsNullOrWhiteSpace(printSize)
                &&
                string.IsNullOrWhiteSpace(printerSize)
                &&
                string.IsNullOrWhiteSpace(plateSize))
            {
                throw new ArgumentException(
                    "Dữ liệu khổ giấy không được để trống.");
            }


            // -----------------------------------------------------
            // KIỂM TRA TRÙNG
            // -----------------------------------------------------

            if (_repository.Exists(
                    printSize,
                    printerSize,
                    plateSize))
            {
                return false;
            }


            // -----------------------------------------------------
            // XÁC ĐỊNH SORT ORDER
            // -----------------------------------------------------

            List<PaperSizeSetting> settings =
                _repository.GetAll();


            int sortOrder =
                settings.Count + 1;


            // -----------------------------------------------------
            // TẠO MODEL
            // -----------------------------------------------------

            var setting =
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
                };


            // -----------------------------------------------------
            // INSERT
            // -----------------------------------------------------

            return _repository.Insert(
                setting);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            int id)
        {
            return _repository.Delete(
                id);
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public void DeleteAll()
        {
            _repository.DeleteAll();
        }
    }
}