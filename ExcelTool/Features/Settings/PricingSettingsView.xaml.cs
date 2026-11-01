using ExcelTool.Features.Settings.Pricing.Views;
using ExcelTool.Services.Pricing;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Settings
{
    public partial class PricingSettingsView : UserControl
    {
        // =========================================================
        // DIFFICULTY
        // =========================================================

        private readonly DifficultySettingService
            _difficultySettingService;

        private readonly DifficultySettingExcelImporter
            _difficultySettingExcelImporter;


        // =========================================================
        // REPRINT
        // =========================================================

        private readonly ReprintSettingService
            _reprintSettingService;

        private readonly ReprintSettingExcelImporter
            _reprintSettingExcelImporter;


        // =========================================================
        // BOM - LOẠI SẢN PHẨM
        // =========================================================

        private readonly BomSettingService
            _bomSettingService;

        private readonly BomSettingExcelImporter
            _bomSettingExcelImporter;


        // =========================================================
        // PRINT BATCH - LÔ IN / LÔ KẼM
        // =========================================================

        private readonly PrintBatchSettingService
            _printBatchSettingService;

        private readonly PrintBatchSettingExcelImporter
            _printBatchSettingExcelImporter;


        // =========================================================
        // MOLD PRICE - GIÁ KHUÔN
        // =========================================================

        private readonly MoldPriceSettingService
            _moldPriceSettingService;

        private readonly MoldPriceSettingExcelImporter
            _moldPriceSettingExcelImporter;


        // =========================================================
        // DANH SÁCH PANEL
        // =========================================================

        private readonly List<UIElement>
            _panels;


        // =========================================================
        // TRẠNG THÁI SIDEBAR
        // =========================================================

        private bool _isSidebarVisible = true;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PricingSettingsView()
        {
            InitializeComponent();


            // =====================================================
            // DIFFICULTY
            // =====================================================

            _difficultySettingService =
                new DifficultySettingService();

            _difficultySettingExcelImporter =
                new DifficultySettingExcelImporter();


            // =====================================================
            // REPRINT
            // =====================================================

            _reprintSettingService =
                new ReprintSettingService();

            _reprintSettingExcelImporter =
                new ReprintSettingExcelImporter();


            // =====================================================
            // BOM - LOẠI SẢN PHẨM
            // =====================================================

            _bomSettingService =
                new BomSettingService();

            _bomSettingExcelImporter =
                new BomSettingExcelImporter();


            // =====================================================
            // PRINT BATCH - LÔ IN / LÔ KẼM
            // =====================================================

            _printBatchSettingService =
                new PrintBatchSettingService();

            _printBatchSettingExcelImporter =
                new PrintBatchSettingExcelImporter();


            // =====================================================
            // MOLD PRICE - GIÁ KHUÔN
            // =====================================================

            _moldPriceSettingService =
                new MoldPriceSettingService();

            _moldPriceSettingExcelImporter =
                new MoldPriceSettingExcelImporter();


            // =====================================================
            // DANH SÁCH PANEL
            //
            // 0  - Thông báo
            // 1  - Độ khó sản phẩm
            // 2  - Cài đặt tái bản
            // 3  - Định mức bù hao
            // 4  - Giá giấy
            // 5  - Khổ giấy - máy in - kẽm
            // 6  - Nhíp - Tay kê - Tràn màu
            // 7  - Tai dán - Khóa gài
            // 8  - Loại sản phẩm (BOM)
            // 9  - Lô in - Lô kẽm
            // 10 - Quy ước kiểu in
            // 11 - Đơn giá in và kẽm
            // 12 - Giá gia công
            // 13 - Giá khuôn
            // 14 - Đơn vị tính
            // 15 - Giá Support
            // 16 - Giá keo dán
            // 17 - Quy đổi màng
            // =====================================================

            _panels =
                new List<UIElement>
                {
                    // 0
                    NotificationPanel,

                    // 1
                    DifficultyPanel,

                    // 2
                    ReprintPanel,

                    // 3
                    WastePricingView,

                    // 4
                    PaperPricePricingView,

                    // 5
                    PaperSizePricingView,

                    // 6
                    GripPricingView,

                    // 7
                    ShippingPricingView,

                    // 8
                    BomPricingView,

                    // 9
                    PrintBatchPricingView,

                    // 10
                    PrintTypePricingView,

                    // 11
                    PrintPricePricingView,

                    // 12
                    ProcessPricePricingView,

                    // 13
                    MoldPricePricingView,

                    // 14
                    UnitPricingView,

                    // 15
                    SupportPricingView,

                    // 16
                    GluePricePricingView,

                    // 17
                    FilmPricingView
                };


            // =====================================================
            // EVENT - DIFFICULTY
            // =====================================================

            DifficultyPricingGrid.ImportRequested +=
                Difficulty_ImportRequested;


            // =====================================================
            // EVENT - REPRINT
            // =====================================================

            ReprintPricingGrid.ImportRequested +=
                Reprint_ImportRequested;


            // =====================================================
            // LOAD DỮ LIỆU
            // =====================================================

            LoadDifficultyData();

            LoadReprintData();


            // =====================================================
            // PANEL MẶC ĐỊNH
            // =====================================================

            ShowPanel(0);
        }
    }
}