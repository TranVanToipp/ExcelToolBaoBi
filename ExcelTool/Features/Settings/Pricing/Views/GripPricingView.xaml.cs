using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class GripPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly GripSettingService
            _gripSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly GripSettingExcelImporter
            _gripSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public GripPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _gripSettingService =
                new GripSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _gripSettingExcelImporter =
                new GripSettingExcelImporter();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            GripPricingGrid.ImportRequested +=
                Grip_ImportRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadGripData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadGripData()
        {
            try
            {
                var data =
                    _gripSettingService.GetAll();


                GripPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu nhíp - tay kê - tràn màu.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void Grip_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportGripFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportGripFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import nhíp - tay kê - tràn màu",

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
                        "Dữ liệu nhíp - tay kê - tràn màu hiện tại " +
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
                    _gripSettingExcelImporter.Import(
                        filePath);


                // -------------------------------------------------
                // LOAD LẠI GRID
                // -------------------------------------------------

                LoadGripData();


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
                    "Import dữ liệu nhíp - tay kê - tràn màu thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}