using System.Windows;

namespace ExcelTool.Features.Settings
{
    public partial class PricingSettingsView
    {
        // =========================================================
        // HIỂN THỊ PANEL
        // =========================================================

        private void ShowPanel(int index)
        {
            if (index < 0 || index >= _panels.Count)
            {
                return;
            }

            for (int i = 0; i < _panels.Count; i++)
            {
                _panels[i].Visibility =
                    i == index
                        ? Visibility.Visible
                        : Visibility.Collapsed;
            }
        }


        // =========================================================
        // 0. THÔNG BÁO
        // =========================================================

        private void NotificationButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "0. Thông báo";

            CurrentDescription.Text =
                "Các thông báo được sử dụng trong quá trình tính giá.";

            ShowPanel(0);
        }


        // =========================================================
        // 1. ĐỘ KHÓ SẢN PHẨM
        // =========================================================

        private void DifficultyButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "1. Độ khó sản phẩm";

            CurrentDescription.Text =
                "Cấu hình tỷ lệ bù hao theo độ khó sản phẩm.";

            ShowPanel(1);

            LoadDifficultyData();
        }


        // =========================================================
        // 2. CÀI ĐẶT TÁI BẢN
        // =========================================================

        private void ReprintButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "2. Cài đặt tái bản";

            CurrentDescription.Text =
                "Quy định thay đổi khuôn theo loại tái bản.";

            ShowPanel(2);

            LoadReprintData();
        }


        // =========================================================
        // 3. ĐỊNH MỨC BÙ HAO
        // =========================================================

        private void WasteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "3. Định mức bù hao";

            CurrentDescription.Text =
                "Định mức bù hao giấy qua các công đoạn.";

            ShowPanel(3);
        }


        // =========================================================
        // 4. GIÁ GIẤY
        // =========================================================

        private void PaperPriceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "4. Giá giấy";

            CurrentDescription.Text =
                "Danh mục giấy và đơn giá.";

            ShowPanel(4);
        }


        // =========================================================
        // 5. KHỔ GIẤY - MÁY IN - KẼM
        // =========================================================

        private void PrintSizeButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "5. Khổ giấy - máy in - kẽm";

            CurrentDescription.Text =
                "Thiết lập khổ giấy, máy in và khổ kẽm.";

            ShowPanel(5);
        }


        // =========================================================
        // 6. NHÍP - TAY KÊ - TRÀN MÀU
        // =========================================================

        private void GripButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "6. Nhíp - Tay kê - Tràn màu";

            CurrentDescription.Text =
                "Thiết lập nhíp, tay kê và phần tràn màu.";

            ShowPanel(6);
        }


        // =========================================================
        // 7. TAI DÁN - KHÓA GÀI
        // =========================================================

        private void GlueTabButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "7. Tai dán - Khóa gài";

            CurrentDescription.Text =
                "Thiết lập kích thước tai dán và khóa gài.";

            ShowPanel(7);
        }


        // =========================================================
        // 8. BOM
        // =========================================================

        private void BomButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "8. Chủng loại sản phẩm (BOM)";

            CurrentDescription.Text =
                "Cấu hình kiểu dáng, quy cách và công thức khổ trải.";

            ShowPanel(8);
        }


        // =========================================================
        // 9. LÔ IN - LÔ KẼM
        // =========================================================

        private void PrintBatchButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "9. Lô in - Lô kẽm";

            CurrentDescription.Text =
                "Thiết lập số lượng tối thiểu cho lô in và lô kẽm.";

            ShowPanel(9);
        }


        // =========================================================
        // 10. QUY ƯỚC KIỂU IN
        // =========================================================

        private void PrintTypeButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "10. Quy ước kiểu in";

            CurrentDescription.Text =
                "Thiết lập công nghệ in, số bộ kẽm, lượt in và hệ số.";

            ShowPanel(10);
        }


        // =========================================================
        // 11. ĐƠN GIÁ IN VÀ KẼM
        // =========================================================

        private void PrintPriceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "11. Đơn giá in và kẽm";

            CurrentDescription.Text =
                "Thiết lập giá kẽm, mực và giá bài.";

            ShowPanel(11);
        }


        // =========================================================
        // 12. GIÁ GIA CÔNG
        // =========================================================

        private void ProcessPriceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "12. Giá gia công";

            CurrentDescription.Text =
                "Danh mục công đoạn sản xuất và đơn giá.";

            ShowPanel(12);
        }


        // =========================================================
        // 13. GIÁ KHUÔN
        // =========================================================

        private void MoldPriceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "13. Giá khuôn";

            CurrentDescription.Text =
                "Thiết lập giá các loại khuôn.";

            ShowPanel(13);
        }


        // =========================================================
        // 14. ĐƠN VỊ TÍNH
        // =========================================================

        private void UnitButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "14. Đơn vị tính";

            CurrentDescription.Text =
                "Danh mục đơn vị dùng trong tính giá.";

            ShowPanel(14);
        }


        // =========================================================
        // 15. GIÁ SUPPORT
        // =========================================================

        private void SupportButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "15. Giá Support";

            CurrentDescription.Text =
                "Thiết lập giá vật liệu Support.";

            ShowPanel(15);
        }


        // =========================================================
        // 16. GIÁ KEO DÁN
        // =========================================================

        private void GluePriceButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "16. Giá keo dán";

            CurrentDescription.Text =
                "Thiết lập giá keo dán.";

            ShowPanel(16);
        }


        // =========================================================
        // 17. QUY ĐỔI MÀNG
        // =========================================================

        private void FilmButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CurrentTitle.Text =
                "17. Quy đổi màng";

            CurrentDescription.Text =
                "Quy đổi độ dày, chiều dài, trọng lượng và diện tích màng.";

            ShowPanel(17);
        }


        // =========================================================
        // SIDEBAR ẨN / HIỆN
        // =========================================================

        private void SidebarToggleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _isSidebarVisible =
                !_isSidebarVisible;

            if (_isSidebarVisible)
            {
                SidebarColumn.Width =
                    new GridLength(255);

                PricingSidebar.Visibility =
                    Visibility.Visible;
            }
            else
            {
                SidebarColumn.Width =
                    new GridLength(0);

                PricingSidebar.Visibility =
                    Visibility.Collapsed;
            }
        }
    }
}