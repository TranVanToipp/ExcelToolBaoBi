using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExcelTool.Features.Data.Create
{
    public partial class TaoSauInView : UserControl
    {
        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public TaoSauInView()
        {
            InitializeComponent();

            ShowEmptyPanel();
        }


        // =========================================================
        // TRẠNG THÁI BAN ĐẦU
        // =========================================================

        private void ShowEmptyPanel()
        {
            EmptyPanel.Visibility =
                Visibility.Visible;

            TrangPhuCanMangPanel.Visibility =
                Visibility.Collapsed;

            EpNhuCanVanPanel.Visibility =
                Visibility.Collapsed;

            BoiPanel.Visibility =
                Visibility.Collapsed;

            BookPanel.Visibility =
                Visibility.Collapsed;

            ResetSelectedButtons();
        }


        // =========================================================
        // TRÁNG PHỦ & CÁN MÀNG
        // =========================================================

        private void CoatingButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            HideAllPanels();

            TrangPhuCanMangPanel.Visibility =
                Visibility.Visible;

            SetSelectedButton(
                CoatingButton);
        }


        // =========================================================
        // ÉP NHŨ & CÁN VÂN
        // =========================================================

        private void FoilEmbossButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            HideAllPanels();

            EpNhuCanVanPanel.Visibility =
                Visibility.Visible;

            SetSelectedButton(
                FoilEmbossButton);
        }


        // =========================================================
        // BỒI
        // =========================================================

        private void BoiButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            HideAllPanels();

            BoiPanel.Visibility =
                Visibility.Visible;

            SetSelectedButton(
                BoiButton);
        }


        // =========================================================
        // RUỘT SÁCH
        // =========================================================

        private void BookButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            HideAllPanels();

            BookPanel.Visibility =
                Visibility.Visible;

            SetSelectedButton(
                BookButton);
        }


        // =========================================================
        // ẨN TẤT CẢ PANEL
        // =========================================================

        private void HideAllPanels()
        {
            EmptyPanel.Visibility =
                Visibility.Collapsed;

            TrangPhuCanMangPanel.Visibility =
                Visibility.Collapsed;

            EpNhuCanVanPanel.Visibility =
                Visibility.Collapsed;

            BoiPanel.Visibility =
                Visibility.Collapsed;

            BookPanel.Visibility =
                Visibility.Collapsed;
        }


        // =========================================================
        // RESET BUTTON
        // =========================================================

        private void ResetSelectedButtons()
        {
            Button[] buttons =
            {
                CoatingButton,
                FoilEmbossButton,
                BoiButton,
                BookButton
            };

            foreach (Button button in buttons)
            {
                button.Background =
                    Brushes.White;

                button.Foreground =
                    new SolidColorBrush(
                        Color.FromRgb(
                            71,
                            85,
                            105));
            }
        }


        // =========================================================
        // BUTTON ĐANG ĐƯỢC CHỌN
        // =========================================================

        private void SetSelectedButton(
            Button selectedButton)
        {
            ResetSelectedButtons();

            selectedButton.Background =
                new SolidColorBrush(
                    Color.FromRgb(
                        239,
                        246,
                        255));

            selectedButton.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(
                        37,
                        99,
                        235));
        }
    }
}