using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class PrintTypePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly PrintTypeSettingService
            _printTypeSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly PrintTypeSettingExcelImporter
            _printTypeSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PrintTypePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _printTypeSettingService =
                new PrintTypeSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _printTypeSettingExcelImporter =
                new PrintTypeSettingExcelImporter();


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadPrintTypeData();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            PrintTypePricingGrid.ImportRequested +=
                PrintType_ImportRequested;
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadPrintTypeData()
        {
            try
            {
                var data =
                    _printTypeSettingService.GetAll();


                PrintTypePricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu quy ước các kiểu in.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void PrintType_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportPrintTypeFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportPrintTypeFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import quy ước các kiểu in",

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
                        "Dữ liệu quy ước các kiểu in hiện tại " +
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
                    _printTypeSettingExcelImporter.Import(
                        filePath);


                // =================================================
                // LOAD LẠI
                // =================================================

                LoadPrintTypeData();


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
                    "Import quy ước các kiểu in thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}