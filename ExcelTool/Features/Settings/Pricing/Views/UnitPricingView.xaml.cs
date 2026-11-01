using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;
using ExcelTool.Shared.Controls;

namespace ExcelTool.Features.Settings.Pricing.Views
{
    public partial class UnitPricingView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly UnitSettingService
            _unitSettingService;


        // =========================================================
        // EXCEL IMPORTER
        // =========================================================

        private readonly UnitSettingExcelImporter
            _unitSettingExcelImporter;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public UnitPricingView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _unitSettingService =
                new UnitSettingService();


            // =====================================================
            // EXCEL IMPORTER
            // =====================================================

            _unitSettingExcelImporter =
                new UnitSettingExcelImporter();


            // =====================================================
            // EVENT - ADD
            // =====================================================

            UnitPricingGrid.AddRequested +=
                Unit_AddRequested;


            // =====================================================
            // EVENT - IMPORT
            // =====================================================

            UnitPricingGrid.ImportRequested +=
                Unit_ImportRequested;


            // =====================================================
            // EVENT - EDIT
            // =====================================================

            UnitPricingGrid.EditRequested +=
                Unit_EditRequested;


            // =====================================================
            // EVENT - DELETE
            // =====================================================

            UnitPricingGrid.DeleteRequested +=
                Unit_DeleteRequested;


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
                UnitPricingGrid.ItemsSource =
                    _unitSettingService.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu đơn vị tính.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // ADD
        // =========================================================

        private void Unit_AddRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                string? unit =
                    ShowUnitInputDialog(
                        "Thêm đơn vị tính",
                        "Nhập đơn vị tính:");

                if (string.IsNullOrWhiteSpace(unit))
                {
                    return;
                }


                UnitSetting setting =
                    new UnitSetting
                    {
                        Unit = unit.Trim(),
                        SortOrder =
                            _unitSettingService
                                .GetAll()
                                .Count + 1
                    };


                bool success =
                    _unitSettingService.Create(
                        setting);


                if (!success)
                {
                    MessageBox.Show(
                        "Không thể thêm đơn vị tính.",

                        "Lỗi",

                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                LoadData();


                MessageBox.Show(
                    "Đã thêm đơn vị tính thành công.",

                    "Thông báo",

                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể thêm đơn vị tính.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        private void Unit_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                var dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import đơn vị tính",

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


                string filePath =
                    dialog.FileName;


                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu Đơn vị tính hiện tại " +
                        "trong SQLite sẽ được thay thế bằng " +
                        "dữ liệu từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                int importedCount =
                    _unitSettingExcelImporter.Import(
                        filePath);


                LoadData();


                MessageBox.Show(
                    $"Import thành công {importedCount} dòng " +
                    "đơn vị tính.",

                    "Import thành công",

                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import dữ liệu đơn vị tính thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // EDIT
        // =========================================================

        private void Unit_EditRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            try
            {
                if (e.Item is not UnitSetting setting)
                {
                    return;
                }


                string? unit =
                    ShowUnitInputDialog(
                        "Sửa đơn vị tính",
                        "Nhập đơn vị tính:",
                        setting.Unit);


                if (string.IsNullOrWhiteSpace(unit))
                {
                    return;
                }


                setting.Unit =
                    unit.Trim();


                bool success =
                    _unitSettingService.Update(
                        setting);


                if (!success)
                {
                    MessageBox.Show(
                        "Không thể cập nhật đơn vị tính.",

                        "Lỗi",

                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                LoadData();


                MessageBox.Show(
                    "Đã cập nhật đơn vị tính.",

                    "Thông báo",

                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật đơn vị tính.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // DELETE
        // =========================================================

        private void Unit_DeleteRequested(
            object? sender,
            PricingDataGridItemEventArgs e)
        {
            try
            {
                if (e.Item is not UnitSetting setting)
                {
                    return;
                }


                MessageBoxResult confirm =
                    MessageBox.Show(
                        $"Bạn có chắc muốn xóa đơn vị tính \"{setting.Unit}\" không?",

                        "Xác nhận xóa",

                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                bool success =
                    _unitSettingService.Delete(
                        setting.Id);


                if (!success)
                {
                    MessageBox.Show(
                        "Không thể xóa đơn vị tính.",

                        "Lỗi",

                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                LoadData();


                MessageBox.Show(
                    "Đã xóa đơn vị tính.",

                    "Thông báo",

                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa đơn vị tính.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // INPUT DIALOG
        // =========================================================

        private string? ShowUnitInputDialog(
            string title,
            string message,
            string defaultValue = "")
        {
            Window window =
                new Window
                {
                    Title = title,

                    Width = 400,
                    Height = 190,

                    WindowStartupLocation =
                        WindowStartupLocation.CenterOwner,

                    ResizeMode =
                        ResizeMode.NoResize,

                    Owner =
                        Window.GetWindow(this)
                };


            Grid grid =
                new Grid
                {
                    Margin =
                        new Thickness(20)
                };


            grid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = GridLength.Auto
                });

            grid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = new GridLength(10)
                });

            grid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = GridLength.Auto
                });

            grid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = new GridLength(20)
                });

            grid.RowDefinitions.Add(
                new RowDefinition
                {
                    Height = GridLength.Auto
                });


            TextBlock label =
                new TextBlock
                {
                    Text = message,

                    FontWeight =
                        FontWeights.SemiBold
                };


            Grid.SetRow(label, 0);


            TextBox textBox =
                new TextBox
                {
                    Text = defaultValue,

                    Height = 36,

                    Padding =
                        new Thickness(8),

                    VerticalContentAlignment =
                        VerticalAlignment.Center
                };


            Grid.SetRow(textBox, 2);


            StackPanel buttons =
                new StackPanel
                {
                    Orientation =
                        Orientation.Horizontal,

                    HorizontalAlignment =
                        HorizontalAlignment.Right
                };


            Button cancelButton =
                new Button
                {
                    Content = "Hủy",

                    Width = 80,
                    Height = 35,

                    Margin =
                        new Thickness(0, 0, 10, 0)
                };


            Button okButton =
                new Button
                {
                    Content = "Lưu",

                    Width = 80,
                    Height = 35
                };


            string? result = null;


            cancelButton.Click +=
                (_, _) =>
                {
                    window.DialogResult = false;
                };


            okButton.Click +=
                (_, _) =>
                {
                    result =
                        textBox.Text.Trim();

                    window.DialogResult = true;
                };


            buttons.Children.Add(
                cancelButton);

            buttons.Children.Add(
                okButton);


            Grid.SetRow(buttons, 4);


            grid.Children.Add(label);
            grid.Children.Add(textBox);
            grid.Children.Add(buttons);


            window.Content =
                grid;


            textBox.Focus();

            textBox.SelectAll();


            bool? dialogResult =
                window.ShowDialog();


            if (dialogResult != true)
            {
                return null;
            }


            return result;
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