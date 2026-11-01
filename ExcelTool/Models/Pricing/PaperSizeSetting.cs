namespace ExcelTool.Models.Pricing
{
    public class PaperSizeSetting
    {
        public int Id { get; set; }

        public string PrintSize { get; set; } = "";

        public string PrinterSize { get; set; } = "";

        public string PlateSize { get; set; } = "";

        public int SortOrder { get; set; }
    }
}