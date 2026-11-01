using System;

namespace ExcelTool.Models
{
    public class Product
    {
        // =========================================================
        // ID
        // =========================================================

        public string Id { get; set; } = string.Empty;


        // =========================================================
        // MÃ SẢN PHẨM
        // =========================================================

        public string ProductCode { get; set; } = string.Empty;


        // =========================================================
        // THÔNG TIN CHUNG
        // =========================================================

        public DateTime CreatedDate { get; set; }

        public string CustomerId { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string ProductType { get; set; } = string.Empty;

        public string ReprintType { get; set; } = string.Empty;

        public string Specification { get; set; } = string.Empty;

        public string SpreadSize { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public double Difficulty { get; set; }

        public DateTime? QuotationDeadline { get; set; }

        public string Note { get; set; } = string.Empty;


        // =========================================================
        // ĐƠN VỊ / GIÁ
        // =========================================================

        public string Unit { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }


        // =========================================================
        // MÔ TẢ
        // =========================================================

        public string Description { get; set; } = string.Empty;
    }
}