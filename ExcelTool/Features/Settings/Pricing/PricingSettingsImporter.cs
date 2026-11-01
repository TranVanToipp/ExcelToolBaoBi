using System;
using System.Windows;

namespace ExcelTool.Features.Settings
{
    public partial class PricingSettingsView
    {
        // =========================================================
        // DIFFICULTY - IMPORT
        // =========================================================

        private void Difficulty_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportDifficultyFromExcel();
        }


        private void ImportDifficultyFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import độ khó sản phẩm",

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


                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu độ khó sản phẩm hiện tại " +
                        "sẽ được thay thế bằng dữ liệu từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                int importedCount =
                    _difficultySettingExcelImporter.Import(
                        filePath);


                LoadDifficultyData();


                MessageBox.Show(
                    $"Import thành công {importedCount} dòng.",

                    "Import thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import độ khó sản phẩm thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // REPRINT - IMPORT
        // =========================================================

        private void Reprint_ImportRequested(
            object sender,
            RoutedEventArgs e)
        {
            ImportReprintFromExcel();
        }


        private void ImportReprintFromExcel()
        {
            try
            {
                var dialog =
                    new Microsoft.Win32.OpenFileDialog
                    {
                        Title =
                            "Chọn file Excel để import cài đặt tái bản",

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


                MessageBoxResult confirm =
                    MessageBox.Show(
                        "Dữ liệu cài đặt tái bản hiện tại " +
                        "sẽ được thay thế bằng dữ liệu từ file Excel.\n\n" +
                        "Bạn có muốn tiếp tục không?",

                        "Xác nhận Import",

                        MessageBoxButton.YesNo,

                        MessageBoxImage.Question);


                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }


                int importedCount =
                    _reprintSettingExcelImporter.Import(
                        filePath);


                LoadReprintData();


                MessageBox.Show(
                    $"Import thành công {importedCount} dòng.",

                    "Import thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Import cài đặt tái bản thất bại.\n\n" +
                    ex.Message,

                    "Lỗi Import",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }
    }
}