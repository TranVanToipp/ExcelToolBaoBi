using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using ExcelTool.Models.Pricing;

namespace ExcelTool.Shared.Controls
{
    public partial class PricingDataGrid : UserControl
    {
        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public PricingDataGrid()
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

                ConfigureColumns(value);
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
        // CONFIGURE COLUMNS
        // =========================================================

        private void ConfigureColumns(
            IEnumerable? source)
        {
            DataGrid.Columns.Clear();


            if (source == null)
                return;


            Type? itemType =
                GetItemType(source);


            if (itemType == null)
                return;


            // =====================================================
            // 1. STT
            // =====================================================

            AddIndexColumn();


            // =====================================================
            // 2. ĐỘ KHÓ SẢN PHẨM
            // =====================================================

            if (itemType == typeof(DifficultySetting))
            {
                AddTextColumn(
                    "Độ khó",
                    "Difficulty");

                AddTextColumn(
                    "Tỷ lệ bù hao",
                    "WasteRate");
            }


            // =====================================================
            // 3. CÀI ĐẶT TÁI BẢN
            // =====================================================

            else if (itemType == typeof(ReprintSetting))
            {
                AddTextColumn(
                    "Check tái bản",
                    "CheckReprint");

                AddTextColumn(
                    "Thay đổi khuôn",
                    "MoldChange");
            }


            // =====================================================
            // 4. ĐỊNH MỨC BÙ HAO
            // =====================================================

            else if (itemType == typeof(WasteSetting))
            {
                AddTextColumn(
                    "Số tờ",
                    "SoTo");

                AddTextColumn(
                    "01 màu",
                    "Mau01");

                AddTextColumn(
                    "02 màu",
                    "Mau02");

                AddTextColumn(
                    "03 màu",
                    "Mau03");

                AddTextColumn(
                    "04 màu",
                    "Mau04");

                AddTextColumn(
                    "05 màu",
                    "Mau05");

                AddTextColumn(
                    "06 màu",
                    "Mau06");

                AddTextColumn(
                    "07 màu",
                    "Mau07");

                AddTextColumn(
                    "Cán màng",
                    "CanMang");

                AddTextColumn(
                    "Tráng phủ",
                    "TrangPhu");

                AddTextColumn(
                    "Bế",
                    "Be");

                AddTextColumn(
                    "Cán vân",
                    "CanVan");

                AddTextColumn(
                    "Ép nhũ",
                    "EpNhu");

                AddTextColumn(
                    "Bồi",
                    "Boi");

                AddTextColumn(
                    "Đóng kim",
                    "DongKim");

                AddTextColumn(
                    "Cắt",
                    "Cat");

                AddTextColumn(
                    "Dán",
                    "Dan");
            }


            // =====================================================
            // 5. GIÁ GIẤY
            // =====================================================

            else if (itemType == typeof(PaperPriceSetting))
            {
                AddFixedTextColumn(
                    "Loại",
                    "Loai",
                    110);

                AddFixedTextColumn(
                    "Mã giấy",
                    "MaGiay",
                    150);

                AddFixedTextColumn(
                    "Tên giấy",
                    "TenGiay",
                    190);

                AddFixedTextColumn(
                    "VNĐ/m²",
                    "VndPerM2",
                    110);

                AddFixedTextColumn(
                    "VNĐ/tấn",
                    "VndPerTon",
                    120);

                AddFixedTextColumn(
                    "79x109",
                    "Size79x109",
                    100);

                AddFixedTextColumn(
                    "65x86",
                    "Size65x86",
                    100);

                AddFixedTextColumn(
                    "65x84",
                    "Size65x84",
                    100);

                AddFixedTextColumn(
                    "80x120",
                    "Size80x120",
                    100);

                AddFixedTextColumn(
                    "79x54",
                    "Size79x54",
                    100);

                AddFixedTextColumn(
                    "39x54",
                    "Size39x54",
                    100);

                AddFixedTextColumn(
                    "60x80",
                    "Size60x80",
                    100);

                AddFixedTextColumn(
                    "65x43",
                    "Size65x43",
                    100);

                AddFixedTextColumn(
                    "60x42",
                    "Size60x42",
                    100);
            }


            // =====================================================
            // 6. QUY ƯỚC CÁC KIỂU IN
            // =====================================================

            else if (itemType == typeof(PrintTypeSetting))
            {
                AddTextColumn(
                    "Công nghệ in",
                    "PrintingTechnology");

                AddTextColumn(
                    "Kiểu in",
                    "PrintType");

                AddTextColumn(
                    "Số bộ kẽm",
                    "PlateSetCount");

                AddTextColumn(
                    "Số lượt in",
                    "PrintCount");

                AddTextColumn(
                    "Hệ số bù hao",
                    "WasteFactor");

                AddTextColumn(
                    "Hệ số giấy",
                    "PaperFactor");
            }


            // =====================================================
            // 7. ĐƠN GIÁ KẼM IN VÀ MỰC IN
            // =====================================================

            else if (itemType == typeof(PrintPriceSetting))
            {
                AddTextColumn(
                    "Khổ máy",
                    "MachineSize");

                AddTextColumn(
                    "VNĐ/kẽm",
                    "PricePerPlate");

                AddTextColumn(
                    "CMYK",
                    "Cmyk");

                AddTextColumn(
                    "Patone",
                    "Pantone");

                AddTextColumn(
                    "Giá bài CMYK (VNĐ/kẽm)",
                    "CmykJobPrice");

                AddTextColumn(
                    "Giá Bài Màu Pha (VNĐ/kẽm)",
                    "SpotColorJobPrice");
            }


            // =====================================================
            // 8. GIÁ GIA CÔNG
            // =====================================================

            else if (itemType == typeof(ProcessPriceSetting))
            {
                AddTextColumn(
                    "Quá trình",
                    "Process");

                AddTextColumn(
                    "Công đoạn",
                    "Stage");

                AddTextColumn(
                    "Đơn giá (VNĐ)",
                    "UnitPrice");

                AddTextColumn(
                    "Đvt",
                    "Unit");

                AddTextColumn(
                    "Giá lô min (VNĐ)",
                    "MinimumBatchPrice");

                AddTextColumn(
                    "Số lượng tối thiểu",
                    "MinimumQuantity");
            }


            // =====================================================
            // 9. MODEL KHÁC
            // =====================================================

            else
            {
                AddReflectionColumns(
                    itemType);
            }


            // =====================================================
            // 10. THAO TÁC
            // =====================================================

            AddActionColumn();
        }


        // =========================================================
        // GET ITEM TYPE
        // =========================================================

        private Type? GetItemType(
            IEnumerable source)
        {
            Type sourceType =
                source.GetType();


            if (sourceType.IsGenericType)
            {
                Type[] genericArguments =
                    sourceType.GetGenericArguments();


                if (genericArguments.Length > 0)
                {
                    return genericArguments[0];
                }
            }


            foreach (
                object? item
                in source)
            {
                if (item != null)
                {
                    return item.GetType();
                }
            }


            return null;
        }


        // =========================================================
        // STT COLUMN
        // =========================================================

        private void AddIndexColumn()
        {
            DataGridTemplateColumn column =
                new DataGridTemplateColumn
                {
                    Header = "STT",
                    Width = 60,
                    MinWidth = 60,
                    MaxWidth = 70
                };


            FrameworkElementFactory text =
                new FrameworkElementFactory(
                    typeof(TextBlock));


            text.SetValue(
                TextBlock.VerticalAlignmentProperty,
                VerticalAlignment.Center);


            text.SetValue(
                TextBlock.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);


            Binding binding =
                new Binding(
                    "(ItemsControl.AlternationIndex)")
                {
                    RelativeSource =
                        new RelativeSource(
                            RelativeSourceMode.FindAncestor,
                            typeof(DataGridRow),
                            1),

                    Converter =
                        new RowIndexConverter()
                };


            text.SetBinding(
                TextBlock.TextProperty,
                binding);


            DataTemplate template =
                new DataTemplate
                {
                    VisualTree = text
                };


            column.CellTemplate =
                template;


            DataGrid.Columns.Add(
                column);
        }


        // =========================================================
        // TEXT COLUMN
        // =========================================================

        private void AddTextColumn(
            string header,
            string propertyName)
        {
            DataGridTextColumn column =
                new DataGridTextColumn
                {
                    Header = header,

                    Width =
                        new DataGridLength(
                            1,
                            DataGridLengthUnitType.Star),

                    Binding =
                        new Binding(
                            propertyName)
                };


            column.ElementStyle =
                CreateTextStyle(
                    HorizontalAlignment.Center);


            DataGrid.Columns.Add(
                column);
        }


        // =========================================================
        // FIXED TEXT COLUMN
        // DÙNG CHO GIÁ GIẤY
        // =========================================================

        private void AddFixedTextColumn(
            string header,
            string propertyName,
            double width)
        {
            DataGridTextColumn column =
                new DataGridTextColumn
                {
                    Header = header,

                    Width =
                        new DataGridLength(
                            width,
                            DataGridLengthUnitType.Pixel),

                    MinWidth = width,

                    Binding =
                        new Binding(
                            propertyName)
                };


            column.ElementStyle =
                CreateTextStyle(
                    HorizontalAlignment.Center);


            DataGrid.Columns.Add(
                column);
        }


        // =========================================================
        // CREATE TEXT STYLE
        // =========================================================

        private Style CreateTextStyle(
            HorizontalAlignment horizontalAlignment)
        {
            Style style =
                new Style(
                    typeof(TextBlock));


            style.Setters.Add(
                new Setter(
                    TextBlock.VerticalAlignmentProperty,
                    VerticalAlignment.Center));


            style.Setters.Add(
                new Setter(
                    TextBlock.HorizontalAlignmentProperty,
                    horizontalAlignment));


            style.Setters.Add(
                new Setter(
                    TextBlock.TextTrimmingProperty,
                    TextTrimming.CharacterEllipsis));


            return style;
        }


        // =========================================================
        // REFLECTION COLUMNS
        // =========================================================

        private void AddReflectionColumns(
            Type itemType)
        {
            PropertyInfo[] properties =
                itemType.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance);


            foreach (
                PropertyInfo property
                in properties)
            {
                string propertyName =
                    property.Name;


                // -------------------------------------------------
                // ẨN ID
                // -------------------------------------------------

                if (string.Equals(
                        propertyName,
                        "Id",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                // -------------------------------------------------
                // ẨN SORT ORDER
                // -------------------------------------------------

                if (string.Equals(
                        propertyName,
                        "SortOrder",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                string header =
                    GetHeader(
                        propertyName);


                AddTextColumn(
                    header,
                    propertyName);
            }
        }


        // =========================================================
        // GET HEADER
        //
        // Header hiển thị theo tên trong Excel.
        // =========================================================

        private string GetHeader(
            string propertyName)
        {
            var headers =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase)
                {
                    // =================================================
                    // ĐỘ KHÓ
                    // =================================================

                    { "Difficulty", "Độ khó" },
                    { "WasteRate", "Tỷ lệ bù hao" },


                    // =================================================
                    // TÁI BẢN
                    // =================================================

                    { "CheckReprint", "Check tái bản" },
                    { "MoldChange", "Thay đổi khuôn" },


                    // =================================================
                    // ĐỊNH MỨC BÙ HAO
                    // =================================================

                    { "SoTo", "Số tờ" },

                    { "Mau01", "01 màu" },
                    { "Mau02", "02 màu" },
                    { "Mau03", "03 màu" },
                    { "Mau04", "04 màu" },
                    { "Mau05", "05 màu" },
                    { "Mau06", "06 màu" },
                    { "Mau07", "07 màu" },

                    { "CanMang", "Cán màng" },
                    { "TrangPhu", "Tráng phủ" },
                    { "Be", "Bế" },
                    { "CanVan", "Cán vân" },
                    { "EpNhu", "Ép nhũ" },
                    { "Boi", "Bồi" },
                    { "DongKim", "Đóng kim" },
                    { "Cat", "Cắt" },
                    { "Dan", "Dán" },


                    // =================================================
                    // GIÁ GIẤY
                    // =================================================

                    { "Loai", "Loại" },
                    { "MaGiay", "Mã giấy" },
                    { "TenGiay", "Tên giấy" },

                    { "VndPerM2", "VNĐ/m²" },
                    { "VndPerTon", "VNĐ/tấn" },

                    { "Size79x109", "79x109" },
                    { "Size65x86", "65x86" },
                    { "Size65x84", "65x84" },
                    { "Size80x120", "80x120" },
                    { "Size79x54", "79x54" },
                    { "Size39x54", "39x54" },
                    { "Size60x80", "60x80" },
                    { "Size65x43", "65x43" },
                    { "Size60x42", "60x42" },


                    // =================================================
                    // LÔ IN - LÔ KẼM
                    // =================================================

                    { "PrintBatch", "Lô in" },
                    { "PlateBatch", "Lô kẽm" },


                    // =================================================
                    // QUY ƯỚC CÁC KIỂU IN
                    // =================================================

                    { "PrintingTechnology", "Công nghệ in" },
                    { "PrintType", "Kiểu in" },
                    { "PlateSetCount", "Số bộ kẽm" },
                    { "PrintCount", "Số lượt in" },
                    { "WasteFactor", "Hệ số bù hao" },
                    { "PaperFactor", "Hệ số giấy" },


                    // =================================================
                    // ĐƠN GIÁ KẼM IN VÀ MỰC IN
                    // =================================================

                    { "MachineSize", "Khổ máy" },
                    { "PricePerPlate", "VNĐ/kẽm" },
                    { "Cmyk", "CMYK" },
                    { "Pantone", "Patone" },
                    { "CmykJobPrice", "Giá bài CMYK (VNĐ/kẽm)" },
                    { "SpotColorJobPrice", "Giá Bài Màu Pha (VNĐ/kẽm)" },


                    // =================================================
                    // GIÁ GIA CÔNG
                    // =================================================

                    { "Process", "Quá trình" },
                    { "Stage", "Công đoạn" },
                    { "UnitPrice", "Đơn giá (VNĐ)" },
                    { "Unit", "Đvt" },
                    { "MinimumBatchPrice", "Giá lô min (VNĐ)" },
                    { "MinimumQuantity", "Số lượng tối thiểu" }
                };


            if (headers.TryGetValue(
                    propertyName,
                    out string? header))
            {
                return header;
            }


            // =====================================================
            // CHƯA CÓ MAPPING
            //
            // Không làm hỏng bảng.
            // Model mới vẫn hiển thị được.
            // =====================================================

            return propertyName;
        }


        // =========================================================
        // ACTION COLUMN
        // =========================================================

        private void AddActionColumn()
        {
            DataGridTemplateColumn actionColumn =
                new DataGridTemplateColumn
                {
                    Header = "Thao tác",

                    Width =
                        new DataGridLength(
                            150,
                            DataGridLengthUnitType.Pixel),

                    MinWidth = 150,

                    MaxWidth = 150
                };


            DataTemplate template =
                new DataTemplate();


            FrameworkElementFactory panel =
                new FrameworkElementFactory(
                    typeof(StackPanel));


            panel.SetValue(
                StackPanel.OrientationProperty,
                Orientation.Horizontal);


            panel.SetValue(
                StackPanel.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);


            // =====================================================
            // BUTTON SỬA
            // =====================================================

            FrameworkElementFactory editButton =
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
                    8,
                    0));


            editButton.SetValue(
                Button.ForegroundProperty,
                System.Windows.Media.Brushes
                    .DodgerBlue);


            editButton.SetValue(
                Button.TagProperty,
                new Binding());


            editButton.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler(
                    EditButton_Click));


            panel.AppendChild(
                editButton);


            // =====================================================
            // BUTTON XÓA
            // =====================================================

            FrameworkElementFactory deleteButton =
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
                Button.TagProperty,
                new Binding());


            deleteButton.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler(
                    DeleteButton_Click));


            panel.AppendChild(
                deleteButton);


            template.VisualTree =
                panel;


            actionColumn.CellTemplate =
                template;


            DataGrid.Columns.Add(
                actionColumn);
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


            // -----------------------------------------------------
            // KHÔNG CÓ TỪ KHÓA
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                    keyword))
            {
                view.Filter = null;

                view.Refresh();

                return;
            }


            // -----------------------------------------------------
            // FILTER
            // -----------------------------------------------------

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
                            // Bỏ qua property không đọc được
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


    // =============================================================
    // EVENT ARGS
    // =============================================================

    public class PricingDataGridItemEventArgs
        : EventArgs
    {
        public object Item { get; }


        public PricingDataGridItemEventArgs(
            object item)
        {
            Item = item;
        }
    }


    // =============================================================
    // STT CONVERTER
    // =============================================================

    public class RowIndexConverter
        : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value is int index)
            {
                return (
                    index + 1)
                    .ToString();
            }


            return "";
        }


        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}