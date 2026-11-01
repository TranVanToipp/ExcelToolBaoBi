using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using ExcelTool.Models.Pricing;
using ExcelTool.Services.Pricing;

namespace ExcelTool.Features.Data.Create.SauIn
{
    public partial class EpNhu_CanVan : UserControl
    {
        private readonly ProcessPriceSettingService
            _processPriceSettingService;

        private readonly MoldPriceSettingService
            _moldPriceSettingService;

        private List<ProcessPriceSetting>
            _processPriceSettings =
                new List<ProcessPriceSetting>();

        private List<MoldPriceSetting>
            _moldPriceSettings =
                new List<MoldPriceSetting>();


        public EpNhu_CanVan()
        {
            InitializeComponent();

            _processPriceSettingService =
                new ProcessPriceSettingService();

            _moldPriceSettingService =
                new MoldPriceSettingService();

            LoadDuLieuDonGia();
            LoadDuLieuGiaKhuon();
        }


        // =========================================================
        // LOAD DỮ LIỆU ĐƠN GIÁ
        // =========================================================

        private void LoadDuLieuDonGia()
        {
            _processPriceSettings =
                _processPriceSettingService
                    .GetAll()
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .ToList();


            // =====================================================
            // MÀU NHŨ
            //
            // Lấy từ:
            // Process = "Ép nhũ"
            // Stage   = tên màu nhũ
            // =====================================================

            List<string> danhSachMauNhu =
                _processPriceSettings
                    .Where(x =>
                        x.Process == "Ép nhũ" &&
                        !string.IsNullOrWhiteSpace(x.Stage))
                    .Select(x => x.Stage)
                    .Distinct()
                    .ToList();


            FoilTypeComboBox.ItemsSource =
                danhSachMauNhu;

            FoilType02ComboBox.ItemsSource =
                danhSachMauNhu;

            FoilType03ComboBox.ItemsSource =
                danhSachMauNhu;
        }


        // =========================================================
        // LOAD DỮ LIỆU GIÁ KHUÔN
        // =========================================================

        private void LoadDuLieuGiaKhuon()
        {
            _moldPriceSettings =
                _moldPriceSettingService
                    .GetAll()
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Id)
                    .ToList();
        }
    }
}