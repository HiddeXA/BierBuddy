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
using Material.Icons.WPF;
using Material.Icons;

namespace BierBuddy.UILib
{
    public class DateTimeAccepterDialog : Window
    {
        private Button _OkButton { get; }
        private StackPanel _DateTimeAccepters { get; }
        private StackPanel _StackPanel { get; }
        private HashSet<string> _ChoiceMade { get; } = new HashSet<string>();
        public Dictionary<List<DateTime>, bool> AcceptedNotAcceptedDateTimes => _DateTimeAccepters.Children.OfType<Grid>().ToDictionary(grid => new List<DateTime>
        {
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[1]).SelectedTime,
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[3]).SelectedTime
        }, grid => 
            ((Border)grid.Children[6]).Background == new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72))
        );

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

            _DateTimeAccepters = new StackPanel { Orientation = Orientation.Vertical };
            _StackPanel.Children.Add(_DateTimeAccepters);
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
            _OkButton.Template = GetButtonPanelButtonTemplate(UIUtils.BabyPoeder, Brushes.Black);
            _OkButton.IsEnabled = false;
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
        /// Voegt een DateTimeSelector toe aan de _DateTimeAccepters stackpanel
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
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock dateText = new TextBlock { Text = dateTimes[0].ToShortDateString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
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
            UIElement declineButton = GetDeclineButton(30, 30);
            Grid.SetColumn(declineButton, 4);
            dateTimeAccepter.Children.Add(declineButton);
            UIElement acceptButton = GetAcceptButton(30, 30);
            Grid.SetColumn(acceptButton, 5);
            dateTimeAccepter.Children.Add(acceptButton);
            Border accepted = new Border { CornerRadius = UIUtils.UniversalCornerRadius, Background = UIUtils.Outer_Space, Padding = new Thickness(5) };
            Grid.SetColumn(accepted, 6);
            dateTimeAccepter.Children.Add(accepted);
            dateTimeAccepter.Uid = Guid.NewGuid().ToString();
            _DateTimeAccepters.Children.Add(dateTimeAccepter);
        }

        private void AcceptDecline_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button == null)
            {
                return;
            }
            if (button.Parent is Grid grid)
            {
                if (button.Name == "accept")
                {
                    ((Border)grid.Children[6]).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72));
                    _ChoiceMade.Add(grid.Uid);
                    CheckIfAllChoicesMade();
                }
                else if (button.Name == "decline")
                {
                    ((Border)grid.Children[6]).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32));
                    _ChoiceMade.Add(grid.Uid);
                    CheckIfAllChoicesMade();
                }
            }
        }

        private void CheckIfAllChoicesMade()
        {
            if (_ChoiceMade.Count == _DateTimeAccepters.Children.Count)
            {
                _OkButton.IsEnabled = true;
                _OkButton.Template = GetButtonPanelButtonTemplate(new SolidColorBrush(Color.FromRgb(126, 161, 114)), Brushes.Black);
            }
        }

        private UIElement GetDeclineButton(double width, double height)
        {
            Button declineButton = new();
            declineButton.Template = GetAcceptButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)));
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMugOff;
            icon.Foreground = UIUtils.BabyPoeder;
            declineButton.Width = width;
            declineButton.Height = height;
            declineButton.Content = icon;
            declineButton.VerticalAlignment = VerticalAlignment.Center;
            declineButton.HorizontalAlignment = HorizontalAlignment.Center;
            declineButton.Background = UIUtils.Transparent;
            declineButton.Margin = new Thickness(0, 0, 5, 0);
            declineButton.Click += AcceptDecline_Clicked;
            declineButton.Name = "decline";

            return declineButton;
        }

        private UIElement GetAcceptButton(double width, double height)
        {
            Button acceptButton = new();
            acceptButton.Template = GetAcceptButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72)));

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMug;
            icon.Foreground = UIUtils.BabyPoeder;
            acceptButton.Width = width;
            acceptButton.Height = height;
            acceptButton.Content = icon;
            acceptButton.VerticalAlignment = VerticalAlignment.Center;
            acceptButton.HorizontalAlignment = HorizontalAlignment.Center;
            acceptButton.Background = UIUtils.Transparent;
            acceptButton.Margin = new Thickness(0, 0, 5, 0);
            acceptButton.Click += AcceptDecline_Clicked;
            acceptButton.Name = "accept";

            return acceptButton;
        }

        private ControlTemplate GetAcceptButtonTemplate(Brush brush)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            //set background color
            borderFactory.SetValue(Border.BackgroundProperty, brush);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;

            return template;
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
