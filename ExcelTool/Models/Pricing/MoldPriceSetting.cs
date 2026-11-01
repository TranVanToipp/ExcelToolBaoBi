using System;

namespace ExcelTool.Models.Pricing
{
    public class MoldPriceSetting
    {
        public int Id { get; set; }

        // Khuôn mới
        public string MoldType { get; set; } = "";

        // VNĐ/khuôn
        public decimal PricePerMold { get; set; }

        // VNĐ/cm²
        public decimal PricePerSquareCm { get; set; }

        public int SortOrder { get; set; }
    }
}