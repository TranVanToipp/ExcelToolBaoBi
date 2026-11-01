using ExcelTool.Features.Customer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using CustomerModel =
    ExcelTool.Features.Customer.Customer;

namespace ExcelTool.Features.Quotation
{
    public partial class QuotationEditView : UserControl
    {
        private readonly string _quotationId;

        private readonly QuotationService _quotationService;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;

        private readonly List<ProductRowControls>
            _productRows =
                new List<ProductRowControls>();

        private bool _isInitializing = true;


        // ==========================================
        // KHỞI TẠO
        // ==========================================

        public QuotationEditView(
            string quotationId)
        {
            InitializeComponent();

            _quotationId =
                quotationId;

            _quotationService =
                new QuotationService();

            _customerService =
                new CustomerService();

            _productService =
                new ProductService();

            // Load danh sách khách hàng trước
            LoadCustomers();

            // Sau đó load báo giá cũ
            LoadQuotation();

            // Hoàn tất khởi tạo
            _isInitializing = false;

            // Tính lại tổng tiền
            CalculateTotal();
        }


        // ==========================================
        // LOAD KHÁCH HÀNG
        // ==========================================

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


        // ==========================================
        // LOAD BÁO GIÁ
        // ==========================================

        private void LoadQuotation()
        {
            if (string.IsNullOrWhiteSpace(
                    _quotationId))
            {
                MessageBox.Show(
                    "Không tìm thấy mã báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            // ==========================================
            // LẤY BÁO GIÁ
            // ==========================================

            Quotation? quotation =
                _quotationService.GetById(
                    _quotationId);


            if (quotation == null)
            {
                MessageBox.Show(
                    "Không tìm thấy báo giá.",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }


            // ==========================================
            // THÔNG TIN CƠ BẢN
            // ==========================================

            QuotationIdText.Text =
                quotation.Id;


            CreatedDateTextBox.Text =
                quotation.CreatedDate
                    .ToString("dd/MM/yyyy");


            StatusTextBox.Text =
                quotation.Status;


            // ==========================================
            // NGƯỜI LIÊN HỆ
            // ==========================================

            ContactTextBox.Text =
                quotation.ContactPerson ?? "";


            // ==========================================
            // SỐ ĐIỆN THOẠI
            // ==========================================

            PhoneTextBox.Text =
                quotation.Phone ?? "";


            // ==========================================
            // GHI CHÚ
            // ==========================================

            NoteTextBox.Text =
                quotation.Note ?? "";


            // ==========================================
            // LOAD KHÁCH HÀNG CŨ
            // ==========================================

            CustomerModel? customer =
                CustomerComboBox.Items
                    .Cast<CustomerModel>()
                    .FirstOrDefault(
                        x =>
                            x.Id ==
                            quotation.CustomerId);


            CustomerComboBox.SelectedItem =
                customer;


            // QUAN TRỌNG:
            //
            // Không lấy Phone và ContactPerson
            // từ Customer ở đây.
            //
            // Vì màn hình Sửa phải giữ lại
            // dữ liệu đã lưu trong báo giá.
            //
            // Do đó KHÔNG làm:
            //
            // ContactTextBox.Text =
            //     customer.ContactPerson;
            //
            // PhoneTextBox.Text =
            //     customer.Phone;


            // ==========================================
            // CHIẾT KHẤU
            // ==========================================

            DiscountTextBox.Text =
                quotation.Discount
                    .ToString(
                        "N0",
                        CultureInfo.InvariantCulture);


            // ==========================================
            // LOAD SẢN PHẨM
            // ==========================================

            List<QuotationItem> items =
                _quotationService.GetItems(
                    quotation.Id);


            foreach (QuotationItem item
                     in items)
            {
                AddProductRow(item);
            }
        }


        // ==========================================
        // CHỌN KHÁCH HÀNG
        // ==========================================

        private void CustomerComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            // Khi chương trình đang load dữ liệu cũ
            // thì không được tự động thay đổi
            // Contact và Phone.
            if (_isInitializing)
                return;


            if (CustomerComboBox.SelectedItem
                is not CustomerModel customer)
            {
                ContactTextBox.Text = "";
                PhoneTextBox.Text = "";

                return;
            }


            // Chỉ khi người dùng thực sự
            // chọn khách hàng mới thì
            // tự động lấy thông tin Customer.

            ContactTextBox.Text =
                customer.ContactPerson ?? "";


            PhoneTextBox.Text =
                customer.Phone ?? "";
        }


        // ==========================================
        // THÊM SẢN PHẨM
        // ==========================================

        private void AddProductButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AddProductRow(null);

            CalculateTotal();
        }


        // ==========================================
        // TẠO 1 DÒNG SẢN PHẨM
        // ==========================================

        private void AddProductRow(
            QuotationItem? existingItem)
        {
            // ==========================================
            // LOAD DANH SÁCH SẢN PHẨM
            // ==========================================

            List<Product> products =
                _productService.GetAll();


            // ==========================================
            // TẠO ROW
            // ==========================================

            var row =
                new ProductRowControls();


            // ==========================================
            // BORDER
            // ==========================================

            var border =
                new Border
                {
                    BorderBrush =
                        System.Windows.Media.Brushes
                            .Transparent,

                    BorderThickness =
                        new Thickness(
                            0,
                            0,
                            0,
                            1),

                    Padding =
                        new Thickness(12)
                };


            // ==========================================
            // GRID
            // ==========================================

            var grid =
                new Grid();


            // SẢN PHẨM
            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(
                            1,
                            GridUnitType.Star)
                });


            // SỐ LƯỢNG
            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(120)
                });


            // ĐƠN GIÁ
            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(150)
                });


            // THÀNH TIỀN
            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(150)
                });


            // XÓA
            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(50)
                });


            // ==========================================
            // COMBOBOX SẢN PHẨM
            // ==========================================

            var productComboBox =
                new ComboBox
                {
                    Height = 36,

                    VerticalContentAlignment =
                        VerticalAlignment.Center,

                    DisplayMemberPath =
                        "ProductName",

                    ItemsSource =
                        products
                };


            Grid.SetColumn(
                productComboBox,
                0);


            // ==========================================
            // SỐ LƯỢNG
            // ==========================================

            var quantityTextBox =
                new TextBox
                {
                    Height = 36,

                    Padding =
                        new Thickness(
                            8,
                            0,
                            8,
                            0),

                    VerticalContentAlignment =
                        VerticalAlignment.Center,

                    HorizontalContentAlignment =
                        HorizontalAlignment.Right,

                    Text = "1"
                };


            Grid.SetColumn(
                quantityTextBox,
                1);


            // ==========================================
            // ĐƠN GIÁ
            // ==========================================

            var unitPriceTextBox =
                new TextBox
                {
                    Height = 36,

                    Padding =
                        new Thickness(
                            8,
                            0,
                            8,
                            0),

                    VerticalContentAlignment =
                        VerticalAlignment.Center,

                    HorizontalContentAlignment =
                        HorizontalAlignment.Right,

                    Text = "0"
                };


            Grid.SetColumn(
                unitPriceTextBox,
                2);


            // ==========================================
            // THÀNH TIỀN
            // ==========================================

            var amountTextBlock =
                new TextBlock
                {
                    Text =
                        "0 VNĐ",

                    VerticalAlignment =
                        VerticalAlignment.Center,

                    HorizontalAlignment =
                        HorizontalAlignment.Right,

                    FontWeight =
                        FontWeights.SemiBold,

                    Foreground =
                        System.Windows.Media.Brushes
                            .Black
                };


            Grid.SetColumn(
                amountTextBlock,
                3);


            // ==========================================
            // NÚT XÓA
            // ==========================================

            var deleteButton =
                new Button
                {
                    Content = "×",

                    Width = 32,

                    Height = 32,

                    Background =
                        System.Windows.Media.Brushes
                            .Transparent,

                    BorderThickness =
                        new Thickness(0),

                    Foreground =
                        System.Windows.Media.Brushes
                            .IndianRed,

                    FontSize = 18,

                    Cursor =
                        System.Windows.Input.Cursors
                            .Hand
                };


            Grid.SetColumn(
                deleteButton,
                4);


            // ==========================================
            // GÁN CONTROL VÀO ROW
            // ==========================================

            row.ProductComboBox =
                productComboBox;

            row.QuantityTextBox =
                quantityTextBox;

            row.UnitPriceTextBox =
                unitPriceTextBox;

            row.AmountText =
                amountTextBlock;

            row.DeleteButton =
                deleteButton;


            // ==========================================
            // EVENT CHỌN SẢN PHẨM
            // ==========================================

            productComboBox.SelectionChanged +=
                (s, e) =>
                {
                    if (_isInitializing)
                        return;


                    if (productComboBox.SelectedItem
                        is Product product)
                    {
                        // Khi người dùng chọn sản phẩm
                        // thì tự lấy đơn giá.

                        unitPriceTextBox.Text =
                            product.UnitPrice
                                .ToString("N0");


                        CalculateRowAmount(
                            row);


                        CalculateTotal();
                    }
                };


            // ==========================================
            // EVENT SỐ LƯỢNG
            // ==========================================

            quantityTextBox.TextChanged +=
                (s, e) =>
                {
                    if (_isInitializing)
                        return;


                    CalculateRowAmount(
                        row);


                    CalculateTotal();
                };


            // ==========================================
            // EVENT ĐƠN GIÁ
            // ==========================================

            unitPriceTextBox.TextChanged +=
                (s, e) =>
                {
                    if (_isInitializing)
                        return;


                    CalculateRowAmount(
                        row);


                    CalculateTotal();
                };


            // ==========================================
            // EVENT XÓA
            // ==========================================

            deleteButton.Click +=
                (s, e) =>
                {
                    ProductRowsPanel.Children
                        .Remove(border);


                    _productRows.Remove(
                        row);


                    CalculateTotal();
                };


            // ==========================================
            // ADD CONTROL
            // ==========================================

            border.Child =
                grid;


            grid.Children.Add(
                productComboBox);

            grid.Children.Add(
                quantityTextBox);

            grid.Children.Add(
                unitPriceTextBox);

            grid.Children.Add(
                amountTextBlock);

            grid.Children.Add(
                deleteButton);


            ProductRowsPanel.Children.Add(
                border);


            _productRows.Add(
                row);


            // ==========================================
            // LOAD DỮ LIỆU CŨ
            // ==========================================

            if (existingItem != null)
            {
                // ======================================
                // TÌM PRODUCT THEO PRODUCT ID
                // ======================================

                Product? existingProduct =
                    products.FirstOrDefault(
                        x =>
                            x.Id ==
                            existingItem.ProductId);


                // ======================================
                // CHỌN PRODUCT CŨ
                // ======================================

                productComboBox.SelectedItem =
                    existingProduct;


                // ======================================
                // LOAD SỐ LƯỢNG CŨ
                // ======================================

                quantityTextBox.Text =
                    existingItem.Quantity
                        .ToString(
                            "N0",
                            CultureInfo.InvariantCulture);


                // ======================================
                // LOAD ĐƠN GIÁ CŨ
                // ======================================

                unitPriceTextBox.Text =
                    existingItem.UnitPrice
                        .ToString(
                            "N0",
                            CultureInfo.InvariantCulture);


                // ======================================
                // TÍNH THÀNH TIỀN
                // ======================================

                CalculateRowAmount(
                    row);
            }
        }


        // ==========================================
        // TÍNH THÀNH TIỀN
        // ==========================================

        private void CalculateRowAmount(
            ProductRowControls row)
        {
            decimal quantity =
                ParseNumber(
                    row.QuantityTextBox.Text);


            decimal unitPrice =
                ParseMoney(
                    row.UnitPriceTextBox.Text);


            decimal amount =
                quantity *
                unitPrice;


            row.AmountText.Text =
                amount.ToString("N0")
                + " VNĐ";
        }


        // ==========================================
        // TÍNH TỔNG
        // ==========================================

        private void CalculateTotal()
        {
            decimal subTotal = 0;


            foreach (ProductRowControls row
                     in _productRows)
            {
                decimal quantity =
                    ParseNumber(
                        row.QuantityTextBox.Text);


                decimal unitPrice =
                    ParseMoney(
                        row.UnitPriceTextBox.Text);


                subTotal +=
                    quantity *
                    unitPrice;
            }


            decimal discount =
                ParseMoney(
                    DiscountTextBox.Text);


            if (discount < 0)
                discount = 0;


            if (discount > subTotal)
                discount = subTotal;


            decimal total =
                subTotal -
                discount;


            // ==========================================
            // TẠM TÍNH
            // ==========================================

            SubTotalText.Text =
                subTotal.ToString("N0")
                + " VNĐ";


            // ==========================================
            // TỔNG CỘNG
            // ==========================================

            TotalAmountText.Text =
                total.ToString("N0")
                + " VNĐ";
        }


        // ==========================================
        // CHIẾT KHẤU THAY ĐỔI
        // ==========================================

        private void DiscountTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (_isInitializing)
                return;


            CalculateTotal();
        }


        // ==========================================
        // LƯU THAY ĐỔI
        // ==========================================

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            try
            {
                // ======================================
                // KIỂM TRA KHÁCH HÀNG
                // ======================================

                if (CustomerComboBox.SelectedItem
                    is not CustomerModel customer)
                {
                    MessageBox.Show(
                        "Vui lòng chọn khách hàng.",
                        "Thiếu thông tin",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ======================================
                // DANH SÁCH SẢN PHẨM
                // ======================================

                var items =
                    new List<QuotationItem>();


                var productIds =
                    new HashSet<string>();


                foreach (ProductRowControls row
                         in _productRows)
                {
                    if (row.ProductComboBox.SelectedItem
                        is not Product product)
                    {
                        continue;
                    }


                    decimal quantity =
                        ParseNumber(
                            row.QuantityTextBox.Text);


                    decimal unitPrice =
                        ParseMoney(
                            row.UnitPriceTextBox.Text);


                    // ==================================
                    // KIỂM TRA SỐ LƯỢNG
                    // ==================================

                    if (quantity <= 0)
                    {
                        MessageBox.Show(
                            $"Số lượng của " +
                            $"'{product.ProductName}' " +
                            $"phải lớn hơn 0.",
                            "Số lượng không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }


                    // ==================================
                    // KIỂM TRA ĐƠN GIÁ
                    // ==================================

                    if (unitPrice < 0)
                    {
                        MessageBox.Show(
                            $"Đơn giá của " +
                            $"'{product.ProductName}' " +
                            $"không hợp lệ.",
                            "Đơn giá không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }


                    // ==================================
                    // KIỂM TRA TRÙNG SẢN PHẨM
                    // ==================================

                    if (!productIds.Add(
                            product.Id))
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


                    // ==================================
                    // THÊM ITEM
                    // ==================================

                    items.Add(
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
                        });
                }


                // ======================================
                // PHẢI CÓ SẢN PHẨM
                // ======================================

                if (items.Count == 0)
                {
                    MessageBox.Show(
                        "Báo giá phải có ít nhất " +
                        "một sản phẩm.",
                        "Thiếu sản phẩm",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // ======================================
                // LẤY BÁO GIÁ CŨ
                // ======================================

                Quotation? quotation =
                    _quotationService.GetById(
                        _quotationId);


                if (quotation == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy báo giá.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                // ======================================
                // CẬP NHẬT THÔNG TIN
                // ======================================

                quotation.CustomerId =
                    customer.Id;


                quotation.CustomerName =
                    customer.FullName;


                // LẤY ĐÚNG GIÁ TRỊ
                // NGƯỜI LIÊN HỆ TRÊN FORM

                quotation.ContactPerson =
                    ContactTextBox.Text.Trim();


                // LẤY ĐÚNG GIÁ TRỊ
                // SỐ ĐIỆN THOẠI TRÊN FORM

                quotation.Phone =
                    PhoneTextBox.Text.Trim();


                // CHIẾT KHẤU

                quotation.Discount =
                    ParseMoney(
                        DiscountTextBox.Text);


                // GHI CHÚ

                quotation.Note =
                    NoteTextBox.Text.Trim();


                // ======================================
                // CẬP NHẬT DATABASE
                // ======================================

                _quotationService.Update(
                    quotation,
                    items);


                // ======================================
                // THÔNG BÁO
                // ======================================

                MessageBox.Show(
                    "Đã cập nhật báo giá thành công.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);


                // ======================================
                // QUAY LẠI
                // ======================================

                BackRequested?.Invoke(
                    this,
                    new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật báo giá.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // ==========================================
        // PARSE SỐ
        // ==========================================

        private decimal ParseNumber(
            string? text)
        {
            if (string.IsNullOrWhiteSpace(
                    text))
            {
                return 0;
            }


            text =
                text.Trim();


            if (decimal.TryParse(
                    text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out decimal result))
            {
                return result;
            }


            if (decimal.TryParse(
                    text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out result))
            {
                return result;
            }


            return 0;
        }


        // ==========================================
        // PARSE TIỀN
        // ==========================================

        private decimal ParseMoney(
            string? text)
        {
            if (string.IsNullOrWhiteSpace(
                    text))
            {
                return 0;
            }


            text =
                text.Trim();


            // Xóa VNĐ

            text =
                text.Replace(
                    "VNĐ",
                    "",
                    StringComparison
                        .OrdinalIgnoreCase);


            // Xóa đ

            text =
                text.Replace(
                    "đ",
                    "",
                    StringComparison
                        .OrdinalIgnoreCase);


            // Xóa dấu phẩy

            text =
                text.Replace(
                    ",",
                    "");


            text =
                text.Trim();


            if (decimal.TryParse(
                    text,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal result))
            {
                return result;
            }


            return 0;
        }


        // ==========================================
        // QUAY LẠI
        // ==========================================

        public event RoutedEventHandler BackRequested;


        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            BackRequested?.Invoke(
                this,
                new RoutedEventArgs());
        }


        // ==========================================
        // MODEL CONTROL CỦA PRODUCT ROW
        // ==========================================

        private class ProductRowControls
        {
            public ComboBox ProductComboBox
            {
                get;
                set;
            } = null!;


            public TextBox QuantityTextBox
            {
                get;
                set;
            } = null!;


            public TextBox UnitPriceTextBox
            {
                get;
                set;
            } = null!;


            public TextBlock AmountText
            {
                get;
                set;
            } = null!;


            public Button DeleteButton
            {
                get;
                set;
            } = null!;
        }
    }
}