using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Quotation
{
    public partial class QuotationView : UserControl
    {
        private readonly QuotationService _quotationService;

        private List<QuotationDisplayItem> _allQuotationRows =
            new List<QuotationDisplayItem>();


        // ==========================================
        // KHỞI TẠO
        // ==========================================

        public QuotationView()
        {
            InitializeComponent();

            _quotationService =
                new QuotationService();

            LoadQuotations();
        }


        // ==========================================
        // LOAD DANH SÁCH BÁO GIÁ
        // ==========================================

        private void LoadQuotations()
        {
            try
            {
                List<Quotation> quotations =
                    _quotationService.GetAll();

                _allQuotationRows =
                    new List<QuotationDisplayItem>();


                foreach (var quotation in quotations)
                {
                    List<QuotationItem> items =
                        _quotationService.GetItems(
                            quotation.Id);


                    // ==========================================
                    // GHÉP TÊN SẢN PHẨM
                    // ==========================================

                    string productNames;

                    if (items.Count == 0)
                    {
                        productNames = "—";
                    }
                    else
                    {
                        productNames =
                            string.Join(
                                ", ",
                                items
                                    .Select(x => x.ProductName)
                                    .Where(x =>
                                        !string.IsNullOrWhiteSpace(x)));
                    }


                    if (string.IsNullOrWhiteSpace(
                            productNames))
                    {
                        productNames = "—";
                    }


                    _allQuotationRows.Add(
                        new QuotationDisplayItem
                        {
                            Id =
                                quotation.Id,

                            CustomerId =
                                quotation.CustomerId,

                            CustomerName =
                                quotation.CustomerName,

                            ProductNames =
                                productNames,

                            CreatedDate =
                                quotation.CreatedDate,

                            TotalAmount =
                                quotation.TotalAmount,

                            Status =
                                quotation.Status
                        });
                }


                // ==========================================
                // LOAD FILTER
                // ==========================================

                LoadCustomerFilter();

                LoadStatusFilter();


                // ==========================================
                // HIỂN THỊ TOÀN BỘ
                // ==========================================

                QuotationList.ItemsSource =
                    _allQuotationRows;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách báo giá.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // LOAD FILTER KHÁCH HÀNG
        // ==========================================

        private void LoadCustomerFilter()
        {
            var customers =
                _allQuotationRows
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(
                            x.CustomerName))
                    .GroupBy(x =>
                        x.CustomerId)
                    .Select(x =>
                        new CustomerFilterItem
                        {
                            Id =
                                x.Key,

                            Name =
                                x.First().CustomerName
                        })
                    .OrderBy(x =>
                        x.Name)
                    .ToList();


            CustomerFilterComboBox.Items.Clear();


            CustomerFilterComboBox.Items.Add(
                new CustomerFilterItem
                {
                    Id = "",
                    Name = "Tất cả khách hàng"
                });


            foreach (var customer in customers)
            {
                CustomerFilterComboBox.Items.Add(
                    customer);
            }


            CustomerFilterComboBox.SelectedIndex =
                0;
        }


        // ==========================================
        // LOAD FILTER TRẠNG THÁI
        // ==========================================

        private void LoadStatusFilter()
        {
            StatusFilterComboBox.Items.Clear();


            StatusFilterComboBox.Items.Add(
                "Tất cả trạng thái");


            var statuses =
                _allQuotationRows
                    .Select(x =>
                        x.Status)
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x =>
                        x);


            foreach (string status in statuses)
            {
                StatusFilterComboBox.Items.Add(
                    status);
            }


            StatusFilterComboBox.SelectedIndex =
                0;
        }


        // ==========================================
        // MODEL HIỂN THỊ
        // ==========================================

        private class QuotationDisplayItem
        {
            public string Id { get; set; } = "";

            public string CustomerId { get; set; } = "";

            public string CustomerName { get; set; } = "";

            public string ProductNames { get; set; } = "";

            public DateTime CreatedDate { get; set; }

            public decimal TotalAmount { get; set; }

            public string Status { get; set; } = "";
        }


        // ==========================================
        // MODEL FILTER KHÁCH HÀNG
        // ==========================================

        private class CustomerFilterItem
        {
            public string Id { get; set; } = "";

            public string Name { get; set; } = "";


            public override string ToString()
            {
                return Name;
            }
        }


        // ==========================================
        // LỌC
        // ==========================================

        private void FilterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ApplyFilters();
        }


        // ==========================================
        // ÁP DỤNG FILTER
        // ==========================================

        private void ApplyFilters()
        {
            try
            {
                string keyword =
                    SearchTextBox.Text
                        .Trim()
                        .ToLower();


                string customerId = "";


                if (CustomerFilterComboBox.SelectedItem
                    is CustomerFilterItem customer)
                {
                    customerId =
                        customer.Id;
                }


                string status = "";


                if (StatusFilterComboBox.SelectedItem
                    is string selectedStatus)
                {
                    if (!selectedStatus.Equals(
                            "Tất cả trạng thái",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        status =
                            selectedStatus;
                    }
                }


                IEnumerable<QuotationDisplayItem>
                    filtered =
                        _allQuotationRows;


                // ==========================================
                // TÌM KIẾM
                // ==========================================

                if (!string.IsNullOrWhiteSpace(
                        keyword))
                {
                    filtered =
                        filtered.Where(
                            x =>
                                x.Id
                                    .ToLower()
                                    .Contains(keyword)

                                ||

                                x.CustomerName
                                    .ToLower()
                                    .Contains(keyword)

                                ||

                                x.ProductNames
                                    .ToLower()
                                    .Contains(keyword));
                }


                // ==========================================
                // KHÁCH HÀNG
                // ==========================================

                if (!string.IsNullOrWhiteSpace(
                        customerId))
                {
                    filtered =
                        filtered.Where(
                            x =>
                                x.CustomerId ==
                                customerId);
                }


                // ==========================================
                // TRẠNG THÁI
                // ==========================================

                if (!string.IsNullOrWhiteSpace(
                        status))
                {
                    filtered =
                        filtered.Where(
                            x =>
                                x.Status ==
                                status);
                }


                // ==========================================
                // HIỂN THỊ
                // ==========================================

                QuotationList.ItemsSource =
                    filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể thực hiện lọc.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // ĐẶT LẠI FILTER
        // ==========================================

        private void ResetFilterButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Text = "";


            if (CustomerFilterComboBox.Items.Count > 0)
            {
                CustomerFilterComboBox.SelectedIndex =
                    0;
            }


            if (StatusFilterComboBox.Items.Count > 0)
            {
                StatusFilterComboBox.SelectedIndex =
                    0;
            }


            QuotationList.ItemsSource =
                _allQuotationRows;
        }


        // ==========================================
        // XEM CHI TIẾT
        // ==========================================

        public event EventHandler<string> DetailRequested;


        private void QuotationViewButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            string quotationId =
                button.Tag?.ToString() ?? "";


            if (string.IsNullOrWhiteSpace(
                    quotationId))
            {
                MessageBox.Show(
                    "Không tìm thấy mã báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            DetailRequested?.Invoke(
                this,
                quotationId);
        }


        // ==========================================
        // SỬA BÁO GIÁ
        // ==========================================

        public event EventHandler<string> EditRequested;


        private void EditQuotationButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            string quotationId =
                button.Tag?.ToString() ?? "";


            if (string.IsNullOrWhiteSpace(
                    quotationId))
            {
                MessageBox.Show(
                    "Không tìm thấy mã báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            EditRequested?.Invoke(
                this,
                quotationId);
        }


        // ==========================================
        // XÓA BÁO GIÁ
        // ==========================================

        private void DeleteQuotationButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            string quotationId =
                button.Tag?.ToString() ?? "";


            if (string.IsNullOrWhiteSpace(
                    quotationId))
            {
                MessageBox.Show(
                    "Không tìm thấy mã báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            Quotation? quotation =
                _quotationService.GetById(
                    quotationId);


            if (quotation == null)
            {
                MessageBox.Show(
                    "Không tìm thấy báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                LoadQuotations();

                return;
            }


            MessageBoxResult result =
                MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa báo giá này không?\n\n" +

                    $"Mã báo giá: {quotation.Id}\n" +

                    $"Khách hàng: {quotation.CustomerName}\n" +

                    $"Tổng tiền: {quotation.TotalAmount:N0} VNĐ\n\n" +

                    "Các sản phẩm trong báo giá cũng sẽ bị xóa.",

                    "Xác nhận xóa báo giá",

                    MessageBoxButton.YesNo,

                    MessageBoxImage.Warning);


            if (result != MessageBoxResult.Yes)
                return;


            try
            {
                _quotationService.Delete(
                    quotationId);


                MessageBox.Show(
                    "Đã xóa báo giá thành công.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);


                // ==========================================
                // LOAD LẠI DANH SÁCH
                // ==========================================

                LoadQuotations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa báo giá.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // TẠO BÁO GIÁ
        // ==========================================

        public event RoutedEventHandler CreateRequested;


        private void CreateQuotationButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CreateRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}