namespace ExcelTool.Models.Pricing
{
    public class SupportSetting
    {
        public int Id { get; set; }

        public string Loai { get; set; } = string.Empty;

        public decimal VndPerKg { get; set; }

        public decimal VndPerM2 { get; set; }

        public int SortOrder { get; set; }
    }
}