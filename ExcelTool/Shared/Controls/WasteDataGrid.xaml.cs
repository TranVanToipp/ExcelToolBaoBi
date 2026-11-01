using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExcelTool.Shared.Controls
{
    public partial class WasteDataGrid : UserControl
    {
        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public WasteDataGrid()
        {
            InitializeComponent();
        }


        // =========================================================
        // ITEMS SOURCE
        // =========================================================

        public IEnumerable? ItemsSource
        {
            get
            {
                return DataGrid.ItemsSource;
            }

            set
            {
                DataGrid.ItemsSource = value;
            }
        }


        // =========================================================
        // ADD
        // =========================================================

        public event RoutedEventHandler? AddRequested;


        private void AddButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            AddRequested?.Invoke(
                this,
                e);
        }


        // =========================================================
        // IMPORT EXCEL
        // =========================================================

        public event RoutedEventHandler? ImportRequested;


        private void ImportButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ImportRequested?.Invoke(
                this,
                e);
        }


        // =========================================================
        // EDIT
        // =========================================================

        public event EventHandler<PricingDataGridItemEventArgs>?
            EditRequested;


        private void EditButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            object? item =
                button.Tag;


            if (item == null)
                return;


            EditRequested?.Invoke(
                this,
                new PricingDataGridItemEventArgs(item));
        }


        // =========================================================
        // DELETE
        // =========================================================

        public event EventHandler<PricingDataGridItemEventArgs>?
            DeleteRequested;


        private void DeleteButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;


            object? item =
                button.Tag;


            if (item == null)
                return;


            DeleteRequested?.Invoke(
                this,
                new PricingDataGridItemEventArgs(item));
        }


        // =========================================================
        // AUTO GENERATE COLUMN
        // =========================================================

        private void DataGrid_AutoGeneratingColumn(
            object sender,
            DataGridAutoGeneratingColumnEventArgs e)
        {
            string propertyName =
                e.PropertyName;


            // -----------------------------------------------------
            // ẨN ID
            // -----------------------------------------------------

            if (string.Equals(
                    propertyName,
                    "Id",
                    StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;

                return;
            }


            // -----------------------------------------------------
            // ẨN SORT ORDER
            // -----------------------------------------------------

            if (string.Equals(
                    propertyName,
                    "SortOrder",
                    StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;

                return;
            }


            // -----------------------------------------------------
            // HEADER
            // -----------------------------------------------------

            e.Column.Header =
                GetColumnHeader(propertyName);


            // -----------------------------------------------------
            // WIDTH
            // -----------------------------------------------------

            e.Column.Width =
                new DataGridLength(
                    110,
                    DataGridLengthUnitType.Pixel);


            e.Column.MinWidth =
                95;


            e.Column.MaxWidth =
                180;


            // -----------------------------------------------------
            // CĂN GIỮA
            // -----------------------------------------------------

            if (e.Column is DataGridTextColumn textColumn)
            {
                textColumn.ElementStyle =
                    new Style(
                        typeof(TextBlock))
                    {
                        Setters =
                        {
                            new Setter(
                                TextBlock.HorizontalAlignmentProperty,
                                HorizontalAlignment.Center),

                            new Setter(
                                TextBlock.VerticalAlignmentProperty,
                                VerticalAlignment.Center),

                            new Setter(
                                TextBlock.TextTrimmingProperty,
                                TextTrimming.CharacterEllipsis)
                        }
                    };
            }


            // -----------------------------------------------------
            // PROPERTY SỐ TỜ
            // -----------------------------------------------------

            if (IsPaperQuantityProperty(propertyName))
            {
                e.Column.DisplayIndex = 0;

                e.Column.Width =
                    new DataGridLength(
                        125,
                        DataGridLengthUnitType.Pixel);

                e.Column.MinWidth = 110;
                e.Column.MaxWidth = 150;
            }
        }


        // =========================================================
        // SAU KHI TẠO CỘT
        // =========================================================

        private void DataGrid_AutoGeneratedColumns(
            object? sender,
            EventArgs e)
        {
            // -----------------------------------------------------
            // XÓA CỘT THAO TÁC CŨ NẾU CÓ
            // -----------------------------------------------------

            DataGridColumn? oldActionColumn =
                DataGrid.Columns.FirstOrDefault(
                    column =>
                        string.Equals(
                            column.Header?.ToString(),
                            "Thao tác",
                            StringComparison.OrdinalIgnoreCase));


            if (oldActionColumn != null)
            {
                DataGrid.Columns.Remove(
                    oldActionColumn);
            }


            // -----------------------------------------------------
            // TẠO CỘT THAO TÁC
            // -----------------------------------------------------

            var actionColumn =
                new DataGridTemplateColumn
                {
                    Header = "Thao tác",

                    Width =
                        new DataGridLength(
                            155,
                            DataGridLengthUnitType.Pixel),

                    MinWidth = 155,

                    MaxWidth = 155
                };


            // -----------------------------------------------------
            // CELL TEMPLATE
            // -----------------------------------------------------

            var template =
                new DataTemplate();


            var factory =
                new FrameworkElementFactory(
                    typeof(StackPanel));


            factory.SetValue(
                StackPanel.OrientationProperty,
                Orientation.Horizontal);


            factory.SetValue(
                StackPanel.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);


            // -----------------------------------------------------
            // NÚT SỬA
            // -----------------------------------------------------

            var editButton =
                new FrameworkElementFactory(
                    typeof(Button));


            editButton.SetValue(
                Button.ContentProperty,
                "Sửa");


            editButton.SetValue(
                Button.WidthProperty,
                65.0);


            editButton.SetValue(
                Button.HeightProperty,
                32.0);


            editButton.SetValue(
                Button.MarginProperty,
                new Thickness(
                    0,
                    0,
                    6,
                    0));


            editButton.SetValue(
                Button.ForegroundProperty,
                System.Windows.Media.Brushes
                    .DodgerBlue);


            editButton.SetValue(
                Button.BackgroundProperty,
                System.Windows.Media.Brushes.White);


            editButton.SetValue(
                Button.BorderBrushProperty,
                new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(
                        210,
                        220,
                        235)));


            editButton.SetValue(
                Button.CursorProperty,
                System.Windows.Input.Cursors.Hand);


            editButton.SetBinding(
                Button.TagProperty,
                new Binding());


            editButton.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler(
                    EditButton_Click));


            factory.AppendChild(
                editButton);


            // -----------------------------------------------------
            // NÚT XÓA
            // -----------------------------------------------------

            var deleteButton =
                new FrameworkElementFactory(
                    typeof(Button));


            deleteButton.SetValue(
                Button.ContentProperty,
                "Xóa");


            deleteButton.SetValue(
                Button.WidthProperty,
                65.0);


            deleteButton.SetValue(
                Button.HeightProperty,
                32.0);


            deleteButton.SetValue(
                Button.ForegroundProperty,
                System.Windows.Media.Brushes.Red);


            deleteButton.SetValue(
                Button.BackgroundProperty,
                System.Windows.Media.Brushes.White);


            deleteButton.SetValue(
                Button.BorderBrushProperty,
                new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(
                        235,
                        210,
                        210)));


            deleteButton.SetValue(
                Button.CursorProperty,
                System.Windows.Input.Cursors.Hand);


            deleteButton.SetBinding(
                Button.TagProperty,
                new Binding());


            deleteButton.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler(
                    DeleteButton_Click));


            factory.AppendChild(
                deleteButton);


            template.VisualTree =
                factory;


            actionColumn.CellTemplate =
                template;


            DataGrid.Columns.Add(
                actionColumn);
        }


        // =========================================================
        // XỬ LÝ HEADER
        // =========================================================

        private static string GetColumnHeader(
            string propertyName)
        {
            if (string.IsNullOrWhiteSpace(
                    propertyName))
            {
                return "";
            }


            // -----------------------------------------------------
            // CÁC TÊN THƯỜNG GẶP
            // -----------------------------------------------------

            var headers =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    { "PaperQuantity", "Số tờ" },
                    { "Quantity", "Số tờ" },

                    { "Color01", "01 màu" },
                    { "Color02", "02 màu" },
                    { "Color03", "03 màu" },
                    { "Color04", "04 màu" },
                    { "Color05", "05 màu" },
                    { "Color06", "06 màu" },
                    { "Color07", "07 màu" },

                    { "CanMang", "Cán màng" },
                    { "TrangPhu", "Tráng phủ" },
                    { "Be", "Bế" },
                    { "CanVan", "Cán vân" },
                    { "EpNhũ", "Ép nhũ" },
                    { "EpNhu", "Ép nhũ" },
                    { "Boi", "Bồi" },
                    { "DongKim", "Đóng kim" },
                    { "Cat", "Cắt" },
                    { "Dan", "Dán" }
                };


            if (headers.TryGetValue(
                    propertyName,
                    out string? header))
            {
                return header;
            }


            // -----------------------------------------------------
            // TỰ ĐỘNG TÁCH CAMEL CASE
            // -----------------------------------------------------

            string result =
                propertyName;


            for (
                int i = result.Length - 1;
                i > 0;
                i--)
            {
                if (char.IsUpper(
                        result[i]))
                {
                    result =
                        result.Insert(
                            i,
                            " ");
                }
            }


            return result;
        }


        // =========================================================
        // KIỂM TRA CỘT SỐ TỜ
        // =========================================================

        private static bool IsPaperQuantityProperty(
            string propertyName)
        {
            return
                string.Equals(
                    propertyName,
                    "PaperQuantity",
                    StringComparison.OrdinalIgnoreCase)
                ||
                string.Equals(
                    propertyName,
                    "Quantity",
                    StringComparison.OrdinalIgnoreCase)
                ||
                string.Equals(
                    propertyName,
                    "SoTo",
                    StringComparison.OrdinalIgnoreCase)
                ||
                string.Equals(
                    propertyName,
                    "PaperCount",
                    StringComparison.OrdinalIgnoreCase);
        }


        // =========================================================
        // SEARCH
        // =========================================================

        private void SearchTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (DataGrid.ItemsSource == null)
                return;


            ICollectionView view =
                CollectionViewSource.GetDefaultView(
                    DataGrid.ItemsSource);


            string keyword =
                SearchTextBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(
                    keyword))
            {
                view.Filter = null;

                view.Refresh();

                return;
            }


            view.Filter =
                item =>
                {
                    if (item == null)
                        return false;


                    PropertyInfo[] properties =
                        item.GetType()
                            .GetProperties(
                                BindingFlags.Public |
                                BindingFlags.Instance);


                    foreach (
                        PropertyInfo property
                        in properties)
                    {
                        try
                        {
                            object? value =
                                property.GetValue(
                                    item);


                            if (value == null)
                                continue;


                            string text =
                                Convert.ToString(
                                    value,
                                    CultureInfo.CurrentCulture)
                                ?? "";


                            if (text.IndexOf(
                                    keyword,
                                    StringComparison
                                        .CurrentCultureIgnoreCase)
                                >= 0)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                            // Bỏ qua property lỗi
                        }
                    }


                    return false;
                };


            view.Refresh();
        }


        // =========================================================
        // CLEAR SEARCH
        // =========================================================

        private void ClearSearchButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SearchTextBox.Clear();
        }
    }
}