using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class PaperSizePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly PaperSizeSettingService
            _paperSizeSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly PaperSizeSettingExcelImporter
            _paperSizeSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperSizePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _paperSizeSettingService =
                new PaperSizeSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _paperSizeSettingExcelImporter =
                new PaperSizeSettingExcelImporter();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            PrintSizePricingGrid.ImportRequested +=
                PrintSize_ImportRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadPrintSizeData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadPrintSizeData()
        {
            try
            {
                var data =
                    _paperSizeSettingService.GetAll();


                PrintSizePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu khổ giấy - máy in - kẽm.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void PrintSize_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportPrintSizeFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportPrintSizeFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import khổ giấy - máy in - kẽm",

                        Filter =
                            "Excel Files (*.xlsx;*.xlsm)|*.xlsx;*.xlsm",

                        Multiselect = false
                    };


                // -------------------------------------------------
                // NGƯỜI DÙNG HỦY
                // -------------------------------------------------

                if (dialog.ShowDialog() != true)
                {
                    return;
                }


                string filePath =
                    dialog.FileName;


                // -------------------------------------------------
                // XÁC NHẬN
                // -------------------------------------------------

                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu khổ giấy - máy in - kẽm hiện tại " +
                        "sẽ được thay thế bằng dữ liệu từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                // -------------------------------------------------
                // IMPORT
                // -------------------------------------------------

                int importedCount =
                    _paperSizeSettingExcelImporter.Import(
                        filePath);


                // -------------------------------------------------
                // LOAD LẠI GRID
                // -------------------------------------------------

                LoadPrintSizeData();


                // -------------------------------------------------
                // THÔNG BÁO
                // -------------------------------------------------

                MessageBox.Show(
                    $"Import thành công {importedCount} dòng.",

                    "Import thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import dữ liệu khổ giấy - máy in - kẽm thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}