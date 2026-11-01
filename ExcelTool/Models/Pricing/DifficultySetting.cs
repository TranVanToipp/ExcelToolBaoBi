namespace ExcelTool.Models.Pricing
{
    public class DifficultySetting
    {
        public int Id { get; set; }

        public decimal Difficulty { get; set; }

        public decimal WasteRate { get; set; }

        public int SortOrder { get; set; }
    }
}