using System;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Production
{
    public partial class ProductionView : UserControl
    {
        public ProductionView()
        {
            InitializeComponent();
        }


        // ==========================================
        // XEM CHI TIẾT
        // ==========================================

        public event EventHandler<string> DetailRequested;


        private void ProductionViewButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button button = (Button)sender;

            string productionId =
                button.Tag?.ToString() ?? "";

            DetailRequested?.Invoke(
                this,
                productionId);
        }


        // ==========================================
        // TẠO LỆNH SẢN XUẤT
        // ==========================================

        public event RoutedEventHandler CreateRequested;


        private void CreateProductionButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CreateRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}