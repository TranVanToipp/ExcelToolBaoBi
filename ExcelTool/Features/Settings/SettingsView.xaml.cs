using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Settings
{
    public partial class SettingsView : UserControl
    {
        private readonly CompanySettingService _companySettingService;


        // =========================================================
        // EVENT
        // =========================================================

        // Khi người dùng bấm "Thiết lập tính giá"
        // SettingsView sẽ báo cho MainWindow biết
        public event RoutedEventHandler PricingSettingsRequested;


        // =========================================================
        // KHỞI TẠO
        // =========================================================

        public SettingsView()
        {
            InitializeComponent();

            _companySettingService =
                new CompanySettingService();

            LoadCompanySetting();
        }


        // =========================================================
        // MỞ THIẾT LẬP TÍNH GIÁ
        // =========================================================

        private void PricingSettingsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            PricingSettingsRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }


        // =========================================================
        // LOAD THÔNG TIN CÔNG TY
        // =========================================================

        private void LoadCompanySetting()
        {
            try
            {
                CompanySetting setting =
                    _companySettingService.Get();


                CompanyNameTextBox.Text =
                    setting.CompanyName;

                AddressTextBox.Text =
                    setting.Address;

                TaxCodeTextBox.Text =
                    setting.TaxCode;

                CompanyPhoneTextBox.Text =
                    setting.Phone;

                EmailTextBox.Text =
                    setting.Email;

                WebsiteTextBox.Text =
                    setting.Website;

                LogoPathTextBox.Text =
                    setting.LogoPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải thông tin công ty.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // CHỌN LOGO
        // =========================================================

        private void ChooseLogoButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog =
                    new OpenFileDialog
                    {
                        Title =
                            "Chọn logo công ty",

                        Filter =
                            "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All Files|*.*",

                        Multiselect =
                            false
                    };


                bool? result =
                    dialog.ShowDialog();


                if (result == true)
                {
                    LogoPathTextBox.Text =
                        dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể chọn logo.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LƯU CÀI ĐẶT
        // =========================================================

        private void SaveSettingsButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                // =====================================================
                // KIỂM TRA TÊN CÔNG TY
                // =====================================================

                if (string.IsNullOrWhiteSpace(
                        CompanyNameTextBox.Text))
                {
                    MessageBox.Show(
                        "Vui lòng nhập tên công ty.",

                        "Thiếu thông tin",

                        MessageBoxButton.OK,

                        MessageBoxImage.Warning);

                    CompanyNameTextBox.Focus();

                    return;
                }


                // =====================================================
                // TẠO OBJECT
                // =====================================================

                CompanySetting setting =
                    new CompanySetting
                    {
                        Id = 1,

                        CompanyName =
                            CompanyNameTextBox.Text.Trim(),

                        Address =
                            AddressTextBox.Text.Trim(),

                        TaxCode =
                            TaxCodeTextBox.Text.Trim(),

                        Phone =
                            CompanyPhoneTextBox.Text.Trim(),

                        Email =
                            EmailTextBox.Text.Trim(),

                        Website =
                            WebsiteTextBox.Text.Trim(),

                        LogoPath =
                            LogoPathTextBox.Text.Trim()
                    };


                // =====================================================
                // LƯU DATABASE
                // =====================================================

                _companySettingService.Save(
                    setting);


                MessageBox.Show(
                    "Đã lưu thông tin công ty thành công.",

                    "Thành công",

                    MessageBoxButton.OK,

                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu thông tin công ty.\n\n" +
                    ex.Message,

                    "Lỗi",

                    MessageBoxButton.OK,

                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // KHÔI PHỤC MẶC ĐỊNH
        // =========================================================

        private void RestoreDefaultButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show(
                    "Bạn có chắc muốn khôi phục thông tin công ty về mặc định không?\n\n" +
                    "Các thông tin công ty hiện tại trên màn hình sẽ được xóa.",

                    "Khôi phục mặc định",

                    MessageBoxButton.YesNo,

                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            CompanyNameTextBox.Text = "";

            AddressTextBox.Text = "";

            TaxCodeTextBox.Text = "";

            CompanyPhoneTextBox.Text = "";

            EmailTextBox.Text = "";

            WebsiteTextBox.Text = "";

            LogoPathTextBox.Text = "";
        }
    }
}