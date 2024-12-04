using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Documents;

namespace BierBuddy.UILib
{
    public class DateTimePlannerDialog : Window
    {
        private Button _OkButton { get; }
        private StackPanel _DateTimeSelectors { get; }
        private StackPanel _StackPanel { get; }
        private HashSet<string> _InvalidPickers { get; } = new HashSet<string>();
        public List<List<DateTime>> SelectedDateTimes => _DateTimeSelectors.Children.OfType<Grid>().Select(grid => new List<DateTime>
        {
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[1]).SelectedTime,
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[3]).SelectedTime
        }).ToList();

        public DateTimePlannerDialog()
        {
            this.Title = "Beschikbaarheden";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.Background = UIUtils.Onyx;

            _StackPanel = new StackPanel { Margin = new Thickness(10) };
            TextBlock textBlock = new TextBlock { Text = "GEEF BESCHIKBAARHEID DOOR:" };
            textBlock.Foreground = Brushes.White;
            textBlock.FontSize = 20;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.FontFamily = UIUtils.UniversalFontFamily;
            _StackPanel.Children.Add(textBlock);

            _DateTimeSelectors = new StackPanel { Orientation = Orientation.Vertical };
            _StackPanel.Children.Add(_DateTimeSelectors);
            AddDateTimeSelector(DateTime.Now);

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
        private void AddDateTimeSelector(DateTime dateTime)
        {
            Grid dateTimeSelector = new Grid();
            dateTimeSelector.Margin = new Thickness(0, 0, 0, 5);
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            DatePicker datePicker = new CustomDatePicker { SelectedDate = dateTime.Date, IsTodayHighlighted = true, DisplayDateStart = dateTime.Date, Margin = new Thickness(0, 0, 5, 0), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily };
            Grid.SetColumn(datePicker, 0);
            dateTimeSelector.Children.Add(datePicker);
            TimePicker timePickerStart = new TimePicker { Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(timePickerStart, 1);
            timePickerStart.TimeChanged += TimePicker_TimeChanged;
            dateTimeSelector.Children.Add(timePickerStart);
            TextBlock textBlock = new TextBlock { Text = "TOT", Margin = new Thickness(0, 0, 5, 0), Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetColumn(textBlock, 2);
            dateTimeSelector.Children.Add(textBlock);
            TimePicker timePickerEnd = new TimePicker { Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(timePickerEnd, 3);
            timePickerEnd.TimeChanged += TimePicker_TimeChanged;
            dateTimeSelector.Children.Add(timePickerEnd);
            dateTimeSelector.Uid = Guid.NewGuid().ToString();
            _DateTimeSelectors.Children.Add(dateTimeSelector);
            CheckSelectors();
        }

        private void CheckSelectors()
        {
            if (_DateTimeSelectors.Children.Count == 6)
            {
                _StackPanel.Children.Remove(_StackPanel.Children.OfType<Button>().First());
            }
            else
            {
                if (CheckForLastButtonWithContent("ve"))
                {
                    _StackPanel.Children.Remove(_StackPanel.Children.OfType<Button>().Last());
                }
                if (!CheckForLastButtonWithContent("vo"))
                {
                    StackPanel buttonPanel = _StackPanel.Children.OfType<StackPanel>().Last();
                    if (buttonPanel.Children.OfType<Button>().Any())
                    {
                        _StackPanel.Children.Remove(buttonPanel);
                        AddAddDateTimeSelectorButton();
                        _StackPanel.Children.Add(buttonPanel);
                    } else
                    {
                        AddAddDateTimeSelectorButton();
                    }
                }
                if (_DateTimeSelectors.Children.Count > 1 && !CheckForLastButtonWithContent("ve"))
                {
                    StackPanel buttonPanel = _StackPanel.Children.OfType<StackPanel>().Last();
                    _StackPanel.Children.Remove(buttonPanel);
                    _StackPanel.Children.Add(new Button { Content = "VERWIJDER LAATSTE DATUM", Margin = new Thickness(0, 5, 0, 0), Template = GetButtonPanelButtonTemplate(UIUtils.Outer_Space, UIUtils.BabyPoeder) });
                    _StackPanel.Children.OfType<Button>().Last().Click += (sender, e) => {
                        _DateTimeSelectors.Children.Remove(_DateTimeSelectors.Children.OfType<Grid>().Last());
                        CheckSelectors();
                    };
                    _StackPanel.Children.Add(buttonPanel);
                }
            }
        }

        private bool CheckForLastButtonWithContent(string content)
        {
            return _StackPanel.Children.OfType<Button>().Any() && _StackPanel.Children.OfType<Button>().Last().Content.ToString().StartsWith(content, StringComparison.CurrentCultureIgnoreCase);
        }

        private void AddAddDateTimeSelectorButton()
        {
            Button addDateTimeSelectorButton = new Button { Content = "VOEG DATUM TOE" };
            addDateTimeSelectorButton.Template = GetButtonPanelButtonTemplate(UIUtils.Outer_Space, UIUtils.BabyPoeder);
            addDateTimeSelectorButton.Click += (sender, e) => AddDateTimeSelector(DateTime.Now);
            _StackPanel.Children.Add(addDateTimeSelectorButton);
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
            contentPresenter.SetValue(ContentPresenter.MarginProperty, new Thickness(20, 10, 20, 10 ));
            contentPresenter.SetValue(TextElement.ForegroundProperty, foreground);
            contentPresenter.SetValue(TextElement.FontWeightProperty, FontWeights.Bold);
            contentPresenter.SetValue(TextElement.FontFamilyProperty, UIUtils.UniversalFontFamily);
            border.AppendChild(contentPresenter);
            template.VisualTree = border;
            return template;
        }
    }
}