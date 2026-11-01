using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ExcelTool.Features.Customer
{
    public partial class CustomerView : UserControl
    {
        private readonly CustomerService _customerService;

        public CustomerView()
        {
            InitializeComponent();

            _customerService = new CustomerService();

            LoadCustomers();
        }


        // ==========================================
        // LOAD KHÁCH HÀNG
        // ==========================================

        private void LoadCustomers()
        {
            List<Customer> customers =
                _customerService.GetAll();

            CustomerList.ItemsSource = customers;
        }


        // ==========================================
        // THÊM KHÁCH HÀNG
        // ==========================================

        private void AddCustomer_Click(
            object sender,
            RoutedEventArgs e)
        {
            CustomerFormWindow form =
                new CustomerFormWindow();

            form.Owner = Window.GetWindow(this);

            bool? result =
                form.ShowDialog();

            if (result == true)
            {
                LoadCustomers();
            }
        }


        // ==========================================
        // XEM CHI TIẾT
        // ==========================================

        private void CustomerDetail_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not Customer customer)
                return;

            CustomerDetailWindow detailWindow =
                new CustomerDetailWindow(
                    customer.Id,
                    customer.ShortName,
                    customer.FullName,
                    customer.Address,
                    customer.TaxCode,
                    customer.ContactPerson,
                    customer.Email,
                    customer.Phone);

            detailWindow.Owner =
                Window.GetWindow(this);

            bool? result = detailWindow.ShowDialog();

            if (result == true)
            {
                LoadCustomers();
            }
        }

        // ==========================================
        // TÌM KIẾM KHÁCH HÀNG
        // ==========================================

        private void SearchCustomer_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string keyword =
                SearchCustomer.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadCustomers();
                return;
            }

            List<Customer> customers =
                _customerService.Search(keyword);

            CustomerList.ItemsSource = customers;
        }

        // ==========================================
        // XÓA KHÁCH HÀNG
        // ==========================================

        private void DeleteCustomer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not Customer customer)
                return;


            // ==========================================
            // XÁC NHẬN
            // ==========================================

            MessageBoxResult result =
                MessageBox.Show(
                    $"Bạn có chắc muốn xóa khách hàng này không?\n\n" +
                    $"ID: {customer.Id}\n" +
                    $"Tên viết tắt: {customer.ShortName}\n" +
                    $"Tên đầy đủ: {customer.FullName}",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;


            // ==========================================
            // XÓA DATABASE
            // ==========================================

            try
            {
                _customerService.Delete(customer.Id);

                MessageBox.Show(
                    "Đã xóa khách hàng thành công.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa khách hàng.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}