using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Models;
using ExcelTool.Services;

namespace ExcelTool.Features.Data
{
    public partial class DataView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ProductService
            _productService;


        // =========================================================
        // DATA
        // =========================================================

        private List<Product>
            _products;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public DataView()
        {
            InitializeComponent();

            _productService =
                new ProductService();

            _products =
                new List<Product>();

            LoadData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadData()
        {
            try
            {
                _products =
                    _productService.GetAll();

                ProductItemsControl.ItemsSource =
                    _products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // XEM
        // =========================================================

        private void ViewData_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            if (button.Tag is not string productId)
            {
                return;
            }

            MainWindow? mainWindow =
                Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
            {
                return;
            }

            mainWindow.MainContent.Content =
                new DataDetailView(productId);
        }


        // =========================================================
        // TẠO DỮ LIỆU
        // =========================================================

        private void CreateData_Click(
            object sender,
            RoutedEventArgs e)
        {
            MainWindow? mainWindow =
                Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
            {
                return;
            }

            mainWindow.MainContent.Content =
                new DataCreateView();
        }


        // =========================================================
        // RELOAD
        // =========================================================

        public void Reload()
        {
            LoadData();
        }
    }
}