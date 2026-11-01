using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Data.Create
{
    public partial class CreatePaperPrintView : UserControl
    {
        // =========================================================
        // SERVICES
        // =========================================================

        private readonly PaperPriceSettingService
            _paperPriceSettingService;

        private readonly PaperSizeSettingService
            _paperSizeSettingService;


        // =========================================================
        // DATA
        // =========================================================

        private List<PaperPriceSetting>
            _paperPrices =
                new List<PaperPriceSetting>();

        private List<PaperSizeSetting>
            _paperSizes =
                new List<PaperSizeSetting>();


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public CreatePaperPrintView()
        {
            InitializeComponent();

            // -----------------------------------------------------
            // SERVICE
            // -----------------------------------------------------

            _paperPriceSettingService =
                new PaperPriceSettingService();

            _paperSizeSettingService =
                new PaperSizeSettingService();


            // -----------------------------------------------------
            // LOAD DATA
            // -----------------------------------------------------

            LoadPaperPrices();

            LoadPaperSizes();
        }


        // =========================================================
        // LOAD GIÁ GIẤY
        // =========================================================

        private void LoadPaperPrices()
        {
            try
            {
                _paperPrices =
                    _paperPriceSettingService.GetAll();

                PaperComboBox.ItemsSource =
                    _paperPrices;

                // -------------------------------------------------
                // Hiển thị tên giấy
                // -------------------------------------------------

                PaperComboBox.DisplayMemberPath =
                    nameof(PaperPriceSetting.TenGiay);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách giấy.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LOAD KHỔ IN
        // =========================================================

        private void LoadPaperSizes()
        {
            try
            {
                _paperSizes =
                    _paperSizeSettingService.GetAll();

                PrintSizeComboBox.ItemsSource =
                    _paperSizes;

                // -------------------------------------------------
                // Hiển thị khổ tờ in
                // -------------------------------------------------

                PrintSizeComboBox.DisplayMemberPath =
                    nameof(PaperSizeSetting.PrintSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách khổ giấy - máy in - kẽm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // CHỌN GIẤY
        // =========================================================

        private void PaperComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (PaperComboBox.SelectedItem
                is not PaperPriceSetting paper)
            {
                PaperCodeTextBox.Text = "";
                PaperNameTextBox.Text = "";
                PaperPriceTextBox.Text = "";

                UpdateSummary();

                return;
            }


            // -----------------------------------------------------
            // MÃ GIẤY
            // -----------------------------------------------------

            PaperCodeTextBox.Text =
                paper.MaGiay ?? "";


            // -----------------------------------------------------
            // TÊN GIẤY
            // -----------------------------------------------------

            PaperNameTextBox.Text =
                paper.TenGiay ?? "";


            // -----------------------------------------------------
            // GIÁ GIẤY
            // -----------------------------------------------------

            PaperPriceTextBox.Text =
                paper.VndPerM2.ToString("N0");


            UpdateSummary();
        }


        // =========================================================
        // CHỌN KHỔ IN
        // =========================================================

        private void PrintSizeComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (PrintSizeComboBox.SelectedItem
                is not PaperSizeSetting setting)
            {
                PrinterSizeTextBox.Text = "";
                PlateSizeTextBox.Text = "";

                UpdateSummary();

                return;
            }


            // -----------------------------------------------------
            // MÁY IN KHỔ
            // -----------------------------------------------------

            PrinterSizeTextBox.Text =
                setting.PrinterSize ?? "";


            // -----------------------------------------------------
            // KHỔ KẼM
            // -----------------------------------------------------

            PlateSizeTextBox.Text =
                setting.PlateSize ?? "";


            UpdateSummary();
        }


        // =========================================================
        // CẬP NHẬT TÓM TẮT
        // =========================================================

        private void UpdateSummary()
        {
            string paperName =
                PaperNameTextBox.Text.Trim();

            string paperCode =
                PaperCodeTextBox.Text.Trim();

            string paperPrice =
                PaperPriceTextBox.Text.Trim();

            string printSize =
                PrintSizeComboBox.SelectedItem
                    is PaperSizeSetting size
                    ? size.PrintSize ?? ""
                    : "";

            string printerSize =
                PrinterSizeTextBox.Text.Trim();

            string plateSize =
                PlateSizeTextBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(paperName) &&
                string.IsNullOrWhiteSpace(printSize))
            {
                SummaryTextBlock.Text =
                    "Chưa chọn cấu hình giấy và in.";

                return;
            }


            SummaryTextBlock.Text =
                "Giấy: " +
                paperName +
                (string.IsNullOrWhiteSpace(paperCode)
                    ? ""
                    : $" ({paperCode})") +

                "\nGiá: " +
                (string.IsNullOrWhiteSpace(paperPrice)
                    ? "-"
                    : paperPrice + " VNĐ/m²") +

                "\nKhổ tờ in: " +
                (string.IsNullOrWhiteSpace(printSize)
                    ? "-"
                    : printSize) +

                "\nMáy in khổ: " +
                (string.IsNullOrWhiteSpace(printerSize)
                    ? "-"
                    : printerSize) +

                "\nKhổ kẽm: " +
                (string.IsNullOrWhiteSpace(plateSize)
                    ? "-"
                    : plateSize);
        }


        // =========================================================
        // LẤY MÃ GIẤY
        // =========================================================

        public string GetPaperCode()
        {
            return PaperCodeTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY TÊN GIẤY
        // =========================================================

        public string GetPaperName()
        {
            return PaperNameTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY GIÁ GIẤY
        // =========================================================

        public decimal GetPaperPrice()
        {
            if (PaperComboBox.SelectedItem
                is not PaperPriceSetting paper)
            {
                return 0;
            }

            return Convert.ToDecimal(
                paper.VndPerM2);
        }


        // =========================================================
        // LẤY KHỔ TỜ IN
        // =========================================================

        public string GetPrintSize()
        {
            if (PrintSizeComboBox.SelectedItem
                is not PaperSizeSetting setting)
            {
                return "";
            }

            return setting.PrintSize ?? "";
        }


        // =========================================================
        // LẤY MÁY IN KHỔ
        // =========================================================

        public string GetPrinterSize()
        {
            return PrinterSizeTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY KHỔ KẼM
        // =========================================================

        public string GetPlateSize()
        {
            return PlateSizeTextBox.Text.Trim();
        }


        // =========================================================
        // KIỂM TRA DỮ LIỆU
        // =========================================================

        public bool ValidateData()
        {
            if (PaperComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn loại giấy.",
                    "Thiếu dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                PaperComboBox.Focus();

                return false;
            }


            if (PrintSizeComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn khổ tờ in.",
                    "Thiếu dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                PrintSizeComboBox.Focus();

                return false;
            }


            return true;
        }
    }
}