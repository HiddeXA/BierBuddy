using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;

namespace BierBuddy.UILib
{
    public class DateTimeAccepterDialog : Window
    {
        private Button _OkButton { get; }
        private StackPanel _DateTimeSelectors { get; }
        private StackPanel _StackPanel { get; }
        private HashSet<string> _InvalidPickers { get; } = new HashSet<string>();
        public Dictionary<List<DateTime>, bool> AcceptedNotAcceptedDateTimes => _DateTimeSelectors.Children.OfType<Grid>().ToDictionary(grid => new List<DateTime>
        {
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[1]).SelectedTime,
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[3]).SelectedTime
        }, grid => ((CheckBox)grid.Children[4]).IsChecked.Value);

        public DateTimeAccepterDialog(List<List<DateTime>> dateTimes)
        {
            this.Title = "Beschikbaarheden";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.Background = UIUtils.Onyx;

            _StackPanel = new StackPanel { Margin = new Thickness(10) };
            TextBlock textBlock = new TextBlock { Text = "AFSPRAKEN GOEDKEUREN:" };
            textBlock.Foreground = Brushes.White;
            textBlock.FontSize = 20;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.FontFamily = UIUtils.UniversalFontFamily;
            _StackPanel.Children.Add(textBlock);

            _DateTimeSelectors = new StackPanel { Orientation = Orientation.Vertical };
            _StackPanel.Children.Add(_DateTimeSelectors);
            foreach (List<DateTime> dateTime in dateTimes)
            {
                AddDateTimeAccepter(dateTime);
            }

            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Button cancelButton = new Button { Content = "CANCEL", IsCancel = true };
            cancelButton.Template = GetButtonPanelButtonTemplate(new SolidColorBrush(Color.FromRgb(190, 55, 50)), Brushes.Black);
            _OkButton = new Button { Content = "GEEF DOOR", IsDefault = true };
            _OkButton.Click += OK_Click;
            _OkButton.Template = GetButtonPanelButtonTemplate(new SolidColorBrush(Color.FromRgb(126, 161, 114)), Brushes.Black);
            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(_OkButton);

            _StackPanel.Children.Add(buttonPanel);
            this.Content = _StackPanel;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to true (OK)
            this.DialogResult = true;
        }

        /// <summary>
        /// Voegt een DateTimeSelector toe aan de _DateTimeSelectors stackpanel
        /// </summary>
        /// <param name="dateTime">De datum die standaard geselecteerd is, ook kan er geen datum meer hiervoor gekozen worden</param>
        private void AddDateTimeAccepter(List<DateTime> dateTimes)
        {
            Grid dateTimeAccepter = new Grid();
            dateTimeAccepter.Margin = new Thickness(0, 0, 0, 5);
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock dateText = new TextBlock { Text = dateTimes[0].ToShortDateString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily };
            Border dateTextBorder = GenerateThemedTextBlock(dateText);
            Grid.SetColumn(dateTextBorder, 0);
            dateTimeAccepter.Children.Add(dateTextBorder);
            TextBlock timeTextStart = new TextBlock { Text = dateTimes[0].ToShortTimeString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Border timeTextStartBorder = GenerateThemedTextBlock(timeTextStart);
            Grid.SetColumn(timeTextStartBorder, 1);
            dateTimeAccepter.Children.Add(timeTextStartBorder);
            TextBlock textBlock = new TextBlock { Text = "TOT", Margin = new Thickness(0, 0, 5, 0), Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetColumn(textBlock, 2);
            dateTimeAccepter.Children.Add(textBlock);
            TextBlock timeTextEnd = new TextBlock { Text = dateTimes[1].ToShortTimeString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Border timeTextEndBorder = GenerateThemedTextBlock(timeTextEnd);
            Grid.SetColumn(timeTextEndBorder, 3);
            dateTimeAccepter.Children.Add(timeTextEndBorder);
            dateTimeAccepter.Uid = Guid.NewGuid().ToString();
            _DateTimeSelectors.Children.Add(dateTimeAccepter);
        }

        private void TimePicker_TimeChanged(object sender, TimeSpan e)
        {
            TimePicker timePicker = (TimePicker)sender;
            if (timePicker == null)
            {
                return;
            }
            if (timePicker.Parent is Grid grid)
            {
                if (((TimePicker)grid.Children[1]).SelectedTime.CompareTo(((TimePicker)grid.Children[3]).SelectedTime) > 0)
                {
                    ((TimePicker)grid.Children[1]).Effect = new DropShadowEffect { Color = Colors.Red, ShadowDepth = 0 };
                    ((TimePicker)grid.Children[3]).Effect = new DropShadowEffect { Color = Colors.Red, ShadowDepth = 0 };
                    _InvalidPickers.Add(grid.Uid);
                    _OkButton.IsEnabled = false;
                    _OkButton.Template = GetButtonPanelButtonTemplate(UIUtils.BabyPoeder, Brushes.Black);
                }
                else
                {
                    ((TimePicker)grid.Children[1]).Effect = null;
                    ((TimePicker)grid.Children[3]).Effect = null;
                    _InvalidPickers.Remove(grid.Uid);
                    if (_InvalidPickers.Count == 0)
                    {
                        _OkButton.Template = GetButtonPanelButtonTemplate(new SolidColorBrush(Color.FromRgb(126, 161, 114)), Brushes.Black);
                        _OkButton.IsEnabled = true;
                    }
                }
            }
        }

        private ControlTemplate GetButtonPanelButtonTemplate(Brush background, Brush foreground)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.CornerRadiusProperty, UIUtils.UniversalCornerRadius);
            border.SetValue(Border.BackgroundProperty, background);
            border.SetValue(Border.MarginProperty, new Thickness(5));
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.MarginProperty, new Thickness(20, 10, 20, 10));
            contentPresenter.SetValue(TextElement.ForegroundProperty, foreground);
            contentPresenter.SetValue(TextElement.FontWeightProperty, FontWeights.Bold);
            contentPresenter.SetValue(TextElement.FontFamilyProperty, UIUtils.UniversalFontFamily);
            border.AppendChild(contentPresenter);
            template.VisualTree = border;
            return template;
        }

        private Border GenerateThemedTextBlock(TextBlock textBlock)
        {
            Border border = new Border();
            border.CornerRadius = UIUtils.UniversalCornerRadius;
            border.Background = UIUtils.Outer_Space;
            border.Padding = new Thickness(5);
            border.Margin = new Thickness(0, 0, 5, 0);
            border.Child = textBlock;
            return border;
        }
    
    }
}
