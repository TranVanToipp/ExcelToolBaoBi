namespace ExcelTool.Models.Pricing
{
    public class UnitSetting
    {
        public int Id { get; set; }

        // Đơn giá
        public string Unit { get; set; } = "";

        public int SortOrder { get; set; }
    }
}