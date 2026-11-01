namespace ExcelTool.Models.Pricing
{
    public class WasteSetting
    {
        public int Id { get; set; }

        public string SoTo { get; set; } = "";

        public decimal Mau01 { get; set; }

        public decimal Mau02 { get; set; }

        public decimal Mau03 { get; set; }

        public decimal Mau04 { get; set; }

        public decimal Mau05 { get; set; }

        public decimal Mau06 { get; set; }

        public decimal Mau07 { get; set; }

        public decimal CanMang { get; set; }

        public decimal TrangPhu { get; set; }

        public decimal Be { get; set; }

        public decimal CanVan { get; set; }

        public decimal EpNhu { get; set; }

        public decimal Boi { get; set; }

        public decimal DongKim { get; set; }

        public decimal Cat { get; set; }

        public decimal Dan { get; set; }

        public int SortOrder { get; set; }
    }
}