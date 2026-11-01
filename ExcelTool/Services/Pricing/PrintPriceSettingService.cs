using System;
using System.Collections.Generic;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class PrintPriceSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly PrintPriceSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PrintPriceSettingService()
        {
            _repository =
                new PrintPriceSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<PrintPriceSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public PrintPriceSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            PrintPriceSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }

            return _repository.Insert(setting);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            PrintPriceSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }

            return _repository.Update(setting);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            int id)
        {
            return _repository.Delete(id);
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public bool DeleteAll()
        {
            return _repository.DeleteAll();
        }
    }
}