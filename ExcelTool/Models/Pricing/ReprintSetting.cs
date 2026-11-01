namespace ExcelTool.Models.Pricing
{
    public class ReprintSetting
    {
        public int Id { get; set; }

        public string CheckReprint { get; set; } = "";

        public string MoldChange { get; set; } = "";

        public int SortOrder { get; set; }
    }
}