namespace ExcelTool.Features.Quotation
{
    public class QuotationItem
    {
        public int Id { get; set; }

        public string QuotationId { get; set; } = "";

        public string ProductId { get; set; } = "";

        public string ProductName { get; set; } = "";

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount =>
            Quantity * UnitPrice;
    }
}