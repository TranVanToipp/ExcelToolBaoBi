using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelTool.Features.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng Nhập dữ liệu sẽ được triển khai ở bước tiếp theo.",
                "Nhập dữ liệu",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Chức năng Xuất dữ liệu sẽ được triển khai ở bước tiếp theo.",
                "Xuất dữ liệu",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Màn hình Lịch sử sẽ được triển khai ở bước tiếp theo.",
                "Lịch sử",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Màn hình Cài đặt sẽ được triển khai ở bước tiếp theo.",
                "Cài đặt",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }


}
