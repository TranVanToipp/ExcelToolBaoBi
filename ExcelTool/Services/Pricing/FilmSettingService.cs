using System;
using System.Collections.Generic;

using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class FilmSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly FilmSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FilmSettingService()
        {
            _repository =
                new FilmSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<FilmSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public FilmSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            FilmSetting setting)
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
            FilmSetting setting)
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