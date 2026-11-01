using System;
using System.Windows;

namespace ExcelTool.Features.Customer
{
    public partial class CustomerFormWindow : Window
    {
        private readonly CustomerService _customerService;

        public string ShortName { get; private set; } = "";
        public string FullName { get; private set; } = "";
        public string Address { get; private set; } = "";
        public string TaxCode { get; private set; } = "";
        public string Contact { get; private set; } = "";
        public string Email { get; private set; } = "";
        public string Phone { get; private set; } = "";


        public CustomerFormWindow()
        {
            InitializeComponent();

            _customerService =
                new CustomerService();

            ShortNameTextBox.Focus();
        }


        // ==========================================
        // LƯU
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
            // LẤY DỮ LIỆU
            // ==========================================

            ShortName =
                ShortNameTextBox.Text.Trim();

            FullName =
                FullNameTextBox.Text.Trim();

            Address =
                AddressTextBox.Text.Trim();

            TaxCode =
                TaxCodeTextBox.Text.Trim();

            Contact =
                ContactTextBox.Text.Trim();

            Email =
                EmailTextBox.Text.Trim();

            Phone =
                PhoneTextBox.Text.Trim();


            // ==========================================
            // TẠO CUSTOMER
            // ==========================================

            string id =
                _customerService.GetNextId();

            Customer customer = new Customer
            {
                Id = id,
                ShortName = ShortName,
                FullName = FullName,
                Address = Address,
                TaxCode = TaxCode,
                ContactPerson = Contact,
                Email = Email,
                Phone = Phone
            };


            // ==========================================
            // LƯU VÀO SQLITE
            // ==========================================

            try
            {
                _customerService.Add(customer);

                MessageBox.Show(
                    $"Đã thêm khách hàng thành công.\n\n" +
                    $"Mã khách hàng: {id}",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu khách hàng.\n\n" +
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