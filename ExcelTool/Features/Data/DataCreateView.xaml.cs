using System;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Models;
using ExcelTool.Services;

using ExcelTool.Features.Data.Create;

namespace ExcelTool.Features.Data
{
    public partial class DataCreateView : UserControl
    {
        // =========================================================
        // SERVICE
        // =========================================================

        private readonly ProductService
            _productService;


        // =========================================================
        // CÁC VIEW
        // =========================================================

        private readonly CreateDataInfoView
            _dataInfoView;

        private readonly CreatePaperPrintView
            _paperPrintView;

        private readonly TaoSauInView _afterPrintView;


        // =========================================================
        // PRODUCT HIỆN TẠI
        // =========================================================

        // Product được tạo ở bước 1 và giữ lại để sử dụng
        // cho các bước tiếp theo.

        private Product? _currentProduct;


        // =========================================================
        // BƯỚC HIỆN TẠI
        // =========================================================

        private int _currentStep = 1;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public DataCreateView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _productService =
                new ProductService();


            // =====================================================
            // KHỞI TẠO VIEW
            // =====================================================

            _dataInfoView =
                new CreateDataInfoView();

            _paperPrintView =
                new CreatePaperPrintView();

            _afterPrintView =
                new TaoSauInView();


            // =====================================================
            // MẶC ĐỊNH BƯỚC 1
            // =====================================================

            ShowStep1();
        }


        // =========================================================
        // HIỂN THỊ BƯỚC 1
        // =========================================================

        private void ShowStep1()
        {
            _currentStep = 1;

            StepContent.Content =
                _dataInfoView;

            StepStatusText.Text =
                "Bước 1 / 5 — Thông tin chung";
        }


        // =========================================================
        // HIỂN THỊ BƯỚC 2
        // =========================================================

        private void ShowStep2()
        {
            _currentStep = 2;

            StepContent.Content =
                _paperPrintView;

            StepStatusText.Text =
                "Bước 2 / 5 — Giấy & In";
        }


        // =========================================================
        // HIỂN THỊ BƯỚC 3
        // =========================================================

        private void ShowStep3()
        {
            _currentStep = 3;

            StepContent.Content =
                _afterPrintView;

            StepStatusText.Text =
                "Bước 3 / 5 — Sau in";
        }


        // =========================================================
        // CLICK TAB 1 — THÔNG TIN CHUNG
        // =========================================================

        private void Step1Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            ShowStep1();
        }


        // =========================================================
        // CLICK TAB 2 — GIẤY & IN
        // =========================================================

        private void Step2Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            // -----------------------------------------------------
            // Chưa có Product thì không cho nhảy sang bước 2.
            //
            // Người dùng phải lưu bước 1 trước.
            // -----------------------------------------------------

            //if (_currentProduct == null)
            //{
            //    MessageBox.Show(
            //        "Vui lòng hoàn thành và lưu Thông tin chung trước.",
            //        "Chưa có dữ liệu",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Warning);

            //    return;
            //}

            ShowStep2();
        }


        // =========================================================
        // CLICK TAB 3 — SAU IN
        // =========================================================

        private void Step3Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            // -----------------------------------------------------
            // Chưa có Product thì không cho nhảy sang bước 3.
            //
            // Người dùng phải lưu bước 1 trước.
            // -----------------------------------------------------

            //if (_currentProduct == null)
            //{
            //    MessageBox.Show(
            //        "Vui lòng hoàn thành và lưu Thông tin chung trước.",
            //        "Chưa có dữ liệu",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Warning);

            //    return;
            //}

            ShowStep3();
        }


        // =========================================================
        // CLICK TAB 4 — THÀNH PHẨM
        // =========================================================

        private void Step4Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            //if (_currentProduct == null)
            //{
            //    MessageBox.Show(
            //        "Vui lòng hoàn thành và lưu Thông tin chung trước.",
            //        "Chưa 
            //MessageBox.Show(
            //    "Bước 4 — Thành phẩm chưa được triển khai.",
            //    "Thông báo",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information);
        }


        // =========================================================
        // CLICK TAB 5 — TỔNG HỢP GIÁ
        // =========================================================

        private void Step5Button_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Bước 5 — Tổng hợp giá chưa được triển khai.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================================
        // QUAY LẠI
        // =========================================================

        private void BackButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            // -----------------------------------------------------
            // Nếu đang ở bước 3
            // → quay lại bước 2.
            // -----------------------------------------------------

            if (_currentStep == 3)
            {
                ShowStep2();

                return;
            }


            // -----------------------------------------------------
            // Nếu đang ở bước 2
            // → quay lại bước 1.
            // -----------------------------------------------------

            if (_currentStep == 2)
            {
                ShowStep1();

                return;
            }


            // -----------------------------------------------------
            // Nếu đang ở bước 1
            // → quay về danh sách dữ liệu.
            // -----------------------------------------------------

            MainWindow? mainWindow =
                Window.GetWindow(this)
                as MainWindow;


            if (mainWindow == null)
            {
                return;
            }


            mainWindow.MainContent.Content =
                new DataView();
        }


        // =========================================================
        // LƯU / TIẾP THEO
        // =========================================================

        private void SaveButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            switch (_currentStep)
            {
                case 1:

                    SaveStep1AndNext();

                    break;


                case 2:

                    SaveStep2AndNext();

                    break;


                case 3:

                    SaveStep3AndNext();

                    break;


                case 4:

                    SaveStep4AndNext();

                    break;


                case 5:

                    FinishCreate();

                    break;
            }
        }


        // =========================================================
        // BƯỚC 1
        //
        // LƯU THÔNG TIN CHUNG
        // → CHUYỂN BƯỚC 2
        // =========================================================

        private void SaveStep1AndNext()
        {
            try
            {
                // =================================================
                // TÊN SẢN PHẨM
                // =================================================

                string productName =
                    _dataInfoView.GetProductName();


                if (string.IsNullOrWhiteSpace(
                    productName))
                {
                    MessageBox.Show(
                        "Vui lòng nhập Tên sản phẩm.",
                        "Thiếu dữ liệu",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }


                // =================================================
                // SỐ LƯỢNG
                // =================================================

                string quantityText =
                    _dataInfoView.GetQuantityText();


                int quantity = 0;


                if (!string.IsNullOrWhiteSpace(
                    quantityText))
                {
                    if (!int.TryParse(
                        quantityText,
                        out quantity))
                    {
                        MessageBox.Show(
                            "Số lượng yêu cầu phải là số nguyên.",
                            "Dữ liệu không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }


                    if (quantity < 0)
                    {
                        MessageBox.Show(
                            "Số lượng yêu cầu không được nhỏ hơn 0.",
                            "Dữ liệu không hợp lệ",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }
                }


                // =================================================
                // LẤY DỮ LIỆU BƯỚC 1
                // =================================================

                DateTime createdDate =
                    _dataInfoView.GetCreatedDate();


                DateTime? quotationDeadline =
                    _dataInfoView.GetQuotationDeadline();


                string customerId =
                    _dataInfoView.GetCustomerId();


                string productType =
                    _dataInfoView.GetProductType();


                double difficulty =
                    _dataInfoView.GetDifficulty();


                string reprintType =
                    _dataInfoView.GetReprintType();


                string specification =
                    _dataInfoView.GetSpecification();


                string spreadSize =
                    _dataInfoView.GetSpreadSize();


                string note =
                    _dataInfoView.GetNote();


                // =================================================
                // TẠO PRODUCT
                // =================================================

                Product product =
                    new Product
                    {
                        Id =
                            Guid.NewGuid()
                                .ToString(),

                        ProductCode =
                            GenerateProductCode(),

                        CreatedDate =
                            createdDate,

                        CustomerId =
                            customerId,

                        ProductName =
                            productName,

                        ProductType =
                            productType,

                        ReprintType =
                            reprintType,

                        Specification =
                            specification,

                        SpreadSize =
                            spreadSize,

                        Quantity =
                            quantity,

                        Difficulty =
                            difficulty,

                        QuotationDeadline =
                            quotationDeadline,

                        Note =
                            note,

                        Unit =
                            "",

                        UnitPrice =
                            0,

                        Description =
                            ""
                    };


                // =================================================
                // LƯU DATABASE
                // =================================================

                bool success =
                    _productService.Create(
                        product);


                if (!success)
                {
                    MessageBox.Show(
                        "Không thể lưu sản phẩm.",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    return;
                }


                // =================================================
                // GIỮ PRODUCT
                // =================================================

                _currentProduct =
                    product;


                // =================================================
                // CHUYỂN BƯỚC 2
                // =================================================

                ShowStep2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu thông tin chung.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // BƯỚC 2
        //
        // HIỆN TẠI CHƯA LƯU NGHIỆP VỤ.
        // =========================================================

        private void SaveStep2AndNext()
        {
            MessageBox.Show(
                "Bước 2 — Giấy & In hiện đã mở.\n\n" +
                "Nghiệp vụ lưu Giấy & In sẽ được triển khai tiếp.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================================
        // BƯỚC 3
        //
        // HIỆN TẠI CHƯA LƯU NGHIỆP VỤ.
        // =========================================================

        private void SaveStep3AndNext()
        {
            MessageBox.Show(
                "Bước 3 — Sau in hiện đã mở.\n\n" +
                "Nghiệp vụ lưu Sau in sẽ được triển khai tiếp.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================================
        // BƯỚC 4
        // =========================================================

        private void SaveStep4AndNext()
        {
            MessageBox.Show(
                "Bước 4 — Thành phẩm chưa được triển khai.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================================
        // BƯỚC 5
        // =========================================================

        private void FinishCreate()
        {
            MessageBox.Show(
                "Chức năng hoàn tất sẽ được triển khai sau.",
                "Thông báo",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================================
        // GENERATE PRODUCT CODE
        // =========================================================

        private string GenerateProductCode()
        {
            return
                "P" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff");
        }
    }
}