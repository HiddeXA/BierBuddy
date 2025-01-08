using System.Windows.Controls;
using System.Windows.Documents;
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
        public List<bool> AcceptedNotAcceptedDateTimes => _DateTimeAccepters.Children.OfType<Grid>().Select(grid =>
            ((Border)grid.Children[6]).Background == UIUtils.AcceptGreen
        ).ToList();

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

            //alle datetimes toevoegen aan de stackpanel
            _DateTimeAccepters = new StackPanel { Orientation = Orientation.Vertical };
            _StackPanel.Children.Add(_DateTimeAccepters);
            foreach (List<DateTime> dateTime in dateTimes)
            {
                AddDateTimeAccepter(dateTime);
            }

            //de buttons toevoegen. de geef door is standaard niet enabled omdat nog niet alle beschikbaarheden zijn beoordeeld
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Button cancelButton = new Button { Content = "CANCEL", IsCancel = true };
            cancelButton.Template = GetButtonPanelButtonTemplate(UIUtils.DeclineRed, Brushes.Black);
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
            this.DialogResult = true;
        }

        /// <summary>
        /// Voegt een DateTimeAccepter toe aan de _DateTimeAccepters stackpanel
        /// </summary>
        /// <param name="dateTime">De datum die standaard weergeven wordt</param>
        private void AddDateTimeAccepter(List<DateTime> dateTimes)
        {
            Grid dateTimeAccepter = new Grid();
            dateTimeAccepter.Margin = new Thickness(0, 0, 0, 5);
            //7 kolommen: datum, starttijd, tot, eindtijd, decline, accept, accepted
            //datum
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock dateText = new TextBlock { Text = dateTimes[0].ToShortDateString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Border dateTextBorder = GenerateThemedTextBlock(dateText);
            Grid.SetColumn(dateTextBorder, 0);
            dateTimeAccepter.Children.Add(dateTextBorder);

            //starttijd
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock timeTextStart = new TextBlock { Text = dateTimes[0].ToShortTimeString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Border timeTextStartBorder = GenerateThemedTextBlock(timeTextStart);
            Grid.SetColumn(timeTextStartBorder, 1);
            dateTimeAccepter.Children.Add(timeTextStartBorder);

            //tot, deze heeft geen GenerateThemedTextBlock omdat hij geen achtergrond heeft
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock textBlock = new TextBlock { Text = "TOT", Margin = new Thickness(0, 0, 5, 0), Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetColumn(textBlock, 2);
            dateTimeAccepter.Children.Add(textBlock);

            //eindtijd
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            TextBlock timeTextEnd = new TextBlock { Text = dateTimes[1].ToShortTimeString(), Background = UIUtils.Outer_Space, Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            Border timeTextEndBorder = GenerateThemedTextBlock(timeTextEnd);
            Grid.SetColumn(timeTextEndBorder, 3);
            dateTimeAccepter.Children.Add(timeTextEndBorder);

            //decline
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            UIElement declineButton = GetDeclineButton(30, 30);
            Grid.SetColumn(declineButton, 4);
            dateTimeAccepter.Children.Add(declineButton);

            //accept
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            UIElement acceptButton = GetAcceptButton(30, 30);
            Grid.SetColumn(acceptButton, 5);
            dateTimeAccepter.Children.Add(acceptButton);

            //accepted
            dateTimeAccepter.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            Border accepted = new Border { CornerRadius = UIUtils.UniversalCornerRadius, Background = UIUtils.Outer_Space, Padding = new Thickness(5) };
            Grid.SetColumn(accepted, 6);
            dateTimeAccepter.Children.Add(accepted);

            //uid toevoegen aan de grid zodat we deze kunnen gebruiken om te checken of de gebruiker al een keuze heeft gemaakt
            dateTimeAccepter.Uid = Guid.NewGuid().ToString();
            _DateTimeAccepters.Children.Add(dateTimeAccepter);
        }

        /// <summary>
        /// zet het bolletje achter de datum op groen of rood
        /// </summary>
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
                    ((Border)grid.Children[6]).Background = UIUtils.AcceptGreen;
                    _ChoiceMade.Add(grid.Uid);
                    CheckIfAllChoicesMade();
                }
                else if (button.Name == "decline")
                {
                    ((Border)grid.Children[6]).Background = UIUtils.DeclineRed;
                    _ChoiceMade.Add(grid.Uid);
                    CheckIfAllChoicesMade();
                }
            }
        }

        /// <summary>
        /// kijken of alle keuzes gemaakt zijn, zo ja dan kan de gebruiker op de OK knop drukken
        /// </summary>
        private void CheckIfAllChoicesMade()
        {
            if (_ChoiceMade.Count == _DateTimeAccepters.Children.Count)
            {
                _OkButton.IsEnabled = true;
                _OkButton.Template = GetButtonPanelButtonTemplate(UIUtils.AcceptGreen, Brushes.Black);
            }
        }

        private UIElement GetDeclineButton(double width, double height)
        {
            Button declineButton = new();
            declineButton.Template = GetAcceptButtonTemplate(UIUtils.DeclineRed);

            return GenerateAcceptDeclineButton(declineButton, width, height, MaterialIconKind.GlassMugOff, "decline");
        }

        private UIElement GetAcceptButton(double width, double height)
        {
            Button acceptButton = new();
            acceptButton.Template = GetAcceptButtonTemplate(UIUtils.AcceptGreen);

            return GenerateAcceptDeclineButton(acceptButton, width, height, MaterialIconKind.Glass, "accept");

        }

        /// <summary>
        /// zet alle standaard waardes voor de accept en decline buttons
        /// </summary>
        private Button GenerateAcceptDeclineButton(Button button, double width, double height, MaterialIconKind iconKind, string name)
        {
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = iconKind;
            icon.Foreground = UIUtils.BabyPoeder;
            button.Width = width;
            button.Height = height;
            button.Content = icon;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Background = UIUtils.Transparent;
            button.Margin = new Thickness(0, 0, 5, 0);
            button.Click += AcceptDecline_Clicked;
            button.Name = name;

            return button;
        }

        private ControlTemplate GetAcceptButtonTemplate(Brush brush)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            //achtergrond
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, brush);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            //voorgrond
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

            //achtergrond
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.CornerRadiusProperty, UIUtils.UniversalCornerRadius);
            border.SetValue(Border.BackgroundProperty, background);
            border.SetValue(Border.MarginProperty, new Thickness(5));

            //voorgrond
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

        /// <summary>
        /// deze functie zet een textblock in een border zodat deze geround is
        /// </summary>
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
