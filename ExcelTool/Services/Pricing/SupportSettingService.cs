using System;
using System.Collections.Generic;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class SupportSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly SupportSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public SupportSettingService()
        {
            _repository =
                new SupportSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<SupportSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public SupportSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            SupportSetting setting)
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
            SupportSetting setting)
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