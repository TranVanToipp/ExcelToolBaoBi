using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;

namespace ExcelTool.Services.Pricing
{
    public class DifficultySettingService
    {
        private readonly DifficultySettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public DifficultySettingService()
        {
            _repository =
                new DifficultySettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<DifficultySetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public DifficultySetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            DifficultySetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(
                    nameof(setting));

            Validate(setting);

            return _repository.Insert(
                setting);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            DifficultySetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(
                    nameof(setting));

            if (setting.Id <= 0)
                throw new ArgumentException(
                    "ID không hợp lệ.",
                    nameof(setting));

            Validate(setting);

            return _repository.Update(
                setting);
        }


        // =========================================================
        // DELETE
        // =========================================================

        public bool Delete(
            int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "ID không hợp lệ.",
                    nameof(id));

            return _repository.Delete(id);
        }


        // =========================================================
        // VALIDATE
        // =========================================================

        private static void Validate(
            DifficultySetting setting)
        {
            if (setting.Difficulty < 0)
            {
                throw new ArgumentException(
                    "Độ khó không được nhỏ hơn 0.");
            }


            if (setting.WasteRate < 0)
            {
                throw new ArgumentException(
                    "Tỷ lệ bù hao không được nhỏ hơn 0.");
            }


            if (setting.SortOrder < 0)
            {
                throw new ArgumentException(
                    "Thứ tự sắp xếp không được nhỏ hơn 0.");
            }
        }
    }
}