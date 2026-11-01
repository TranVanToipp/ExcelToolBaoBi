using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class ProcessPricePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ProcessPriceSettingService
            _processPriceSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly ProcessPriceSettingExcelImporter
            _processPriceSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public ProcessPricePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _processPriceSettingService =
                new ProcessPriceSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _processPriceSettingExcelImporter =
                new ProcessPriceSettingExcelImporter();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            ProcessPricePricingGrid.ImportRequested +=
                ProcessPrice_ImportRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadProcessPriceData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadProcessPriceData()
        {
            try
            {
                var data =
                    _processPriceSettingService.GetAll();


                ProcessPricePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu danh mục công đoạn sản xuất và đơn giá.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void ProcessPrice_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportProcessPriceFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportProcessPriceFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import danh mục công đoạn sản xuất và đơn giá",

                        Filter =
                            "Excel Files (*.xlsx;*.xlsm)|*.xlsx;*.xlsm",

                        Multiselect = false
                    };


                if (dialog.ShowDialog() != true)
                {
                    return;
                }


                // =================================================
                // XÁC NHẬN
                // =================================================

                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu danh mục công đoạn sản xuất và đơn giá hiện tại " +
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
                    _processPriceSettingExcelImporter.Import(
                        dialog.FileName);


                // =================================================
                // LOAD LẠI
                // =================================================

                LoadProcessPriceData();


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
                    "Import danh mục công đoạn sản xuất và đơn giá thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}