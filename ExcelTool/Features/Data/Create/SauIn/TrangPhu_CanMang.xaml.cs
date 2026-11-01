using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Data.Create
{
    public partial class TrangPhu_CanMang : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ProcessPriceSettingService _processPriceSettingService;


        // =========================================================
        // DỮ LIỆU
        // =========================================================

        private List<ProcessPriceSetting> _processPriceSettings =
            new List<ProcessPriceSetting>();


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public TrangPhu_CanMang()
        {
            InitializeComponent();

            _processPriceSettingService =
                new ProcessPriceSettingService();

            LoadProcessPriceSettings();
        }


        // =========================================================
        // LOAD DỮ LIỆU CÔNG ĐOẠN
        // =========================================================

        private void LoadProcessPriceSettings()
        {
            try
            {
                _processPriceSettings =
                    _processPriceSettingService.GetAll();


                // =================================================
                // 1. TRÁNG PHỦ
                // =================================================

                List<string> coatingStages =
                    _processPriceSettings
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.Process) &&
                            x.Process.Trim().Equals(
                                "Tráng phủ",
                                StringComparison.OrdinalIgnoreCase) &&
                            !string.IsNullOrWhiteSpace(x.Stage))
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Id)
                        .Select(x => x.Stage.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();


                CoatingTypeComboBox.ItemsSource =
                    coatingStages;

                CoatingType2ComboBox.ItemsSource =
                    coatingStages;

                CoatingType3ComboBox.ItemsSource =
                    coatingStages;

                CoatingType4ComboBox.ItemsSource =
                    coatingStages;


                // =================================================
                // 2. CÁN MÀNG
                // =================================================

                List<string> filmProcessStages =
                    _processPriceSettings
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(x.Process) &&
                            x.Process.Trim().Equals(
                                "Cán màng",
                                StringComparison.OrdinalIgnoreCase) &&
                            !string.IsNullOrWhiteSpace(x.Stage))
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Id)
                        .Select(x => x.Stage.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();


                FilmTypeComboBox.ItemsSource =
                    filmProcessStages;

                FilmTypeBComboBox.ItemsSource =
                    filmProcessStages;

                FilmTypeOtherComboBox.ItemsSource =
                    filmProcessStages;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu công đoạn gia công.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // TÌM ĐƠN GIÁ THEO PROCESS + STAGE
        // =========================================================

        private decimal? GetProcessUnitPrice(
            string process,
            string? stage)
        {
            if (string.IsNullOrWhiteSpace(stage))
            {
                return null;
            }


            ProcessPriceSetting? setting =
                _processPriceSettings
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.Process) &&
                        x.Process.Trim().Equals(
                            process,
                            StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrWhiteSpace(x.Stage) &&
                        x.Stage.Trim().Equals(
                            stage.Trim(),
                            StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .FirstOrDefault();


            if (setting == null)
            {
                return null;
            }


            return setting.UnitPrice;
        }


        // =========================================================
        // SET ĐƠN GIÁ
        // =========================================================

        private void SetPriceText(
            TextBox priceTextBox,
            decimal? unitPrice)
        {
            if (unitPrice.HasValue)
            {
                priceTextBox.Text =
                    unitPrice.Value.ToString("#,##0");
            }
            else
            {
                priceTextBox.Clear();
            }
        }


        // =========================================================
        // TRÁNG PHỦ - LẦN 1
        // =========================================================

        private void CoatingTypeComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                CoatingTypeComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Tráng phủ",
                    stage);

            SetPriceText(
                CoatingPriceTextBox,
                price);
        }


        // =========================================================
        // TRÁNG PHỦ - LẦN 2
        // =========================================================

        private void CoatingType2ComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                CoatingType2ComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Tráng phủ",
                    stage);

            SetPriceText(
                CoatingPrice2TextBox,
                price);
        }


        // =========================================================
        // TRÁNG PHỦ - LẦN 3
        // =========================================================

        private void CoatingType3ComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                CoatingType3ComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Tráng phủ",
                    stage);

            SetPriceText(
                CoatingPrice3TextBox,
                price);
        }


        // =========================================================
        // TRÁNG PHỦ - LẦN 4
        // =========================================================

        private void CoatingType4ComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                CoatingType4ComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Tráng phủ",
                    stage);

            SetPriceText(
                CoatingPrice4TextBox,
                price);
        }


        // =========================================================
        // MẶT A - CHỌN CÁN MÀNG
        // =========================================================

        private void FilmTypeComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                FilmTypeComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Cán màng",
                    stage);

            SetPriceText(
                FilmUnitPriceTextBox,
                price);
        }


        // =========================================================
        // MẶT B - CHỌN CÁN MÀNG
        // =========================================================

        private void FilmTypeBComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                FilmTypeBComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Cán màng",
                    stage);

            SetPriceText(
                FilmUnitPriceBTextBox,
                price);
        }


        // =========================================================
        // CÁN MÀNG KHÁC
        // =========================================================

        private void FilmTypeOtherComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            string? stage =
                FilmTypeOtherComboBox.SelectedItem?.ToString();

            decimal? price =
                GetProcessUnitPrice(
                    "Cán màng",
                    stage);

            SetPriceText(
                FilmUnitPriceOtherTextBox,
                price);
        }
    }
}