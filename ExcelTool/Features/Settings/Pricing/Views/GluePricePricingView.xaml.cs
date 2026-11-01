using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;
using ExcelTool.Shared.Controls;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class GluePricePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly GluePriceSettingService
            _gluePriceSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly GluePriceSettingExcelImporter
            _gluePriceSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public GluePricePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _gluePriceSettingService =
                new GluePriceSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _gluePriceSettingExcelImporter =
                new GluePriceSettingExcelImporter();


            // =====================================================
            // EVENT - ADD
            // =====================================================

            GluePricePricingGrid.AddRequested +=
                GluePrice_AddRequested;


            // =====================================================
            // EVENT - IMPORT
            // =====================================================

            GluePricePricingGrid.ImportRequested +=
                GluePrice_ImportRequested;


            // =====================================================
            // EVENT - EDIT
            // =====================================================

            GluePricePricingGrid.EditRequested +=
                GluePrice_EditRequested;


            // =====================================================
            // EVENT - DELETE
            // =====================================================

            GluePricePricingGrid.DeleteRequested +=
                GluePrice_DeleteRequested;


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
                GluePricePricingGrid.ItemsSource =
                    _gluePriceSettingService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu Giá keo dán.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // ADD
        // =========================================================

        private void GluePrice_AddRequested(
            object sender,
            RoutedEventArgs e)
        {
            // Form thêm sẽ xử lý ở bước CRUD tiếp theo.
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void GluePrice_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import Giá keo dán",

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
                        "Dữ liệu Giá keo dán hiện tại " +
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
                    _gluePriceSettingExcelImporter.Import(
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
                    "Import Giá keo dán thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // EDIT
        // =========================================================

        private void GluePrice_EditRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not GluePriceSetting setting)
            {
                return;
            }


            // Form sửa sẽ xử lý ở bước CRUD tiếp theo.
        }


        // =========================================================
        // DELETE
        // =========================================================

        private void GluePrice_DeleteRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not GluePriceSetting setting)
            {
                return;
            }


            MessageBoxResult confirm =
                MessageBox.Show(
                    $"Bạn có chắc muốn xóa \"{setting.LoaiKeo}\" không?",

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
                    _gluePriceSettingService.Delete(
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
                    "Không thể xóa Giá keo dán.\n\n" +
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