using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Quotation
{
    public partial class QuotationDetailView : UserControl
    {
        private readonly string _quotationId;

        private readonly QuotationService _quotationService;


        // ==========================================
        // KHỞI TẠO
        // ==========================================

        public QuotationDetailView(
            string quotationId)
        {
            InitializeComponent();

            _quotationId =
                quotationId;

            _quotationService =
                new QuotationService();

            LoadQuotation();
        }


        // ==========================================
        // LOAD BÁO GIÁ
        // ==========================================

        private void LoadQuotation()
        {
            try
            {
                // ==========================================
                // KIỂM TRA MÃ BÁO GIÁ
                // ==========================================

                if (string.IsNullOrWhiteSpace(
                        _quotationId))
                {
                    MessageBox.Show(
                        "Không tìm thấy mã báo giá.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // LẤY THÔNG TIN BÁO GIÁ
                // ==========================================

                Quotation? quotation =
                    _quotationService.GetById(
                        _quotationId);


                if (quotation == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy báo giá.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // HIỂN THỊ THÔNG TIN
                // ==========================================

                QuotationIdText.Text =
                    quotation.Id;


                CreatedDateText.Text =
                    quotation.CreatedDate
                        .ToString("dd/MM/yyyy");


                CustomerNameText.Text =
                    quotation.CustomerName;


                StatusText.Text =
                    quotation.Status;


                ContactPersonText.Text =
                    quotation.ContactPerson;


                PhoneText.Text =
                    quotation.Phone;


                // ==========================================
                // LẤY DANH SÁCH SẢN PHẨM
                // ==========================================

                List<QuotationItem> items =
                    _quotationService.GetItems(
                        quotation.Id);


                QuotationItemList.ItemsSource =
                    items;


                // ==========================================
                // TỔNG TIỀN
                // ==========================================

                SubTotalText.Text =
                    quotation.SubTotal
                        .ToString("N0")
                    + " VNĐ";


                DiscountText.Text =
                    quotation.Discount
                        .ToString("N0")
                    + " VNĐ";


                TotalAmountText.Text =
                    quotation.TotalAmount
                        .ToString("N0")
                    + " VNĐ";


                // ==========================================
                // GHI CHÚ
                // ==========================================

                if (string.IsNullOrWhiteSpace(
                        quotation.Note))
                {
                    NoteText.Text =
                        "Không có ghi chú.";
                }
                else
                {
                    NoteText.Text =
                        quotation.Note;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải thông tin báo giá.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // XUẤT PDF
        // ==========================================

        private void ExportPdfButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                // ==========================================
                // KIỂM TRA MÃ BÁO GIÁ
                // ==========================================

                if (string.IsNullOrWhiteSpace(
                        _quotationId))
                {
                    MessageBox.Show(
                        "Không tìm thấy mã báo giá.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // LẤY BÁO GIÁ
                // ==========================================

                Quotation? quotation =
                    _quotationService.GetById(
                        _quotationId);


                if (quotation == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy báo giá.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // LẤY DANH SÁCH SẢN PHẨM
                // ==========================================

                List<QuotationItem> items =
                    _quotationService.GetItems(
                        quotation.Id);


                // ==========================================
                // CHỌN NƠI LƯU FILE
                // ==========================================

                string defaultFileName =
                    $"BaoGia_{quotation.Id}.pdf";


                SaveFileDialog dialog =
                    new SaveFileDialog
                    {
                        Title =
                            "Lưu báo giá PDF",

                        Filter =
                            "PDF Files (*.pdf)|*.pdf",

                        FileName =
                            defaultFileName,

                        DefaultExt =
                            ".pdf",

                        AddExtension =
                            true
                    };


                bool? result =
                    dialog.ShowDialog();


                // Người dùng bấm Cancel

                if (result != true)
                {
                    return;
                }


                // ==========================================
                // KIỂM TRA ĐƯỜNG DẪN
                // ==========================================

                string filePath =
                    dialog.FileName;


                if (string.IsNullOrWhiteSpace(
                        filePath))
                {
                    MessageBox.Show(
                        "Đường dẫn file không hợp lệ.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // TẠO THƯ MỤC NẾU CHƯA CÓ
                // ==========================================

                string? directory =
                    Path.GetDirectoryName(
                        filePath);


                if (!string.IsNullOrWhiteSpace(
                        directory))
                {
                    Directory.CreateDirectory(
                        directory);
                }


                // ==========================================
                // TẠO PDF
                // ==========================================

                QuotationPdfService pdfService =
                    new QuotationPdfService();


                pdfService.Generate(
                    quotation,
                    items,
                    filePath);


                // ==========================================
                // KIỂM TRA FILE ĐÃ TẠO
                // ==========================================

                if (!File.Exists(
                        filePath))
                {
                    MessageBox.Show(
                        "Không tìm thấy file PDF sau khi xuất.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                // ==========================================
                // THÔNG BÁO THÀNH CÔNG
                // ==========================================

                MessageBoxResult openResult =
                    MessageBox.Show(
                        "Xuất báo giá PDF thành công.\n\n" +
                        $"File:\n{filePath}\n\n" +
                        "Bạn có muốn mở thư mục chứa file không?",

                        "Xuất PDF thành công",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Information);


                // ==========================================
                // MỞ THƯ MỤC
                // ==========================================

                if (openResult ==
                    MessageBoxResult.Yes)
                {
                    OpenContainingFolder(
                        filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xuất báo giá PDF.\n\n" +
                    ex.Message,

                    "Lỗi xuất PDF",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // MỞ THƯ MỤC CHỨA FILE
        // ==========================================

        private static void OpenContainingFolder(
            string filePath)
        {
            try
            {
                string argument =
                    $"/select,\"{filePath}\"";


                // Dùng đầy đủ namespace để tránh
                // xung đột với ExcelTool.Features.Process

                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName =
                            "explorer.exe",

                        Arguments =
                            argument,

                        UseShellExecute =
                            true
                    });
            }
            catch
            {
                // Không để lỗi mở Explorer
                // làm crash ứng dụng.
            }
        }


        // ==========================================
        // QUAY LẠI
        // ==========================================

        public event RoutedEventHandler BackRequested;


        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            BackRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}