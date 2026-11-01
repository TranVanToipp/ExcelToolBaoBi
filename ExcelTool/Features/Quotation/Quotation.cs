using System;

namespace ExcelTool.Features.Quotation
{
    public class Quotation
    {
        public string Id { get; set; } = "";

        public string CustomerId { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string ContactPerson { get; set; } = "";

        public string Phone { get; set; } = "";

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Nháp";

        public string Note { get; set; } = "";
    }
}