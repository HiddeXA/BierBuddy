using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        private int FontSizeModifier {  get;}

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


                int fontSize = CalculateNavBarFontSize();
                FindBuddiesLabel.FontSize = fontSize;
                MyBuddiesLabel.FontSize = fontSize;
            }
            else
            {
                NavBar.Width = _NavBarMinSize;
                FindBuddiesLabel.FontSize = _MinFontSize;
                MyBuddiesLabel.FontSize = _MinFontSize;
            }
            
        }

        private void FindBuddyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MyBuddiesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private int CalculateNavBarFontSize()
        {
            int fontSize = (int)(NavBar.Width / _FontSizeIncrement + FontSizeModifier);
            return fontSize;
        }
    }
}