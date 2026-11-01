using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class BomPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly BomSettingService
            _bomSettingService;


        // =========================================================
        // IMPORTER
        // =========================================================

        private readonly BomSettingExcelImporter
            _bomSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public BomPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _bomSettingService =
                new BomSettingService();


            // =====================================================
            // IMPORTER
            // =====================================================

            _bomSettingExcelImporter =
                new BomSettingExcelImporter();


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadBomData();


            // =====================================================
            // IMPORT EVENT
            // =====================================================

            BomPricingGrid.ImportRequested +=
                Bom_ImportRequested;
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadBomData()
        {
            try
            {
                var data =
                    _bomSettingService.GetAll();


                BomPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu chủng loại sản phẩm (BOM).\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EVENT
        // =========================================================

        private void Bom_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportBomFromExcel();
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void ImportBomFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import chủng loại sản phẩm (BOM)",

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
                        "Dữ liệu chủng loại sản phẩm (BOM) hiện tại " +
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
                    _bomSettingExcelImporter.Import(
                        filePath);


                // =================================================
                // LOAD LẠI DỮ LIỆU
                // =================================================

                LoadBomData();


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
                    "Import chủng loại sản phẩm (BOM) thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}