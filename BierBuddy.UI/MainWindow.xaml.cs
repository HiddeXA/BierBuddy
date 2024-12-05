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
        private FindBuddiesPageRenderer _FindBuddiesPageRenderer { get; }
        private MyBuddiesPageRenderer _MyBuddiesPageRenderer { get; }
        private AlgoritmePlaceHolder _AlgoritmePlaceHolder { get; }


        public MainWindow()
        {
            InitializeComponent();
            //initialize page renderers
            _FindBuddiesPageRenderer = new FindBuddiesPageRenderer();
            _MyBuddiesPageRenderer = new MyBuddiesPageRenderer(); 
            _AlgoritmePlaceHolder = new AlgoritmePlaceHolder();


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
            PagePanel.Children.Add(_FindBuddiesPageRenderer.GetFindBuddiesPage(_AlgoritmePlaceHolder.GetVisitor()));
            
        }

        private void MyBuddiesButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStatus = 2;
            PagePanel.Children.Clear();
            PagePanel.Children.Add(_MyBuddiesPageRenderer.GetMyBuddiesPage(_AlgoritmePlaceHolder.GetVisitor()));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStatus = 3;
            PagePanel.Children.Clear();
            //todo wanneer er settings komen
        }
        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStatus = 4;
            PagePanel.Children.Clear();
            //todo voor account userstories
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