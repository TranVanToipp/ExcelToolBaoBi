using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class PrintPricePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly PrintPriceSettingService
            _printPriceSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly PrintPriceSettingExcelImporter
            _printPriceSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PrintPricePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _printPriceSettingService =
                new PrintPriceSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _printPriceSettingExcelImporter =
                new PrintPriceSettingExcelImporter();


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadPrintPriceData();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            PrintPricePricingGrid.ImportRequested +=
                PrintPrice_ImportRequested;
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadPrintPriceData()
        {
            try
            {
                var data =
                    _printPriceSettingService.GetAll();


                PrintPricePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu đơn giá kẽm in và mực in.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void PrintPrice_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportPrintPriceFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportPrintPriceFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import đơn giá kẽm in và mực in",

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
                        "Dữ liệu đơn giá kẽm in và mực in hiện tại " +
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
                    _printPriceSettingExcelImporter.Import(
                        filePath);


                // =================================================
                // LOAD LẠI
                // =================================================

                LoadPrintPriceData();


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
                    "Import đơn giá kẽm in và mực in thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}