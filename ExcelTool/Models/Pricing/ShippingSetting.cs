namespace ExcelTool.Models.Pricing
{
    public class ShippingSetting
    {
        public int Id { get; set; }

        public string Width { get; set; } = string.Empty;

        public decimal Glue { get; set; }

        public decimal Lock { get; set; }

        public int SortOrder { get; set; }
    }
}