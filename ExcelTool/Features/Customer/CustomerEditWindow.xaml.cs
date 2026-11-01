using System;
using System.Windows;

namespace ExcelTool.Features.Customer
{
    public partial class CustomerEditWindow : Window
    {
        private readonly CustomerService _customerService;

        private readonly Customer _customer;

        public CustomerEditWindow(Customer customer)
        {
            InitializeComponent();

            _customerService =
                new CustomerService();

            _customer = customer;

            LoadCustomer();

            ShortNameTextBox.Focus();
        }


        // ==========================================
        // LOAD DỮ LIỆU
        // ==========================================

        private void LoadCustomer()
        {
            ShortNameTextBox.Text =
                _customer.ShortName;

            FullNameTextBox.Text =
                _customer.FullName;

            AddressTextBox.Text =
                _customer.Address;

            TaxCodeTextBox.Text =
                _customer.TaxCode;

            ContactTextBox.Text =
                _customer.ContactPerson;

            EmailTextBox.Text =
                _customer.Email;

            PhoneTextBox.Text =
                _customer.Phone;
        }


        // ==========================================
        // LƯU THAY ĐỔI
        // ==========================================

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            // ==========================================
            // KIỂM TRA BẮT BUỘC
            // ==========================================

            if (string.IsNullOrWhiteSpace(
                ShortNameTextBox.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên viết tắt.",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                ShortNameTextBox.Focus();

                return;
            }


            if (string.IsNullOrWhiteSpace(
                FullNameTextBox.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên đầy đủ.",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                FullNameTextBox.Focus();

                return;
            }


            // ==========================================
            // CẬP NHẬT OBJECT
            // ==========================================

            _customer.ShortName =
                ShortNameTextBox.Text.Trim();

            _customer.FullName =
                FullNameTextBox.Text.Trim();

            _customer.Address =
                AddressTextBox.Text.Trim();

            _customer.TaxCode =
                TaxCodeTextBox.Text.Trim();

            _customer.ContactPerson =
                ContactTextBox.Text.Trim();

            _customer.Email =
                EmailTextBox.Text.Trim();

            _customer.Phone =
                PhoneTextBox.Text.Trim();


            // ==========================================
            // UPDATE SQLITE
            // ==========================================

            try
            {
                _customerService.Update(_customer);

                MessageBox.Show(
                    "Đã cập nhật khách hàng thành công.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật khách hàng.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // HỦY
        // ==========================================

        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}