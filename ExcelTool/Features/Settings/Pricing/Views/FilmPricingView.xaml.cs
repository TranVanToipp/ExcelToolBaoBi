using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;
using ExcelTool.Shared.Controls;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class FilmPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly FilmSettingService
            _filmSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly FilmSettingExcelImporter
            _filmSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public FilmPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _filmSettingService =
                new FilmSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _filmSettingExcelImporter =
                new FilmSettingExcelImporter();


            // =====================================================
            // EVENT - ADD
            // =====================================================

            FilmPricingGrid.AddRequested +=
                Film_AddRequested;


            // =====================================================
            // EVENT - IMPORT
            // =====================================================

            FilmPricingGrid.ImportRequested +=
                Film_ImportRequested;


            // =====================================================
            // EVENT - EDIT
            // =====================================================

            FilmPricingGrid.EditRequested +=
                Film_EditRequested;


            // =====================================================
            // EVENT - DELETE
            // =====================================================

            FilmPricingGrid.DeleteRequested +=
                Film_DeleteRequested;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadData();
        }


        // =========================================================
        // LOAD DATA
        // =========================================================

        private void LoadData()
        {
            try
            {
                FilmPricingGrid.ItemsSource =
                    _filmSettingService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu Quy đổi trọng lượng màng.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // ADD
        // =========================================================

        private void Film_AddRequested(
            object sender,
            RoutedEventArgs e)
        {
            // Form thêm sẽ xử lý ở bước CRUD tiếp theo.
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void Film_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import Quy đổi trọng lượng màng",

                        Filter =
                            "Excel Files (*.xlsx;*.xlsm)|*.xlsx;*.xlsm",

                        Multiselect = false
                    };


                bool? result =
                    dialog.ShowDialog();


                if (result != true)
                {
                    return;
                }


                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu Quy đổi trọng lượng màng hiện tại " +
                        "sẽ được thay thế bằng dữ liệu " +
                        "từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                int importedCount =
                    _filmSettingExcelImporter.Import(
                        dialog.FileName);


                LoadData();


                MessageBox.Show(
                    $"Import thành công {importedCount} dòng.",

                    "Import thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import Quy đổi trọng lượng màng thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // EDIT
        // =========================================================

        private void Film_EditRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not FilmSetting setting)
            {
                return;
            }


            // Form sửa sẽ xử lý ở bước CRUD tiếp theo.
        }


        // =========================================================
        // DELETE
        // =========================================================

        private void Film_DeleteRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not FilmSetting setting)
            {
                return;
            }


            MessageBoxResult confirm =
                MessageBox.Show(
                    $"Bạn có chắc muốn xóa \"{setting.LoaiMang}\" không?",

                    "Xác nhận xóa",

                    MessageBoxButton.YesNo,

                    MessageBoxImage.Warning);


            if (confirm != MessageBoxResult.Yes)
            {
                return;
            }


            try
            {
                bool success =
                    _filmSettingService.Delete(
                        setting.Id);


                if (!success)
                {
                    MessageBox.Show(
                        "Không thể xóa dữ liệu.",

                        "Lỗi",

                        MessageBoxButton.OK,

                        MessageBoxImage.Error);

                    return;
                }


                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa dữ liệu Quy đổi trọng lượng màng.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // RELOAD
        // =========================================================

        public void Reload()
        {
            LoadData();
        }
    }
}