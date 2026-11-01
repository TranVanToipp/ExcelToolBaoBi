using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using ExcelTool.Features.Customer;
using ExcelTool.Models.Pricing;
using ExcelTool.Services;
using ExcelTool.Services.Pricing;

using CustomerModel = ExcelTool.Features.Customer.Customer;


namespace ExcelTool.Features.Data.Create
{
    public partial class CreateDataInfoView : UserControl
    {
        // =========================================================
        // SERVICES
        // =========================================================

        private readonly CustomerService
            _customerService;

        private readonly BomSettingService
            _bomSettingService;

        private readonly DifficultySettingService
            _difficultySettingService;

        private readonly ReprintSettingService
            _reprintSettingService;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public CreateDataInfoView()
        {
            InitializeComponent();


            // =====================================================
            // SERVICE
            // =====================================================

            _customerService =
                new CustomerService();

            _bomSettingService =
                new BomSettingService();

            _difficultySettingService =
                new DifficultySettingService();

            _reprintSettingService =
                new ReprintSettingService();


            // =====================================================
            // DEFAULT DATE
            // =====================================================

            CreatedDatePicker.SelectedDate =
                DateTime.Today;


            // =====================================================
            // LOAD DATA
            // =====================================================

            LoadCustomers();

            LoadProductTypes();

            LoadDifficulties();

            LoadReprints();
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


                CustomerComboBox.SelectedValuePath =
                    nameof(CustomerModel.Id);


                CustomerComboBox.DisplayMemberPath =
                    nameof(CustomerModel.ShortName);
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
        // LOAD NHÓM SẢN PHẨM
        // =========================================================

        private void LoadProductTypes()
        {
            try
            {
                List<BomSetting> settings =
                    _bomSettingService.GetAll();


                List<string> productTypes =
                    new List<string>();


                foreach (BomSetting setting in settings)
                {
                    string productType =
                        setting.ProductType?.Trim()
                        ?? "";


                    if (string.IsNullOrWhiteSpace(
                        productType))
                    {
                        continue;
                    }


                    if (!productTypes.Contains(
                        productType))
                    {
                        productTypes.Add(
                            productType);
                    }
                }


                ProductTypeComboBox.ItemsSource =
                    productTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải nhóm sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LOAD ĐỘ KHÓ
        // =========================================================

        private void LoadDifficulties()
        {
            try
            {
                List<DifficultySetting> difficulties =
                    _difficultySettingService.GetAll();


                DifficultyComboBox.ItemsSource =
                    difficulties;


                // -------------------------------------------------
                // QUAN TRỌNG
                //
                // Lấy giá trị Difficulty:
                //
                // 1.0
                // 1.1
                // 1.2
                //
                // Không lấy Id.
                // -------------------------------------------------

                DifficultyComboBox.SelectedValuePath =
                    nameof(DifficultySetting.Difficulty);


                DifficultyComboBox.DisplayMemberPath =
                    nameof(DifficultySetting.Difficulty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách độ khó sản phẩm.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LOAD KIỂM SOÁT TÁI BẢN
        // =========================================================

        private void LoadReprints()
        {
            try
            {
                List<ReprintSetting> reprints =
                    _reprintSettingService.GetAll();


                ReprintComboBox.ItemsSource =
                    reprints;


                ReprintComboBox.SelectedValuePath =
                    nameof(ReprintSetting.CheckReprint);


                ReprintComboBox.DisplayMemberPath =
                    nameof(ReprintSetting.CheckReprint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách kiểm soát tái bản.\n\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================================
        // LẤY CUSTOMER ID
        // =========================================================

        public string GetCustomerId()
        {
            return
                CustomerComboBox.SelectedValue?
                    .ToString()
                ?? "";
        }


        // =========================================================
        // LẤY NHÓM SẢN PHẨM
        // =========================================================

        public string GetProductType()
        {
            if (ProductTypeComboBox.SelectedItem
                is string productType)
            {
                return productType.Trim();
            }


            return
                ProductTypeComboBox.SelectedValue?
                    .ToString()
                ?? "";
        }


        // =========================================================
        // LẤY ĐỘ KHÓ
        // =========================================================

        public double GetDifficulty()
        {
            if (DifficultyComboBox.SelectedValue == null)
            {
                return 0;
            }


            return double.TryParse(
                DifficultyComboBox.SelectedValue.ToString(),
                out double difficulty)
                ? difficulty
                : 0;
        }


        // =========================================================
        // LẤY TÁI BẢN
        // =========================================================

        public string GetReprintType()
        {
            return
                ReprintComboBox.SelectedValue?
                    .ToString()
                ?? "";
        }


        // =========================================================
        // LẤY NGÀY LẬP
        // =========================================================

        public DateTime GetCreatedDate()
        {
            return
                CreatedDatePicker.SelectedDate
                ?? DateTime.Today;
        }


        // =========================================================
        // LẤY HẠN BÁO GIÁ
        // =========================================================

        public DateTime? GetQuotationDeadline()
        {
            return
                QuotationDeadlinePicker.SelectedDate;
        }


        // =========================================================
        // LẤY TÊN SẢN PHẨM
        // =========================================================

        public string GetProductName()
        {
            return
                ProductNameTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY QUY CÁCH
        // =========================================================

        public string GetSpecification()
        {
            return
                SpecificationTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY SỐ LƯỢNG
        // =========================================================

        public string GetQuantityText()
        {
            return
                QuantityTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY KHỔ TRẢI
        // =========================================================

        public string GetSpreadSize()
        {
            return
                SpreadSizeTextBox.Text.Trim();
        }


        // =========================================================
        // LẤY GHI CHÚ
        // =========================================================

        public string GetNote()
        {
            return
                NoteTextBox.Text.Trim();
        }
    }
}