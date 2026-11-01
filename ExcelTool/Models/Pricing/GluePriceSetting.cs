namespace ExcelTool.Models.Pricing
{
    public class GluePriceSetting
    {
        public int Id { get; set; }

        // Loại keo
        public string LoaiKeo { get; set; } = string.Empty;

        // VNĐ/kg
        public decimal VndPerKg { get; set; }

        // VNĐ/10cm
        public decimal VndPer10Cm { get; set; }

        // Thứ tự hiển thị
        public int SortOrder { get; set; }
    }
}