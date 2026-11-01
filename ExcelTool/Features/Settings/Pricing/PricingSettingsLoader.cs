using System;
using System.Windows;

namespace ExcelTool.Features.Settings
{
    public partial class PricingSettingsView
    {
        // =========================================================
        // LOAD - ĐỘ KHÓ SẢN PHẨM
        // =========================================================

        private void LoadDifficultyData()
        {
            try
            {
                var data =
                    _difficultySettingService.GetAll();

                DifficultyPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu độ khó sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LOAD - CÀI ĐẶT TÁI BẢN
        // =========================================================

        private void LoadReprintData()
        {
            try
            {
                var data =
                    _reprintSettingService.GetAll();

                ReprintPricingGrid.ItemsSource =
                    data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu cài đặt tái bản.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}