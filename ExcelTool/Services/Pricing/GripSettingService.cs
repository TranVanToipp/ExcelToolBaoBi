using System;
using System.Collections.Generic;
using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;

namespace ExcelTool.Services.Pricing
{
    public class GripSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly GripSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public GripSettingService()
        {
            _repository =
                new GripSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<GripSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public GripSetting? GetById(
            int id)
        {
            return _repository.GetById(
                id);
        }


        // =========================================================
        // CREATE
        // =========================================================

        public bool Create(
            GripSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }


            if (string.IsNullOrWhiteSpace(
                    setting.SettingName))
            {
                throw new ArgumentException(
                    "Tên thiết lập không được để trống.",
                    nameof(setting));
            }


            return _repository.Insert(
                setting);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            GripSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }


            if (setting.Id <= 0)
            {
                throw new ArgumentException(
                    "Id thiết lập không hợp lệ.",
                    nameof(setting));
            }


            if (string.IsNullOrWhiteSpace(
                    setting.SettingName))
            {
                throw new ArgumentException(
                    "Tên thiết lập không được để trống.",
                    nameof(setting));
            }


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
            {
                throw new ArgumentException(
                    "Id thiết lập không hợp lệ.",
                    nameof(id));
            }


            return _repository.Delete(
                id);
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