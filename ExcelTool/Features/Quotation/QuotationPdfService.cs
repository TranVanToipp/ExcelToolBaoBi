using System;
using System.Collections.Generic;
using System.IO;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using ExcelTool.Features.Settings;

namespace ExcelTool.Features.Quotation
{
    public class QuotationPdfService
    {
        // =========================================================
        // XUẤT BÁO GIÁ RA PDF
        // =========================================================

        public void Generate(
            Quotation quotation,
            List<QuotationItem> items,
            string filePath)
        {
            if (quotation == null)
                throw new ArgumentNullException(
                    nameof(quotation));

            if (items == null)
                throw new ArgumentNullException(
                    nameof(items));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    "Đường dẫn file PDF không hợp lệ.",
                    nameof(filePath));


            // =========================================================
            // LẤY THÔNG TIN CÔNG TY
            // =========================================================

            CompanySettingService companySettingService =
                new CompanySettingService();

            CompanySetting company =
                companySettingService.Get();


            // =========================================================
            // KIỂM TRA LOGO
            // =========================================================

            bool hasLogo =
                !string.IsNullOrWhiteSpace(
                    company.LogoPath)
                &&
                File.Exists(
                    company.LogoPath);


            // =========================================================
            // TẠO PDF
            // =========================================================

            Document
                .Create(document =>
                {
                    document.Page(
                        page =>
                        {
                            // =================================================
                            // TRANG
                            // =================================================

                            page.Size(
                                PageSizes.A4);

                            page.MarginHorizontal(
                                40);

                            page.MarginVertical(
                                35);


                            // =================================================
                            // FONT
                            // =================================================

                            page.DefaultTextStyle(
                                style =>
                                    style
                                        .FontSize(10)
                                        .FontFamily("Arial"));


                            // =================================================
                            // HEADER
                            // =================================================

                            page.Header()
                                .Column(column =>
                                {
                                    column.Spacing(5);


                                    // =================================================
                                    // THÔNG TIN CÔNG TY
                                    // =================================================

                                    column.Item()
                                        .Row(row =>
                                        {
                                            // -----------------------------------------
                                            // LOGO
                                            // -----------------------------------------

                                            if (hasLogo)
                                            {
                                                row.ConstantItem(80)
                                                    .Height(65)
                                                    .AlignMiddle()
                                                    .Image(
                                                        company.LogoPath)
                                                    .FitArea();
                                            }


                                            // -----------------------------------------
                                            // THÔNG TIN CÔNG TY
                                            // -----------------------------------------

                                            row.RelativeItem()
                                                .PaddingLeft(
                                                    hasLogo ? 12 : 0)
                                                .Column(companyColumn =>
                                                {
                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.CompanyName))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                company.CompanyName)
                                                            .Bold()
                                                            .FontSize(16);
                                                    }


                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.Address))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                $"Địa chỉ: {company.Address}")
                                                            .FontSize(9);
                                                    }


                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.TaxCode))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                $"Mã số thuế: {company.TaxCode}")
                                                            .FontSize(9);
                                                    }


                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.Phone))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                $"Điện thoại: {company.Phone}")
                                                            .FontSize(9);
                                                    }


                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.Email))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                $"Email: {company.Email}")
                                                            .FontSize(9);
                                                    }


                                                    if (!string.IsNullOrWhiteSpace(
                                                            company.Website))
                                                    {
                                                        companyColumn.Item()
                                                            .Text(
                                                                $"Website: {company.Website}")
                                                            .FontSize(9);
                                                    }
                                                });
                                        });


                                    // =================================================
                                    // TIÊU ĐỀ
                                    // =================================================

                                    column.Item()
                                        .PaddingTop(15)
                                        .AlignCenter()
                                        .Text("BÁO GIÁ")
                                        .Bold()
                                        .FontSize(22);


                                    // =================================================
                                    // MÃ BÁO GIÁ
                                    // =================================================

                                    column.Item()
                                        .AlignCenter()
                                        .Text(
                                            $"Mã báo giá: {quotation.Id}")
                                        .FontSize(10);


                                    // =================================================
                                    // NGÀY LẬP
                                    // =================================================

                                    column.Item()
                                        .AlignCenter()
                                        .Text(
                                            $"Ngày lập: {quotation.CreatedDate:dd/MM/yyyy}")
                                        .FontSize(10);


                                    // =================================================
                                    // ĐƯỜNG KẺ
                                    // =================================================

                                    column.Item()
                                        .PaddingTop(8)
                                        .LineHorizontal(1);
                                });


                            // =================================================
                            // NỘI DUNG
                            // =================================================

                            page.Content()
                                .PaddingTop(15)
                                .Column(column =>
                                {
                                    column.Spacing(15);


                                    // =================================================
                                    // THÔNG TIN KHÁCH HÀNG
                                    // =================================================

                                    column.Item()
                                        .Border(1)
                                        .BorderColor(
                                            Colors.Grey.Lighten2)
                                        .Padding(12)
                                        .Column(customerColumn =>
                                        {
                                            customerColumn.Spacing(5);


                                            customerColumn.Item()
                                                .Text(
                                                    "THÔNG TIN KHÁCH HÀNG")
                                                .Bold()
                                                .FontSize(12);


                                            // -----------------------------------------
                                            // KHÁCH HÀNG + NGƯỜI LIÊN HỆ
                                            // -----------------------------------------

                                            customerColumn.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Text(
                                                            $"Khách hàng: {quotation.CustomerName}");

                                                    row.RelativeItem()
                                                        .Text(
                                                            $"Người liên hệ: {quotation.ContactPerson}");
                                                });


                                            // -----------------------------------------
                                            // ĐIỆN THOẠI + TRẠNG THÁI
                                            // -----------------------------------------

                                            customerColumn.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Text(
                                                            $"Điện thoại: {quotation.Phone}");

                                                    row.RelativeItem()
                                                        .Text(
                                                            $"Trạng thái: {quotation.Status}");
                                                });
                                        });


                                    // =================================================
                                    // BẢNG SẢN PHẨM
                                    // =================================================

                                    column.Item()
                                        .Table(table =>
                                        {
                                            // -----------------------------------------
                                            // CỘT
                                            // -----------------------------------------

                                            table.ColumnsDefinition(
                                                columns =>
                                                {
                                                    columns.ConstantColumn(35);

                                                    columns.RelativeColumn(4);

                                                    columns.ConstantColumn(65);

                                                    columns.ConstantColumn(100);

                                                    columns.ConstantColumn(110);
                                                });


                                            // -----------------------------------------
                                            // HEADER
                                            // -----------------------------------------

                                            table.Header(
                                                header =>
                                                {
                                                    header.Cell()
                                                        .Element(
                                                            HeaderCell)
                                                        .AlignCenter()
                                                        .Text("STT");

                                                    header.Cell()
                                                        .Element(
                                                            HeaderCell)
                                                        .Text("Sản phẩm");

                                                    header.Cell()
                                                        .Element(
                                                            HeaderCell)
                                                        .AlignRight()
                                                        .Text("Số lượng");

                                                    header.Cell()
                                                        .Element(
                                                            HeaderCell)
                                                        .AlignRight()
                                                        .Text("Đơn giá");

                                                    header.Cell()
                                                        .Element(
                                                            HeaderCell)
                                                        .AlignRight()
                                                        .Text("Thành tiền");
                                                });


                                            // -----------------------------------------
                                            // DATA
                                            // -----------------------------------------

                                            int index = 1;


                                            foreach (
                                                QuotationItem item
                                                in items)
                                            {
                                                decimal amount =
                                                    item.Quantity *
                                                    item.UnitPrice;


                                                table.Cell()
                                                    .Element(
                                                        DataCell)
                                                    .AlignCenter()
                                                    .Text(
                                                        index.ToString());


                                                table.Cell()
                                                    .Element(
                                                        DataCell)
                                                    .Text(
                                                        item.ProductName);


                                                table.Cell()
                                                    .Element(
                                                        DataCell)
                                                    .AlignRight()
                                                    .Text(
                                                        FormatNumber(
                                                            item.Quantity));


                                                table.Cell()
                                                    .Element(
                                                        DataCell)
                                                    .AlignRight()
                                                    .Text(
                                                        FormatMoney(
                                                            item.UnitPrice));


                                                table.Cell()
                                                    .Element(
                                                        DataCell)
                                                    .AlignRight()
                                                    .Text(
                                                        FormatMoney(
                                                            amount));


                                                index++;
                                            }


                                            // -----------------------------------------
                                            // KHÔNG CÓ SẢN PHẨM
                                            // -----------------------------------------

                                            if (items.Count == 0)
                                            {
                                                table.Cell()
                                                    .ColumnSpan(5)
                                                    .Element(
                                                        DataCell)
                                                    .AlignCenter()
                                                    .Text(
                                                        "Không có sản phẩm.");
                                            }
                                        });


                                    // =================================================
                                    // TỔNG TIỀN
                                    // =================================================

                                    column.Item()
                                        .AlignRight()
                                        .Width(300)
                                        .Column(totalColumn =>
                                        {
                                            totalColumn.Spacing(5);


                                            // -----------------------------------------
                                            // TẠM TÍNH
                                            // -----------------------------------------

                                            totalColumn.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Text(
                                                            "Tạm tính:");

                                                    row.ConstantItem(130)
                                                        .AlignRight()
                                                        .Text(
                                                            FormatMoney(
                                                                quotation.SubTotal));
                                                });


                                            // -----------------------------------------
                                            // CHIẾT KHẤU
                                            // -----------------------------------------

                                            totalColumn.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Text(
                                                            "Chiết khấu:");

                                                    row.ConstantItem(130)
                                                        .AlignRight()
                                                        .Text(
                                                            FormatMoney(
                                                                quotation.Discount));
                                                });


                                            // -----------------------------------------
                                            // LINE
                                            // -----------------------------------------

                                            totalColumn.Item()
                                                .PaddingVertical(3)
                                                .LineHorizontal(1);


                                            // -----------------------------------------
                                            // TỔNG CỘNG
                                            // -----------------------------------------

                                            totalColumn.Item()
                                                .Row(row =>
                                                {
                                                    row.RelativeItem()
                                                        .Text(
                                                            "TỔNG CỘNG")
                                                        .Bold()
                                                        .FontSize(12);


                                                    row.ConstantItem(130)
                                                        .AlignRight()
                                                        .Text(
                                                            FormatMoney(
                                                                quotation.TotalAmount))
                                                        .Bold()
                                                        .FontSize(12);
                                                });
                                        });


                                    // =================================================
                                    // GHI CHÚ
                                    // =================================================

                                    if (!string.IsNullOrWhiteSpace(
                                            quotation.Note))
                                    {
                                        column.Item()
                                            .Border(1)
                                            .BorderColor(
                                                Colors.Grey.Lighten2)
                                            .Padding(10)
                                            .Column(noteColumn =>
                                            {
                                                noteColumn.Spacing(5);


                                                noteColumn.Item()
                                                    .Text(
                                                        "GHI CHÚ")
                                                    .Bold()
                                                    .FontSize(11);


                                                noteColumn.Item()
                                                    .Text(
                                                        quotation.Note);
                                            });
                                    }
                                });


                            // =================================================
                            // FOOTER
                            // =================================================

                            page.Footer()
                                .AlignCenter()
                                .Text(text =>
                                {
                                    text.Span(
                                        "Báo giá - ");

                                    text.Span(
                                        quotation.Id);

                                    text.Span(
                                        "  |  Trang ");

                                    text.CurrentPageNumber();

                                    text.Span(
                                        "/");

                                    text.TotalPages();
                                });
                        });
                })
                .GeneratePdf(
                    filePath);
        }


        // =========================================================
        // FORMAT TIỀN
        // =========================================================

        private static string FormatMoney(
            decimal value)
        {
            return value.ToString("N0")
                   + " VNĐ";
        }


        // =========================================================
        // FORMAT SỐ LƯỢNG
        // =========================================================

        private static string FormatNumber(
            decimal value)
        {
            return value.ToString("N0");
        }


        // =========================================================
        // STYLE HEADER TABLE
        // =========================================================

        private static IContainer HeaderCell(
            IContainer container)
        {
            return container
                .Background(
                    Colors.Grey.Lighten3)
                .Border(1)
                .BorderColor(
                    Colors.Grey.Lighten1)
                .Padding(7)
                .DefaultTextStyle(
                    style =>
                        style
                            .Bold()
                            .FontSize(9));
        }


        // =========================================================
        // STYLE DATA TABLE
        // =========================================================

        private static IContainer DataCell(
            IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(
                    Colors.Grey.Lighten2)
                .Padding(7);
        }
    }
}