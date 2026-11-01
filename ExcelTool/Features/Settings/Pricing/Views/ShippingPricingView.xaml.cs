using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class ShippingPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ShippingSettingService
            _shippingSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly ShippingSettingExcelImporter
            _shippingSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ShippingPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _shippingSettingService =
                new ShippingSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _shippingSettingExcelImporter =
                new ShippingSettingExcelImporter();


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadShippingData();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            ShippingPricingGrid.ImportRequested +=
                Shipping_ImportRequested;
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadShippingData()
        {
            try
            {
                var data =
                    _shippingSettingService.GetAll();


                ShippingPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu tai dán - khóa gài.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void Shipping_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportShippingFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportShippingFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import tai dán - khóa gài",

                        Filter =
                            "Excel Files (*.xlsx;*.xlsm)|*.xlsx;*.xlsm",

                        Multiselect = false
                    };


                if (dialog.ShowDialog() != true)
                {
                    return;
                }


                string filePath =
                    dialog.FileName;


                // =================================================
                // XÁC NHẬN
                // =================================================

                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu tai dán - khóa gài hiện tại " +
                        "sẽ được thay thế bằng dữ liệu từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                // =================================================
                // IMPORT
                // =================================================

                int importedCount =
                    _shippingSettingExcelImporter.Import(
                        filePath);


                // =================================================
                // LOAD LẠI
                // =================================================

                LoadShippingData();


                // =================================================
                // THÔNG BÁO
                // =================================================

                MessageBox.Show(
                    $"Import thành công {importedCount} dòng.",

                    "Import thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import tai dán - khóa gài thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}