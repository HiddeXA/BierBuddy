﻿using BierBuddy.UILib;
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
using BierBuddy.Core;
using BierBuddy.DataAccess;

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

        private readonly int _MinFontSize = 24;
        private readonly int _FontSizeIncrement = 30;
        private int _FontSizeModifier { get; }

        // Gebruikt voor het dynamisch resizen van de app met de gerenderde pagina.
        private int WindowStatus { get; set; } = 0;

        //definitie pageRenderers
        private Main _Main { get; }
        private FindBuddiesPageRenderer _FindBuddiesPageRenderer { get; }
        private FindBuddies _FindBuddies { get; }
        private IDataAccess _DataAccess { get; }
        private MyBuddies _MyBuddies { get; }
        private MyBuddiesPageRenderer _MyBuddiesPageRenderer { get;  }
        private AlgoritmePlaceHolder _AlgoritmePlaceHolder { get; }

        public MainWindow()
        {
            InitializeComponent();
            //initialize dataAccess
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=BierBuddy;user=root;port=3306;password=");
            connection.Open();
            _DataAccess = new MySQLDatabase(connection);
            //initialize main
            _Main = new Main(_DataAccess);
            //initialize page renderers
            _MyBuddies = new MyBuddies(_DataAccess, _Main);
            _MyBuddiesPageRenderer = new MyBuddiesPageRenderer(_MyBuddies); 
            _AlgoritmePlaceHolder = new AlgoritmePlaceHolder();

            //initialize page renderers
            _FindBuddies = new FindBuddies(_DataAccess, _Main);
            _FindBuddiesPageRenderer = new FindBuddiesPageRenderer(_FindBuddies);

            _FontSizeModifier = _MinFontSize - _NavBarMinSize / _FontSizeIncrement;
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
                MoveBeerFoam(e);
                int fontSize = CalculateNavBarFontSize();
                FindBuddiesLabel.FontSize = fontSize;
                MyBuddiesLabel.FontSize = fontSize;
                NavBar.Width = _NavBarMinSize;
            }

            MoveBeerFoam(e);
            _FindBuddiesPageRenderer.UpdatePageSize(NavBar.Width, e.NewSize);
            _MyBuddiesPageRenderer.UpdatePageSize(NavBar.Width, e.NewSize);

            if (WindowStatus == 1)
            {
                FindBuddyButton_Click(sender, e);
            }
            else if (WindowStatus == 2)
            {
                MyBuddiesButton_Click(sender, e);
            }
            else if (WindowStatus == 3)
            {
                SettingsButton_Click(sender, e);
            }
            else if (WindowStatus == 4)
            {
                AccountButton_Click(sender, e);
            }
            else { }

            

        }

        private void FindBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStatus = 1;
            PagePanel.Children.Clear();
            PagePanel.Children.Add(_FindBuddiesPageRenderer.GetFindBuddiesPage(_FindBuddies.GetPotentialMatch()));
            
        }

        
        private void MyBuddiesButton_Click(object sender, RoutedEventArgs e)
        {
            List<Visitor> buddyList = new List<Visitor>
        {
             new Visitor(1, "Rick", "Test", 25),
             new Visitor(2, "Martijn", "Test", 30),
             new Visitor(3, "Yannick", "Test", 28),

        };
            this.WindowStatus = 2;
            PagePanel.Children.Clear();
            PagePanel.Children.Add(_MyBuddiesPageRenderer.GetMyBuddiesPage(buddyList));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStatus = 3;
            PagePanel.Children.Clear();
            //todo wanneer er settings komen
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {

            //todo account aanpassen screen
            //todo aanpassen account switchen en authenticatie enzo

            //dit is een tijdelijke oplossing
            #region
            PagePanel.Children.Clear();

            ProfilePageRenderer profilePage = new ProfilePageRenderer();
            PagePanel.Children.Add(profilePage.GetProfilePage(_Main.ClientVisitor, false));
            #endregion
        }

        private int CalculateNavBarFontSize()
        {
            int fontSize = (int)(NavBar.Width / _FontSizeIncrement + _FontSizeModifier);
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