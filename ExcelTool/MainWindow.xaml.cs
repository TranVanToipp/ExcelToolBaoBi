using System.Windows;

using ExcelTool.Features.Dashboard;
using ExcelTool.Features.Customer;
using ExcelTool.Features.Data;
using ExcelTool.Features.Quotation;
using ExcelTool.Features.Production;
using ExcelTool.Features.Attachment;
using ExcelTool.Features.Report;
using ExcelTool.Features.Tool;
using ExcelTool.Features.Settings;
using ExcelTool.Features.About;

namespace ExcelTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            // ==========================================
            // MÀN HÌNH MẶC ĐỊNH
            // ==========================================

            MainContent.Content =
                new DashboardView();


            // ==========================================
            // SIDEBAR EVENTS
            // ==========================================

            Sidebar.DataRequested +=
                Sidebar_DataRequested;

            Sidebar.CustomerRequested +=
                Sidebar_CustomerRequested;

            Sidebar.QuotationRequested +=
                Sidebar_QuotationRequested;

            Sidebar.ProductionRequested +=
                Sidebar_ProductionRequested;

            Sidebar.AttachmentRequested +=
                Sidebar_AttachmentRequested;

            Sidebar.ReportRequested +=
                Sidebar_ReportRequested;

            Sidebar.ToolRequested +=
                Sidebar_ToolRequested;

            Sidebar.SettingsRequested +=
                Sidebar_SettingsRequested;

            Sidebar.AboutRequested +=
                Sidebar_AboutRequested;
        }


        // ==========================================
        // GIỚI THIỆU
        // ==========================================

        private void Sidebar_AboutRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new AboutView();
        }

        // ==========================================
        // CÔNG CỤ
        // ==========================================

        private void Sidebar_ToolRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new ToolView();
        }


        // ==========================================
        // BÁO CÁO
        // ==========================================

        private void Sidebar_ReportRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new ReportView();
        }


        // ==========================================
        // ĐÍNH KÈM
        // ==========================================

        private void Sidebar_AttachmentRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new AttachmentView();
        }


        // ==========================================
        // DỮ LIỆU
        // ==========================================

        private void Sidebar_DataRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new DataView();
        }


        // ==========================================
        // KHÁCH HÀNG
        // ==========================================

        private void Sidebar_CustomerRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new CustomerView();
        }


        // ==========================================
        // BÁO GIÁ
        // ==========================================

        private void Sidebar_QuotationRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowQuotationView();
        }


        // ==========================================
        // HIỂN THỊ DANH SÁCH BÁO GIÁ
        // ==========================================

        private void ShowQuotationView()
        {
            QuotationView view =
                new QuotationView();


            // Khi bấm Xem
            view.DetailRequested +=
                Quotation_DetailRequested;


            // Khi bấm Sửa
            view.EditRequested +=
                Quotation_EditRequested;


            // Khi bấm + Tạo báo giá
            view.CreateRequested +=
                Quotation_CreateRequested;


            MainContent.Content =
                view;
        }


        // ==========================================
        // XEM CHI TIẾT BÁO GIÁ
        // ==========================================

        private void Quotation_DetailRequested(
            object sender,
            string quotationId)
        {
            QuotationDetailView detailView =
                new QuotationDetailView(
                    quotationId);


            // Khi bấm Quay lại
            detailView.BackRequested +=
                QuotationDetail_BackRequested;


            MainContent.Content =
                detailView;
        }


        // ==========================================
        // SỬA BÁO GIÁ
        // ==========================================

        private void Quotation_EditRequested(
            object sender,
            string quotationId)
        {
            QuotationEditView editView =
                new QuotationEditView(
                    quotationId);


            // Khi bấm Quay lại / Hủy
            editView.BackRequested +=
                QuotationEdit_BackRequested;


            MainContent.Content =
                editView;
        }


        // ==========================================
        // TẠO BÁO GIÁ
        // ==========================================

        private void Quotation_CreateRequested(
            object sender,
            RoutedEventArgs e)
        {
            QuotationCreateView createView =
                new QuotationCreateView();


            // Khi bấm Quay lại / Hủy
            createView.BackRequested +=
                QuotationCreate_BackRequested;


            MainContent.Content =
                createView;
        }


        // ==========================================
        // QUAY LẠI TỪ CHI TIẾT BÁO GIÁ
        // ==========================================

        private void QuotationDetail_BackRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowQuotationView();
        }


        // ==========================================
        // QUAY LẠI TỪ SỬA BÁO GIÁ
        // ==========================================

        private void QuotationEdit_BackRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowQuotationView();
        }


        // ==========================================
        // QUAY LẠI TỪ TẠO BÁO GIÁ
        // ==========================================

        private void QuotationCreate_BackRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowQuotationView();
        }


        // ==========================================
        // LỆNH SẢN XUẤT
        // ==========================================

        private void Sidebar_ProductionRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowProductionView();
        }


        // ==========================================
        // HIỂN THỊ DANH SÁCH LỆNH SẢN XUẤT
        // ==========================================

        private void ShowProductionView()
        {
            ProductionView view =
                new ProductionView();


            // Khi bấm Xem
            view.DetailRequested +=
                Production_DetailRequested;


            // Khi bấm + Tạo lệnh sản xuất
            view.CreateRequested +=
                Production_CreateRequested;


            MainContent.Content =
                view;
        }


        // ==========================================
        // XEM CHI TIẾT LỆNH SẢN XUẤT
        // ==========================================

        private void Production_DetailRequested(
            object sender,
            string productionId)
        {
            ProductionDetailView detailView =
                new ProductionDetailView(
                    productionId);


            // Khi bấm Quay lại
            detailView.BackRequested +=
                ProductionDetail_BackRequested;


            MainContent.Content =
                detailView;
        }


        // ==========================================
        // TẠO LỆNH SẢN XUẤT
        // ==========================================

        private void Production_CreateRequested(
            object sender,
            RoutedEventArgs e)
        {
            ProductionCreateView createView =
                new ProductionCreateView();


            // Khi bấm Quay lại / Hủy
            createView.BackRequested +=
                ProductionCreate_BackRequested;


            MainContent.Content =
                createView;
        }


        // ==========================================
        // QUAY LẠI TỪ CHI TIẾT LỆNH SẢN XUẤT
        // ==========================================

        private void ProductionDetail_BackRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowProductionView();
        }


        // ==========================================
        // QUAY LẠI TỪ TẠO LỆNH SẢN XUẤT
        // ==========================================

        private void ProductionCreate_BackRequested(
            object sender,
            RoutedEventArgs e)
        {
            ShowProductionView();
        }


        private void Sidebar_SettingsRequested(
    object sender,
    RoutedEventArgs e)
        {
            ShowSettingsView();
        }


        private void ShowSettingsView()
        {
            SettingsView view =
                new SettingsView();

            view.PricingSettingsRequested +=
                Settings_PricingSettingsRequested;

            MainContent.Content =
                view;
        }


        private void Settings_PricingSettingsRequested(
            object sender,
            RoutedEventArgs e)
        {
            MainContent.Content =
                new PricingSettingsView();
        }
    }
}