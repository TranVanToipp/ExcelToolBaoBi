using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Shared.Controls
{
    public partial class SidebarControl : UserControl
    {
        public SidebarControl()
        {
            InitializeComponent();
        }


        // ==========================================
        // DỮ LIỆU
        // ==========================================

        public event RoutedEventHandler DataRequested;

        private void Sidebar_Data_Click(
            object sender,
            RoutedEventArgs e)
        {
            DataRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }


        // ==========================================
        // KHÁCH HÀNG
        // ==========================================

        public event RoutedEventHandler CustomerRequested;

        private void Sidebar_Customer_Click(
            object sender,
            RoutedEventArgs e)
        {
            CustomerRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }


        // ==========================================
        // BÁO GIÁ
        // ==========================================

        public event RoutedEventHandler QuotationRequested;

        private void Sidebar_Quotation_Click(
            object sender,
            RoutedEventArgs e)
        {
            QuotationRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }


        // ==========================================
        // LỆNH SẢN XUẤT
        // ==========================================

        public event RoutedEventHandler ProductionRequested;

        private void Sidebar_Production_Click(
            object sender,
            RoutedEventArgs e)
        {
            ProductionRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }

        // ==========================================
        // ĐÍNH KÈM
        // ==========================================

        public event RoutedEventHandler AttachmentRequested;

        private void Sidebar_Attachment_Click(
            object sender,
            RoutedEventArgs e)
        {
            AttachmentRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }

        // ==========================================
        // BÁO CÁO
        // ==========================================

        public event RoutedEventHandler ReportRequested;

        private void Sidebar_Report_Click(
            object sender,
            RoutedEventArgs e)
        {
            ReportRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }

        // ==========================================
        // CÔNG CỤ
        // ==========================================

        public event RoutedEventHandler ToolRequested;

        private void Sidebar_Tool_Click(
            object sender,
            RoutedEventArgs e)
        {
            ToolRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }

        // ==========================================
        // CÀI ĐẶT
        // ==========================================

        public event RoutedEventHandler SettingsRequested;

        private void Sidebar_Settings_Click(
            object sender,
            RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }

        // ==========================================
        // GIỚI THIỆU
        // ==========================================

        public event RoutedEventHandler AboutRequested;

        private void Sidebar_About_Click(
            object sender,
            RoutedEventArgs e)
        {
            AboutRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}