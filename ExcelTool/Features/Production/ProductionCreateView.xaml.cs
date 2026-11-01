using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Production
{
    public partial class ProductionCreateView : UserControl
    {
        public ProductionCreateView()
        {
            InitializeComponent();
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
    }
}