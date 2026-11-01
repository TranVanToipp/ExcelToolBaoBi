using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class PrintBatchPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly PrintBatchSettingService
            _printBatchSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly PrintBatchSettingExcelImporter
            _printBatchSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PrintBatchPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _printBatchSettingService =
                new PrintBatchSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _printBatchSettingExcelImporter =
                new PrintBatchSettingExcelImporter();


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadPrintBatchData();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            PrintBatchPricingGrid.ImportRequested +=
                PrintBatch_ImportRequested;
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadPrintBatchData()
        {
            try
            {
                var data =
                    _printBatchSettingService.GetAll();


                PrintBatchPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu lô in - lô kẽm.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void PrintBatch_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportPrintBatchFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportPrintBatchFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import lô in - lô kẽm",

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
                        "Dữ liệu lô in - lô kẽm hiện tại " +
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
                    _printBatchSettingExcelImporter.Import(
                        filePath);


                // =================================================
                // LOAD LẠI
                // =================================================

                LoadPrintBatchData();


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
                    "Import lô in - lô kẽm thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}