using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class WastePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly WasteSettingService
            _wasteSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly WasteSettingExcelImporter
            _wasteSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public WastePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _wasteSettingService =
                new WasteSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _wasteSettingExcelImporter =
                new WasteSettingExcelImporter();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            WastePricingGrid.ImportRequested +=
                Waste_ImportRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadWasteData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadWasteData()
        {
            try
            {
                var data =
                    _wasteSettingService.GetAll();


                WastePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu định mức bù hao.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void Waste_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportWasteFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportWasteFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import định mức bù hao",

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
                        "Dữ liệu định mức bù hao hiện tại " +
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
                    _wasteSettingExcelImporter.Import(
                        filePath);


                // -------------------------------------------------
                // LOAD LẠI GRID
                // -------------------------------------------------

                LoadWasteData();


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
                    "Import dữ liệu định mức bù hao thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}