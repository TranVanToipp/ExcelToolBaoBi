using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;
using ExcelTool.Shared.Controls;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class SupportPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly SupportSettingService
            _supportSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly SupportSettingExcelImporter
            _supportSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public SupportPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _supportSettingService =
                new SupportSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _supportSettingExcelImporter =
                new SupportSettingExcelImporter();


            // =====================================================
            // EVENT - ADD
            // =====================================================

            SupportPricingGrid.AddRequested +=
                Support_AddRequested;


            // =====================================================
            // EVENT - IMPORT
            // =====================================================

            SupportPricingGrid.ImportRequested +=
                Support_ImportRequested;


            // =====================================================
            // EVENT - EDIT
            // =====================================================

            SupportPricingGrid.EditRequested +=
                Support_EditRequested;


            // =====================================================
            // EVENT - DELETE
            // =====================================================

            SupportPricingGrid.DeleteRequested +=
                Support_DeleteRequested;


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
                SupportPricingGrid.ItemsSource =
                    _supportSettingService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu Giá Support.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // ADD
        // =========================================================

        private void Support_AddRequested(
            object sender,
            RoutedEventArgs e)
        {
            // Sẽ xử lý form thêm ở bước tiếp theo.
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void Support_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import Giá Support",

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
                        "Dữ liệu Giá Support hiện tại " +
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
                    _supportSettingExcelImporter.Import(
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
                    "Import Giá Support thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // EDIT
        // =========================================================

        private void Support_EditRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not SupportSetting setting)
            {
                return;
            }


            // Xử lý form sửa ở bước tiếp theo.
        }


        // =========================================================
        // DELETE
        // =========================================================

        private void Support_DeleteRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            if (e.Item is not SupportSetting setting)
            {
                return;
            }


            MessageBoxResult confirm =
                MessageBox.Show(
                    $"Bạn có chắc muốn xóa \"{setting.Loai}\" không?",

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
                    _supportSettingService.Delete(
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
                    "Không thể xóa Giá Support.\n\n" +
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