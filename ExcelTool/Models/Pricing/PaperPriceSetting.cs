namespace ExcelTool.Models.Pricing
{
    public class PaperPriceSetting
    {
        public int Id { get; set; }

        public string Loai { get; set; } = "";

        public string MaGiay { get; set; } = "";

        public string TenGiay { get; set; } = "";

        public decimal VndPerM2 { get; set; }

        public decimal VndPerTon { get; set; }

        public decimal Size79x109 { get; set; }

        public decimal Size65x86 { get; set; }

        public decimal Size65x84 { get; set; }

        public decimal Size80x120 { get; set; }

        public decimal Size79x54 { get; set; }

        public decimal Size39x54 { get; set; }

        public decimal Size60x80 { get; set; }

        public decimal Size65x43 { get; set; }

        public decimal Size60x42 { get; set; }

        public int SortOrder { get; set; }
    }
}