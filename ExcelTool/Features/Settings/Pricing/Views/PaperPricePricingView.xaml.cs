using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class PaperPricePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly PaperPriceSettingService
            _paperPriceSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly PaperPriceSettingExcelImporter
            _paperPriceSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PaperPricePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _paperPriceSettingService =
                new PaperPriceSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _paperPriceSettingExcelImporter =
                new PaperPriceSettingExcelImporter();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            PaperPricePricingGrid.ImportRequested +=
                PaperPrice_ImportRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadPaperPriceData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadPaperPriceData()
        {
            try
            {
                var data =
                    _paperPriceSettingService.GetAll();


                PaperPricePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu giá giấy.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void PaperPrice_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportPaperPriceFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportPaperPriceFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import giá giấy",

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
                        "Dữ liệu giá giấy hiện tại " +
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
                    _paperPriceSettingExcelImporter.Import(
                        filePath);


                // -------------------------------------------------
                // LOAD LẠI GRID
                // -------------------------------------------------

                LoadPaperPriceData();


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
                    "Import dữ liệu giá giấy thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}