using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class MoldPricePricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly MoldPriceSettingService
            _moldPriceSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly MoldPriceSettingExcelImporter
            _moldPriceSettingExcelImporter;


        // =========================================================
        // IMPORT REQUESTED
        // =========================================================

        public event RoutedEventHandler?
            ImportRequested;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public MoldPricePricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _moldPriceSettingService =
                new MoldPriceSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _moldPriceSettingExcelImporter =
                new MoldPriceSettingExcelImporter();


            // =====================================================
            // EVENT
            // =====================================================

            MoldPricePricingGrid.ImportRequested +=
                MoldPricePricingGrid_ImportRequested;


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
            MoldPricePricingGrid.ItemsSource =
                _moldPriceSettingService.GetAll();
        }


        // =========================================================
        // IMPORT BUTTON
        // =========================================================

        private void MoldPricePricingGrid_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                // =================================================
                // CHỌN FILE EXCEL
                // =================================================

                var dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel Giá khuôn",

                        Filter =
                            "Excel Files (*.xlsx;*.xlsm)|*.xlsx;*.xlsm|All Files (*.*)|*.*",

                        Multiselect = false
                    };


                if (dialog.ShowDialog() != true)
                {
                    return;
                }


                // =================================================
                // IMPORT
                // =================================================

                int count =
                    _moldPriceSettingExcelImporter.Import(
                        dialog.FileName);


                // =================================================
                // LOAD LẠI DỮ LIỆU
                // =================================================

                LoadData();


                // =================================================
                // THÔNG BÁO
                // =================================================

                MessageBox.Show(
                    $"Đã import {count} dòng Giá khuôn.",
                    "Import thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);


                // =================================================
                // THÔNG BÁO CHO VIEW CHA
                // =================================================

                ImportRequested?.Invoke(
                    this,
                    e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import Giá khuôn thất bại.\n\n" +
                    ex.Message,
                    "Lỗi Import",
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