using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Features.Production
{
    public partial class ProductionDetailView : UserControl
    {
        private readonly string _productionId;

        public ProductionDetailView(string productionId)
        {
            InitializeComponent();

            _productionId = productionId;
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