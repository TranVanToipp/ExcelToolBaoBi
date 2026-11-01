using ExcelTool.Features.Customer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using CustomerModel =
    ExcelTool.Features.Customer.Customer;

namespace ExcelTool.Features.Quotation
{
    public partial class QuotationCreateView : UserControl
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly QuotationService _quotationService;

        private bool _isInitializing = true;

        private List<Product> _products =
            new List<Product>();

        private readonly List<ProductRowControls>
            _productRows =
                new List<ProductRowControls>();


        // =========================================================
        // QUẢN LÝ 1 DÒNG SẢN PHẨM
        // =========================================================

        private class ProductRowControls
        {
            public Border Container { get; set; } = null!;

            public ComboBox ProductComboBox { get; set; } = null!;

            public TextBox QuantityTextBox { get; set; } = null!;

            public TextBox UnitPriceTextBox { get; set; } = null!;

            public TextBlock AmountText { get; set; } = null!;
        }


        // =========================================================
        // KHỞI TẠO
        // =========================================================

        public QuotationCreateView()
        {
            InitializeComponent();


            _customerService =
                new CustomerService();


            _productService =
                new ProductService();


            _quotationService =
                new QuotationService();


            // ==========================================
            // NGÀY LẬP MẶC ĐỊNH
            // ==========================================

            CreatedDatePicker.SelectedDate =
                DateTime.Today;


            // ==========================================
            // LOAD KHÁCH HÀNG
            // ==========================================

            LoadCustomers();


            // ==========================================
            // LOAD SẢN PHẨM
            // ==========================================

            LoadProducts();


            // ==========================================
            // KẾT THÚC KHỞI TẠO
            // ==========================================

            _isInitializing = false;


            // ==========================================
            // TẠO DÒNG SẢN PHẨM ĐẦU TIÊN
            // ==========================================

            AddProductRow();
        }


        // =========================================================
        // LOAD KHÁCH HÀNG
        // =========================================================

        private void LoadCustomers()
        {
            try
            {
                List<CustomerModel> customers =
                    _customerService.GetAll();

                CustomerComboBox.ItemsSource =
                    customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách khách hàng.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LOAD SẢN PHẨM
        // =========================================================

        private void LoadProducts()
        {
            try
            {
                _products =
                    _productService.GetAll();
            }
            catch (Exception ex)
            {
                _products =
                    new List<Product>();

                MessageBox.Show(
                    "Không thể tải danh sách sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // CHỌN KHÁCH HÀNG
        // =========================================================

        private void CustomerComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (_isInitializing)
                return;


            if (CustomerComboBox.SelectedItem
                is not CustomerModel customer)
            {
                ContactTextBox.Text = "";
                PhoneTextBox.Text = "";

                return;
            }


            // Người liên hệ

            ContactTextBox.Text =
                customer.ContactPerson ?? "";


            // Điện thoại

            PhoneTextBox.Text =
                customer.Phone ?? "";
        }


        // =========================================================
        // LƯU BÁO GIÁ
        // =========================================================

        private void SaveQuotationButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                // ==========================================
                // KIỂM TRA KHÁCH HÀNG
                // ==========================================

                if (CustomerComboBox.SelectedItem
                    is not CustomerModel customer)
                {
                    MessageBox.Show(
                        "Vui lòng chọn khách hàng.",
                        "Thiếu thông tin",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    CustomerComboBox.Focus();

                    return;
                }


                // ==========================================
                // KIỂM TRA NGÀY
                // ==========================================

                DateTime createdDate =
                    CreatedDatePicker.SelectedDate
                    ?? DateTime.Today;


                // ==========================================
                // LẤY SẢN PHẨM
                // ==========================================

                var items =
                    new List<QuotationItem>();


                foreach (ProductRowControls row
                         in _productRows)
                {
                    // Bỏ qua dòng chưa chọn sản phẩm

                    if (row.ProductComboBox.SelectedItem
                        is not Product product)
                    {
                        continue;
                    }


                    // ======================================
                    // SỐ LƯỢNG
                    // ======================================

                    decimal quantity =
                        ParseNumber(
                            row.QuantityTextBox.Text);


                    if (quantity <= 0)
                    {
                        MessageBox.Show(
                            $"Số lượng của sản phẩm " +
                            $"'{product.ProductName}' " +
                            $"phải lớn hơn 0.",
                            "Số lượng không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        row.QuantityTextBox.Focus();

                        return;
                    }


                    // ======================================
                    // ĐƠN GIÁ
                    // ======================================

                    decimal unitPrice =
                        ParseMoney(
                            row.UnitPriceTextBox.Text);


                    if (unitPrice < 0)
                    {
                        MessageBox.Show(
                            $"Đơn giá của sản phẩm " +
                            $"'{product.ProductName}' " +
                            $"không hợp lệ.",
                            "Đơn giá không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        row.UnitPriceTextBox.Focus();

                        return;
                    }


                    // ======================================
                    // KIỂM TRA TRÙNG SẢN PHẨM
                    // ======================================

                    bool duplicated =
                        items.Any(
                            x =>
                                x.ProductId ==
                                product.Id);


                    if (duplicated)
                    {
                        MessageBox.Show(
                            $"Sản phẩm " +
                            $"'{product.ProductName}' " +
                            $"đã được chọn.",
                            "Trùng sản phẩm",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }


                    // ======================================
                    // TẠO ITEM
                    // ======================================

                    QuotationItem item =
                        new QuotationItem
                        {
                            ProductId =
                                product.Id,

                            ProductName =
                                product.ProductName,

                            Quantity =
                                quantity,

                            UnitPrice =
                                unitPrice
                        };


                    items.Add(item);
                }


                // ==========================================
                // KIỂM TRA CÓ SẢN PHẨM
                // ==========================================

                if (items.Count == 0)
                {
                    MessageBox.Show(
                        "Vui lòng chọn ít nhất một sản phẩm.",
                        "Thiếu sản phẩm",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ==========================================
                // TÍNH TẠM TÍNH
                // ==========================================

                decimal subTotal =
                    items.Sum(
                        x =>
                            x.Quantity *
                            x.UnitPrice);


                // ==========================================
                // CHIẾT KHẤU
                // ==========================================

                decimal discount = 0;


                // ==========================================
                // TỔNG TIỀN
                // ==========================================

                decimal totalAmount =
                    subTotal -
                    discount;


                if (totalAmount < 0)
                    totalAmount = 0;


                // ==========================================
                // TẠO BÁO GIÁ
                // ==========================================

                Quotation quotation =
                    new Quotation
                    {
                        CustomerId =
                            customer.Id,

                        CustomerName =
                            customer.FullName,

                        CreatedDate =
                            createdDate,

                        ContactPerson =
                            ContactTextBox.Text.Trim(),

                        Phone =
                            PhoneTextBox.Text.Trim(),

                        SubTotal =
                            subTotal,

                        Discount =
                            discount,

                        TotalAmount =
                            totalAmount,

                        Status =
                            "Nháp",

                        Note =
                            NoteTextBox.Text.Trim()
                    };


                // ==========================================
                // LƯU DATABASE
                // ==========================================

                string quotationId =
                    _quotationService.Save(
                        quotation,
                        items);


                // ==========================================
                // THÔNG BÁO
                // ==========================================

                MessageBox.Show(
                    "Đã lưu báo giá thành công.\n\n" +
                    $"Mã báo giá: {quotationId}\n" +
                    $"Khách hàng: {customer.FullName}\n" +
                    $"Ngày lập: {createdDate:dd/MM/yyyy}\n" +
                    $"Tổng cộng: {totalAmount:N0} VNĐ",
                    "Lưu báo giá",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);


                // ==========================================
                // QUAY VỀ DANH SÁCH
                // ==========================================

                BackRequested?.Invoke(
                    this,
                    new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu báo giá.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // THÊM SẢN PHẨM
        // =========================================================

        private void AddProductButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AddProductRow();
        }


        // =========================================================
        // TẠO DÒNG SẢN PHẨM
        // =========================================================

        private void AddProductRow()
        {
            // ==========================================
            // BORDER
            // ==========================================

            Border rowBorder =
                new Border
                {
                    BorderBrush =
                        new SolidColorBrush(
                            Color.FromRgb(
                                235,
                                238,
                                243)),

                    BorderThickness =
                        new Thickness(
                            0,
                            0,
                            0,
                            1),

                    Padding =
                        new Thickness(
                            10)
                };


            // ==========================================
            // GRID
            // ==========================================

            Grid grid =
                new Grid();


            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(
                            1,
                            GridUnitType.Star)
                });


            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(120)
                });


            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(150)
                });


            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(150)
                });


            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(70)
                });


            // ==========================================
            // COMBOBOX SẢN PHẨM
            // ==========================================

            ComboBox productComboBox =
                new ComboBox
                {
                    Height = 36,

                    Margin =
                        new Thickness(
                            0,
                            0,
                            15,
                            0),

                    VerticalContentAlignment =
                        VerticalAlignment.Center,

                    DisplayMemberPath =
                        "ProductName",

                    ItemsSource =
                        _products
                };


            Grid.SetColumn(
                productComboBox,
                0);


            // ==========================================
            // SỐ LƯỢNG
            // ==========================================

            TextBox quantityTextBox =
                new TextBox
                {
                    Height = 36,

                    Margin =
                        new Thickness(
                            0,
                            0,
                            15,
                            0),

                    Text = "1",

                    VerticalContentAlignment =
                        VerticalAlignment.Center
                };


            Grid.SetColumn(
                quantityTextBox,
                1);


            // ==========================================
            // ĐƠN GIÁ
            // ==========================================

            TextBox unitPriceTextBox =
                new TextBox
                {
                    Height = 36,

                    Margin =
                        new Thickness(
                            0,
                            0,
                            15,
                            0),

                    Text = "0",

                    IsReadOnly = false,

                    Background =
                        Brushes.White,

                    VerticalContentAlignment =
                        VerticalAlignment.Center
                };


            Grid.SetColumn(
                unitPriceTextBox,
                2);


            // ==========================================
            // THÀNH TIỀN
            // ==========================================

            TextBlock amountText =
                new TextBlock
                {
                    Text =
                        "0 VNĐ",

                    FontWeight =
                        FontWeights.SemiBold,

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(
                                37,
                                48,
                                68)),

                    VerticalAlignment =
                        VerticalAlignment.Center,

                    Margin =
                        new Thickness(
                            0,
                            0,
                            15,
                            0)
                };


            Grid.SetColumn(
                amountText,
                3);


            // ==========================================
            // NÚT XÓA
            // ==========================================

            Button deleteButton =
                new Button
                {
                    Content =
                        "Xóa",

                    Width = 55,

                    Height = 34,

                    Background =
                        Brushes.White,

                    BorderBrush =
                        new SolidColorBrush(
                            Color.FromRgb(
                                239,
                                83,
                                80)),

                    BorderThickness =
                        new Thickness(1),

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(
                                220,
                                53,
                                69)),

                    Cursor =
                        System.Windows.Input.Cursors.Hand
                };


            Grid.SetColumn(
                deleteButton,
                4);


            // ==========================================
            // ADD CONTROL
            // ==========================================

            grid.Children.Add(
                productComboBox);

            grid.Children.Add(
                quantityTextBox);

            grid.Children.Add(
                unitPriceTextBox);

            grid.Children.Add(
                amountText);

            grid.Children.Add(
                deleteButton);


            rowBorder.Child =
                grid;


            // ==========================================
            // OBJECT QUẢN LÝ ROW
            // ==========================================

            ProductRowControls row =
                new ProductRowControls
                {
                    Container =
                        rowBorder,

                    ProductComboBox =
                        productComboBox,

                    QuantityTextBox =
                        quantityTextBox,

                    UnitPriceTextBox =
                        unitPriceTextBox,

                    AmountText =
                        amountText
                };


            _productRows.Add(
                row);


            // ==========================================
            // ADD UI
            // ==========================================

            ProductRowsPanel.Children.Add(
                rowBorder);


            // ==========================================
            // EVENT CHỌN SẢN PHẨM
            // ==========================================

            productComboBox.SelectionChanged +=
                (sender, e) =>
                {
                    ProductComboBox_SelectionChanged(
                        row);
                };


            // ==========================================
            // EVENT SỐ LƯỢNG
            // ==========================================

            quantityTextBox.TextChanged +=
                (sender, e) =>
                {
                    CalculateRowAmount(row);
                };


            // ==========================================
            // EVENT ĐƠN GIÁ
            // ==========================================

            unitPriceTextBox.TextChanged +=
                (sender, e) =>
                {
                    CalculateRowAmount(row);
                };


            // ==========================================
            // EVENT XÓA
            // ==========================================

            deleteButton.Click +=
                (sender, e) =>
                {
                    DeleteProductRow(row);
                };
        }


        // =========================================================
        // CHỌN SẢN PHẨM
        // =========================================================

        private void ProductComboBox_SelectionChanged(
            ProductRowControls row)
        {
            if (_isInitializing)
                return;


            if (row.ProductComboBox.SelectedItem
                is not Product product)
            {
                row.UnitPriceTextBox.Text =
                    "0";

                row.AmountText.Text =
                    "0 VNĐ";

                CalculateTotal();

                return;
            }


            // ==========================================
            // KIỂM TRA TRÙNG
            // ==========================================

            bool duplicated =
                _productRows.Any(
                    x =>
                        x != row &&
                        x.ProductComboBox.SelectedItem
                        is Product selectedProduct &&
                        selectedProduct.Id ==
                        product.Id);


            if (duplicated)
            {
                MessageBox.Show(
                    "Sản phẩm này đã được chọn.\n\n" +
                    "Vui lòng chọn sản phẩm khác.",
                    "Trùng sản phẩm",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);


                row.ProductComboBox.SelectedItem =
                    null;


                row.UnitPriceTextBox.Text =
                    "0";


                row.AmountText.Text =
                    "0 VNĐ";


                CalculateTotal();

                return;
            }


            // ==========================================
            // LẤY ĐƠN GIÁ
            // ==========================================

            row.UnitPriceTextBox.Text =
                product.UnitPrice.ToString(
                    "N0");


            // ==========================================
            // TÍNH TIỀN
            // ==========================================

            CalculateRowAmount(row);
        }


        // =========================================================
        // XÓA DÒNG
        // =========================================================

        private void DeleteProductRow(
            ProductRowControls row)
        {
            // Không cho xóa dòng cuối

            if (_productRows.Count <= 1)
            {
                row.ProductComboBox.SelectedItem =
                    null;

                row.QuantityTextBox.Text =
                    "1";

                row.UnitPriceTextBox.Text =
                    "0";

                row.AmountText.Text =
                    "0 VNĐ";

                CalculateTotal();

                return;
            }


            _productRows.Remove(
                row);


            ProductRowsPanel.Children.Remove(
                row.Container);


            CalculateTotal();
        }


        // =========================================================
        // TÍNH THÀNH TIỀN
        // =========================================================

        private void CalculateRowAmount(
            ProductRowControls row)
        {
            if (row == null)
                return;


            decimal quantity =
                ParseNumber(
                    row.QuantityTextBox.Text);


            decimal unitPrice =
                ParseMoney(
                    row.UnitPriceTextBox.Text);


            if (quantity < 0)
                quantity = 0;


            if (unitPrice < 0)
                unitPrice = 0;


            decimal amount =
                quantity *
                unitPrice;


            row.AmountText.Text =
                amount.ToString("N0")
                + " VNĐ";


            CalculateTotal();
        }


        // =========================================================
        // TÍNH TỔNG
        // =========================================================

        private void CalculateTotal()
        {
            decimal subTotal = 0;


            foreach (
                ProductRowControls row
                in _productRows)
            {
                if (row.ProductComboBox.SelectedItem
                    is not Product)
                {
                    continue;
                }


                decimal quantity =
                    ParseNumber(
                        row.QuantityTextBox.Text);


                decimal unitPrice =
                    ParseMoney(
                        row.UnitPriceTextBox.Text);


                if (quantity < 0)
                    quantity = 0;


                if (unitPrice < 0)
                    unitPrice = 0;


                subTotal +=
                    quantity *
                    unitPrice;
            }


            // ==========================================
            // TẠM TÍNH
            // ==========================================

            SubTotalText.Text =
                subTotal.ToString("N0")
                + " VNĐ";


            // ==========================================
            // CHIẾT KHẤU
            // ==========================================

            decimal discount = 0;


            // ==========================================
            // TỔNG
            // ==========================================

            decimal total =
                subTotal -
                discount;


            if (total < 0)
                total = 0;


            TotalAmountText.Text =
                total.ToString("N0")
                + " VNĐ";
        }


        // =========================================================
        // PARSE SỐ LƯỢNG
        // =========================================================

        private decimal ParseNumber(
            string? text)
        {
            if (string.IsNullOrWhiteSpace(
                    text))
            {
                return 0;
            }


            if (decimal.TryParse(
                    text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out decimal result))
            {
                return result;
            }


            if (decimal.TryParse(
                    text.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out result))
            {
                return result;
            }


            return 0;
        }


        // =========================================================
        // PARSE TIỀN
        // =========================================================

        private decimal ParseMoney(
            string? text)
        {
            if (string.IsNullOrWhiteSpace(
                    text))
            {
                return 0;
            }


            string value =
                text.Trim()
                    .Replace(
                        "VNĐ",
                        "",
                        StringComparison
                            .OrdinalIgnoreCase)
                    .Replace(
                        "VND",
                        "",
                        StringComparison
                            .OrdinalIgnoreCase)
                    .Replace(
                        " ",
                        "")
                    .Replace(
                        ",",
                        "")
                    .Replace(
                        ".",
                        "");


            if (decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                return result;
            }


            return 0;
        }


        // =========================================================
        // QUAY LẠI
        // =========================================================

        public event RoutedEventHandler BackRequested;


        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            BackRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}