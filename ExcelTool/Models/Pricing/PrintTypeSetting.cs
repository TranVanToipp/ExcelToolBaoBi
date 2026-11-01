namespace ExcelTool.Models.Pricing
{
    public class PrintTypeSetting
    {
        public int Id { get; set; }

        public string PrintingTechnology { get; set; } = "";

        public string PrintType { get; set; } = "";

        public decimal PlateSetCount { get; set; }

        public decimal PrintCount { get; set; }

        public decimal WasteFactor { get; set; }

        public decimal PaperFactor { get; set; }

        public int SortOrder { get; set; }
    }
}