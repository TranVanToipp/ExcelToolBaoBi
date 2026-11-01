using ExcelTool.Models.Pricing;
using ExcelTool.Repositories;
using System;
using System.Collections.Generic;

namespace ExcelTool.Services.Pricing
{
    public class WasteSettingService
    {
        // =========================================================
        // REPOSITORY
        // =========================================================

        private readonly WasteSettingRepository
            _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public WasteSettingService()
        {
            _repository =
                new WasteSettingRepository();
        }


        // =========================================================
        // GET ALL
        // =========================================================

        public List<WasteSetting> GetAll()
        {
            return _repository.GetAll();
        }


        // =========================================================
        // GET BY ID
        // =========================================================

        public WasteSetting? GetById(
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
            WasteSetting setting)
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
            WasteSetting setting)
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
        // VALIDATE
        // =========================================================

        private static void Validate(
            WasteSetting setting)
        {
            // -----------------------------------------------------
            // SỐ TỜ
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                    setting.SoTo))
            {
                throw new ArgumentException(
                    "Số tờ không được để trống.");
            }


            // -----------------------------------------------------
            // CÁC ĐỊNH MỨC KHÔNG ĐƯỢC ÂM
            // -----------------------------------------------------

            if (setting.Mau01 < 0)
            {
                throw new ArgumentException(
                    "Định mức 01 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau02 < 0)
            {
                throw new ArgumentException(
                    "Định mức 02 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau03 < 0)
            {
                throw new ArgumentException(
                    "Định mức 03 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau04 < 0)
            {
                throw new ArgumentException(
                    "Định mức 04 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau05 < 0)
            {
                throw new ArgumentException(
                    "Định mức 05 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau06 < 0)
            {
                throw new ArgumentException(
                    "Định mức 06 màu không được nhỏ hơn 0.");
            }


            if (setting.Mau07 < 0)
            {
                throw new ArgumentException(
                    "Định mức 07 màu không được nhỏ hơn 0.");
            }


            if (setting.CanMang < 0)
            {
                throw new ArgumentException(
                    "Định mức cán màng không được nhỏ hơn 0.");
            }


            if (setting.TrangPhu < 0)
            {
                throw new ArgumentException(
                    "Định mức tráng phủ không được nhỏ hơn 0.");
            }


            if (setting.Be < 0)
            {
                throw new ArgumentException(
                    "Định mức bế không được nhỏ hơn 0.");
            }


            if (setting.CanVan < 0)
            {
                throw new ArgumentException(
                    "Định mức cán vân không được nhỏ hơn 0.");
            }


            if (setting.EpNhu < 0)
            {
                throw new ArgumentException(
                    "Định mức ép nhũ không được nhỏ hơn 0.");
            }


            if (setting.Boi < 0)
            {
                throw new ArgumentException(
                    "Định mức bồi không được nhỏ hơn 0.");
            }


            if (setting.DongKim < 0)
            {
                throw new ArgumentException(
                    "Định mức đóng kim không được nhỏ hơn 0.");
            }


            if (setting.Cat < 0)
            {
                throw new ArgumentException(
                    "Định mức cắt không được nhỏ hơn 0.");
            }


            if (setting.Dan < 0)
            {
                throw new ArgumentException(
                    "Định mức dán không được nhỏ hơn 0.");
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