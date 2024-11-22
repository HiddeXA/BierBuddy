using BierBuddy.UILib;
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
using Material.Icons.WPF;

namespace BierBuddy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //deel dat dat de navBar opneemt
        private readonly double _SizeModifierNavBar = 0.25;
        private readonly int _NavBarMinSize = 290;

        private readonly int _MinFontSize = 26;
        private readonly int _DefaultFontSize = 32;
        private readonly int _FontSizeIncrement = 30;
        private int FontSizeModifier { get; }

        //definitie pageRenderers
        private FindBuddiesPageRenderer FindBuddiesPageRenderer { get; }

        public MainWindow()
        {
            InitializeComponent();
            //initialize page renderers
            FindBuddiesPageRenderer = new FindBuddiesPageRenderer();


            FontSizeModifier = _MinFontSize - _NavBarMinSize / _FontSizeIncrement;
        }
        private void BierBuddyMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //pas alleen de navBar size aan als deze niet kleiner zal zijn dan de minimum size
            if (BBMainWindow.ActualWidth * _SizeModifierNavBar > _NavBarMinSize)
            {
                //verander de NavBar size
                NavBar.Width = BBMainWindow.ActualWidth * _SizeModifierNavBar;

                MoveBeerFoam(e);
                //pas fontsize van de buttons aan zodat ze niet onder het icoontje komen te staan
                int fontSize = CalculateNavBarFontSize();
                FindBuddiesLabel.FontSize = fontSize;
                MyBuddiesLabel.FontSize = fontSize;
            }
            else
            {
                NavBar.Width = _NavBarMinSize;
            }
            FindBuddiesPageRenderer.UpdatePageSize(NavBar.Width, e.NewSize.Width);
        }

        private void FindBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            PagePanel.Children.Clear();
            PagePanel.Children.Add(FindBuddiesPageRenderer.GetFindBuddiesPage(NavBar.Width, BBMainWindow.Width, BBMainWindow.Height));
            
        }

        private void MyBuddiesButton_Click(object sender, RoutedEventArgs e)
        {
            PagePanel.Children.Clear();
            //todo voor mijn buddies userstory
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            PagePanel.Children.Clear();
            //todo wanneer er settings komen
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            PagePanel.Children.Clear();
            //todo voor account userstories
        }

        private int CalculateNavBarFontSize()
        {
            int fontSize = (int)(NavBar.Width / _FontSizeIncrement + FontSizeModifier);
            return fontSize;
        }
        private void MoveBeerFoam(SizeChangedEventArgs e)
        {
            //zet het bierschuim vast relatief aan de rechtekant van de NavBar
            Canvas.SetLeft(EllipseFoam0, NavBar.Width - 130);
            Canvas.SetLeft(EllipseFoam1, NavBar.Width - 218);
            Canvas.SetLeft(EllipseFoam2, NavBar.Width - 300);
            Canvas.SetLeft(EllipseFoam3, NavBar.Width - 365);
            Canvas.SetLeft(EllipseFoam4, NavBar.Width - 412);
        }
    }
}