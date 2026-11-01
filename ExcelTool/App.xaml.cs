using System.Windows;
using ExcelTool.Database;
using QuestPDF.Infrastructure;

namespace ExcelTool
{
    public partial class App : Application
    {
        protected override void OnStartup(
            StartupEventArgs e)
        {
            base.OnStartup(e);


            // =====================================================
            // CẤU HÌNH QUESTPDF
            // =====================================================

            QuestPDF.Settings.License =
                LicenseType.Evaluation;


            // =====================================================
            // KHỞI TẠO DATABASE
            // =====================================================

            DatabaseInitializer.Initialize();


            // =====================================================
            // TẠO MAIN WINDOW
            // =====================================================

            var mainWindow =
                new MainWindow();


            // =====================================================
            // GÁN MAIN WINDOW CHO APPLICATION
            // =====================================================

            MainWindow =
                mainWindow;


            // =====================================================
            // HIỂN THỊ MAIN WINDOW
            // =====================================================

            mainWindow.Show();
        }
    }
}