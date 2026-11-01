using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Models;
using ExcelTool.Services;

namespace ExcelTool.Features.Data
{
    public partial class DataDetailView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ProductService
            _productService;


        // =========================================================
        // PRODUCT ID
        // =========================================================

        private readonly string
            _productId;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public DataDetailView(
            string productId)
        {
            InitializeComponent();

            _productService =
                new ProductService();

            _productId =
                productId;

            LoadProduct();
        }


        // =========================================================
        // LOAD PRODUCT
        // =========================================================

        private void LoadProduct()
        {
            try
            {
                Product? product =
                    _productService.GetById(
                        _productId);

                if (product == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy sản phẩm.",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    GoBack();

                    return;
                }


                // =================================================
                // THÔNG TIN CHUNG
                // =================================================

                ProductCodeTextBlock.Text =
                    product.ProductCode;

                CreatedDateTextBlock.Text =
                    product.CreatedDate
                        .ToString("dd/MM/yyyy");

                CustomerTextBlock.Text =
                    product.CustomerId;

                ProductNameTextBlock.Text =
                    product.ProductName;

                ProductTypeTextBlock.Text =
                    product.ProductType;

                ReprintTypeTextBlock.Text =
                    product.ReprintType;

                SpecificationTextBlock.Text =
                    product.Specification;

                SpreadSizeTextBlock.Text =
                    product.SpreadSize;

                QuantityTextBlock.Text =
                    product.Quantity
                        .ToString("N0");

                DifficultyTextBlock.Text =
                    product.Difficulty
                        .ToString();

                QuotationDeadlineTextBlock.Text =
                    product.QuotationDeadline.HasValue
                        ? product.QuotationDeadline.Value
                            .ToString("dd/MM/yyyy")
                        : "";

                NoteTextBlock.Text =
                    product.Note;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải thông tin sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // QUAY LẠI
        // =========================================================

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            GoBack();
        }


        // =========================================================
        // GO BACK
        // =========================================================

        private void GoBack()
        {
            MainWindow? mainWindow =
                Window.GetWindow(this) as MainWindow;

            if (mainWindow == null)
            {
                return;
            }

            mainWindow.MainContent.Content =
                new DataView();
        }
    }
}