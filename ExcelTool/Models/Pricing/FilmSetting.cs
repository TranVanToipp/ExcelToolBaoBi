namespace ExcelTool.Models.Pricing
{
    public class FilmSetting
    {
        public int Id { get; set; }

        public string LoaiMang { get; set; } = string.Empty;

        public decimal DayMicron { get; set; }

        public decimal DaiM { get; set; }

        public decimal TrongLuongKgPerM { get; set; }

        public decimal M2PerKg { get; set; }

        public int SortOrder { get; set; }
    }
}