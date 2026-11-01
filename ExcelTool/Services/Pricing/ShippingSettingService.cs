using System;
using System.Collections.Generic;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class ShippingSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly ShippingSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ShippingSettingService()
        {
            _repository =
                new ShippingSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<ShippingSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public ShippingSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            ShippingSetting setting)
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
            ShippingSetting setting)
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