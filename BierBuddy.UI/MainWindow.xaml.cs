using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BierBuddy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly double _SizeModifierNavBar = 0.25;
        private readonly int _NavBarMinSize = 290;

        private readonly int _MinFontSize = 26;
        private readonly int _DefaultFontSize = 32;
        private readonly int _FontSizeIncrement = 30;
        private int FontSizeModifier { get; }

        public MainWindow()
        {
            InitializeComponent();
            FontSizeModifier = _MinFontSize - _NavBarMinSize / _FontSizeIncrement;
        }
        private void BierBuddyMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (BBMainWindow.ActualWidth * _SizeModifierNavBar > _NavBarMinSize)
            {
                NavBar.Width = BBMainWindow.ActualWidth * _SizeModifierNavBar;

                MoveBeerFoam(e);
                int fontSize = CalculateNavBarFontSize();
                FindBuddiesLabel.FontSize = fontSize;
                MyBuddiesLabel.FontSize = fontSize;
            }
            else
            {
                NavBar.Width = _NavBarMinSize;
                //FindBuddiesLabel.FontSize = _MinFontSize;
                //MyBuddiesLabel.FontSize = _MinFontSize;
            }
            
        }

        private void FindBuddyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyBuddiesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup
            {
                Placement = System.Windows.Controls.Primitives.PlacementMode.Center,
                StaysOpen = false
            };

            // Inhoud van de popup
            Border border = new Border
            {
                Background = System.Windows.Media.Brushes.LightGray,
                BorderBrush = System.Windows.Media.Brushes.Black,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10)
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock { Text = "Dit is een dynamische popup!", FontWeight = FontWeights.Bold, FontSize = 14 });
            Button closeButton = new Button { Content = "Sluiten", Margin = new Thickness(5) };
            closeButton.Click += (s, e) => popup.IsOpen = false;
            panel.Children.Add(closeButton);

            border.Child = panel;
            popup.Child = border;

            // Popup openen
            popup.IsOpen = true;
        }

        private int CalculateNavBarFontSize()
        {
            int fontSize = (int)(NavBar.Width / _FontSizeIncrement + FontSizeModifier);
            return fontSize;
        }
        private bool _First = true;
        private void MoveBeerFoam(SizeChangedEventArgs e)
        {
            Canvas.SetLeft(EllipseFoam0, NavBar.Width - 130);
            Canvas.SetLeft(EllipseFoam1, NavBar.Width - 218);
            Canvas.SetLeft(EllipseFoam2, NavBar.Width - 300);
            Canvas.SetLeft(EllipseFoam3, NavBar.Width - 365);
            Canvas.SetLeft(EllipseFoam4, NavBar.Width - 412);
        }
    }
}