using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExcelTool.Shared.Controls
{
    public partial class SidebarMenuItem : UserControl
    {
        public SidebarMenuItem()
        {
            InitializeComponent();
        }

        // =========================
        // ICON
        // =========================

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(string),
                typeof(SidebarMenuItem),
                new PropertyMetadata(""));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // =========================
        // TITLE
        // =========================

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(SidebarMenuItem),
                new PropertyMetadata(""));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // =========================
        // MENU CLICK
        // =========================

        public event RoutedEventHandler MenuClicked;

        private void MenuBorder_MouseLeftButtonUp(
            object sender,
            MouseButtonEventArgs e)
        {
            MenuClicked?.Invoke(
                this,
                new RoutedEventArgs());
        }
    }
}