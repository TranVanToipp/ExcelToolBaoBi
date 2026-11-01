using System.Windows;
using System.Windows.Controls;

namespace ExcelTool.Shared.Controls
{
    public partial class StatisticCard : UserControl
    {
        public StatisticCard()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(StatisticCard),
                new PropertyMetadata(string.Empty));


        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }


        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(string),
                typeof(StatisticCard),
                new PropertyMetadata(string.Empty));


        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(
                nameof(Subtitle),
                typeof(string),
                typeof(StatisticCard),
                new PropertyMetadata(string.Empty));


        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }
    }
}