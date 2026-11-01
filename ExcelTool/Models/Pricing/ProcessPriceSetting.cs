namespace ExcelTool.Models.Pricing
{
    public class ProcessPriceSetting
    {
        public int Id { get; set; }

        // Quá trình
        public string Process { get; set; } = "";

        // Công đoạn
        public string Stage { get; set; } = "";

        // Đơn giá (VNĐ)
        public decimal UnitPrice { get; set; }

        // Đvt
        public string Unit { get; set; } = "";

        // Giá lô min (VNĐ)
        public decimal MinimumBatchPrice { get; set; }

        // Số lượng tối thiểu
        public decimal MinimumQuantity { get; set; }

        public int SortOrder { get; set; }
    }
}