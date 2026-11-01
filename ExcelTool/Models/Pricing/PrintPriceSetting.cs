namespace ExcelTool.Models.Pricing
{
    public class PrintPriceSetting
    {
        public int Id { get; set; }

        public string MachineSize { get; set; } = "";

        public decimal PricePerPlate { get; set; }

        public decimal Cmyk { get; set; }

        public decimal Pantone { get; set; }

        public decimal CmykJobPrice { get; set; }

        public decimal SpotColorJobPrice { get; set; }

        public int SortOrder { get; set; }
    }
}