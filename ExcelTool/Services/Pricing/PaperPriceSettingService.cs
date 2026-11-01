using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;

namespace ExcelTool.Services.Pricing
{
    public class PaperPriceSettingService
    {
        private readonly PaperPriceSettingRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperPriceSettingService()
        {
            _repository =
                new PaperPriceSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<PaperPriceSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public PaperPriceSetting? GetById(
            int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "ID không hợp lệ.",
                    nameof(id));
            }


            return _repository.GetById(id);
        }


        // =========================================================
        // INSERT
        // =========================================================

        public bool Insert(
            PaperPriceSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }


            Validate(setting);


            return _repository.Insert(
                setting);
        }


        // =========================================================
        // UPDATE
        // =========================================================

        public bool Update(
            PaperPriceSetting setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(
                    nameof(setting));
            }


            if (setting.Id <= 0)
            {
                throw new ArgumentException(
                    "ID không hợp lệ.",
                    nameof(setting));
            }


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
            {
                throw new ArgumentException(
                    "ID không hợp lệ.",
                    nameof(id));
            }


            return _repository.Delete(id);
        }


        // =========================================================
        // DELETE ALL
        // =========================================================

        public void DeleteAll()
        {
            _repository.DeleteAll();
        }


        // =========================================================
        // VALIDATE
        // =========================================================

        private static void Validate(
            PaperPriceSetting setting)
        {
            // -----------------------------------------------------
            // TÊN GIẤY
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                    setting.TenGiay))
            {
                throw new ArgumentException(
                    "Tên giấy không được để trống.");
            }


            // -----------------------------------------------------
            // GIÁ VNĐ / M²
            // -----------------------------------------------------

            if (setting.VndPerM2 < 0)
            {
                throw new ArgumentException(
                    "Giá VNĐ/m² không được nhỏ hơn 0.");
            }


            // -----------------------------------------------------
            // GIÁ VNĐ / TẤN
            // -----------------------------------------------------

            if (setting.VndPerTon < 0)
            {
                throw new ArgumentException(
                    "Giá VNĐ/tấn không được nhỏ hơn 0.");
            }


            // -----------------------------------------------------
            // GIÁ THEO KHỔ
            // -----------------------------------------------------

            if (setting.Size79x109 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 79x109 không được nhỏ hơn 0.");
            }


            if (setting.Size65x86 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 65x86 không được nhỏ hơn 0.");
            }


            if (setting.Size65x84 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 65x84 không được nhỏ hơn 0.");
            }


            if (setting.Size80x120 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 80x120 không được nhỏ hơn 0.");
            }


            if (setting.Size79x54 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 79x54 không được nhỏ hơn 0.");
            }


            if (setting.Size39x54 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 39x54 không được nhỏ hơn 0.");
            }


            if (setting.Size60x80 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 60x80 không được nhỏ hơn 0.");
            }


            if (setting.Size65x43 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 65x43 không được nhỏ hơn 0.");
            }


            if (setting.Size60x42 < 0)
            {
                throw new ArgumentException(
                    "Giá khổ 60x42 không được nhỏ hơn 0.");
            }


            // -----------------------------------------------------
            // SORT ORDER
            // -----------------------------------------------------

            if (setting.SortOrder < 0)
            {
                throw new ArgumentException(
                    "Thứ tự sắp xếp không được nhỏ hơn 0.");
            }
        }
    }
}