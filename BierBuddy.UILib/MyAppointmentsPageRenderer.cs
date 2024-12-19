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
using BierBuddy.DataAccess;
using Material.Icons;
using Material.Icons.WPF;

namespace BierBuddy.UILib
{
    public class MyAppointmentsPageRenderer
    {
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private int BigFontSize = 28;
        private int GeneralFontSize = 16;
        private Visitor _Visitor { get; set; }

        private Appointment _Appointment { get; set; }

        private MyBuddies _MyBuddies { get; set; }

        private MySQLDatabase _MySQLDatabase;

        public MyAppointmentsPageRenderer()
        {
            _Visitor = new Visitor(0, "dummy", "dummy", 0);

            //_Appointment = appointment;
        }

        public WrapPanel GetMyAppointmentsPage()
        {
            WrapPanel myAppointmentsPanel = new WrapPanel();

            // Panel voor knoppen
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 10),
            };

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
            //foreach (var buddy in buddies)
            //{
            //    BuddyPanel buddyPanel = new BuddyPanel(buddy, _MainWindowSize.Width - _NavBarWidth - 75, 100, _MyBuddies);
            //    mainPanel.Children.Add(buddyPanel);
            //}

            AppointmentPanel appointmentPanel = new AppointmentPanel(_Visitor, _MainWindowSize.Width - _NavBarWidth - 75, 100, _Appointment);
            mainPanel.Children.Add(appointmentPanel);

            myAppointmentsPanel.Children.Add(mainPanel);
            myAppointmentsPanel.VerticalAlignment = VerticalAlignment.Center;
            myAppointmentsPanel.HorizontalAlignment = HorizontalAlignment.Center;

            return myAppointmentsPanel;
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

