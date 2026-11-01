using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;

namespace ExcelTool.Services.Pricing
{
    public class ReprintSettingService
    {
        private readonly ReprintSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ReprintSettingService()
        {
            _repository =
                new ReprintSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<ReprintSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public ReprintSetting? GetById(
            int id)
        {
            return _repository.GetById(id);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            ReprintSetting setting)
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
            ReprintSetting setting)
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
            ReprintSetting setting)
        {
            if (string.IsNullOrWhiteSpace(
                    setting.CheckReprint))
            {
                throw new ArgumentException(
                    "Loại tái bản không được để trống.");
            }


            if (string.IsNullOrWhiteSpace(
                    setting.MoldChange))
            {
                throw new ArgumentException(
                    "Thông tin thay đổi khuôn không được để trống.");
            }


            if (setting.SortOrder < 0)
            {
                throw new ArgumentException(
                    "Thứ tự sắp xếp không được nhỏ hơn 0.");
            }
        }
    }
}