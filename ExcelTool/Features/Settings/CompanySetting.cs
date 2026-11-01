namespace ExcelTool.Features.Settings
{
    public class CompanySetting
    {
        // ==========================================
        // ID
        // ==========================================

        public int Id { get; set; }


        // ==========================================
        // THÔNG TIN CÔNG TY
        // ==========================================

        public string CompanyName { get; set; } = "";

        public string Address { get; set; } = "";

        public string TaxCode { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Email { get; set; } = "";

        public string Website { get; set; } = "";


        // ==========================================
        // LOGO
        // ==========================================

        public string LogoPath { get; set; } = "";
    }
}