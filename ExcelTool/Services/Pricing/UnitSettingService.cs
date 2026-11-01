using System;
using System.Collections.Generic;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class UnitSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly UnitSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public UnitSettingService()
        {
            _repository =
                new UnitSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<UnitSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public UnitSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            UnitSetting setting)
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
            UnitSetting setting)
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