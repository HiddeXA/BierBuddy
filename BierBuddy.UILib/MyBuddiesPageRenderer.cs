using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml;
using BierBuddy.Core;
using Material.Icons;
using Material.Icons.WPF;

//TO DO:
//Afspraak maken knop functionaliteit
//Afspraak goedkeuren functionaliteit

namespace BierBuddy.UILib
{
    public class MyBuddiesPageRenderer : IPageRenderer
    {
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private int BigFontSize = 28;
        private int GeneralFontSize = 16;
        private Visitor _Visitor { get; set; }

        public MyBuddiesPageRenderer()
        {
            _Visitor = new Visitor(0, "dummy", "dummy", 0);
        }

        public WrapPanel GetMyBuddiesPage(List<Visitor> buddies)
        {
            WrapPanel myBuddiesPanel = new WrapPanel();

            // Panel voor knoppen
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 10),
            };
            buttonPanel.Children.Add(GetMijnBuddiesButton(300, 50));
            buttonPanel.Children.Add(GetMijnAfsprakenButton(300, 50));

            // Hoofdpanel
            StackPanel mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainPanel.Children.Add(buttonPanel);

            // Buddy-panels dynamisch toevoegen
            double panelWidth = _MainWindowSize.Width - _NavBarWidth - 100;
            foreach (var buddy in buddies)
            {
                BuddyPanel buddyPanel = new BuddyPanel(buddy, _MainWindowSize.Width - _NavBarWidth - 75, 100);
                mainPanel.Children.Add(buddyPanel);
            }

            myBuddiesPanel.Children.Add(mainPanel);
            myBuddiesPanel.VerticalAlignment = VerticalAlignment.Center;
            myBuddiesPanel.HorizontalAlignment = HorizontalAlignment.Center;

            return myBuddiesPanel;
        }

        private UIElement GetMijnBuddiesButton(double width, double height)
        {
            Button MijnBuddiesButton = new()
            {
                Template = GetTitleButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0x35, 0x32)), HorizontalAlignment.Center),
                Content = new TextBlock
                {
                    Text = "Mijn Buddies",
                    FontFamily = new FontFamily("Bayon"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 34,
                    Foreground = UIUtils.BabyPoeder,
                },
                Width = width,
                Height = height
            };

            return MijnBuddiesButton;
        }

        private UIElement GetMijnAfsprakenButton(double width, double height)
        {
            Button MijnAfsprakenButton = new()
            {
                Template = GetTitleButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Center),
                Content = new TextBlock
                {
                    Text = "Mijn afspraken",
                    FontFamily = new FontFamily("Bayon"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 34,
                    Foreground = UIUtils.BabyPoeder,
                },
                Width = width,
                Height = height
            };

            return MijnAfsprakenButton;
        }

        private ControlTemplate GetTitleButtonTemplate(Brush brush, HorizontalAlignment contentAlignment)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, UIUtils.PhantomShip);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(20, 20, 20, 20));
            borderFactory.Name = "BorderElement";
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, contentAlignment);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;
            return template;
        }

        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;

            // Fontsize aanpassen
            if (_MainWindowSize.Width < 1500)
            {
                BigFontSize = 20;
                GeneralFontSize = 12;
            }
            else
            {
                BigFontSize = 28;
                GeneralFontSize = 16;
            }
        }
    }

}
