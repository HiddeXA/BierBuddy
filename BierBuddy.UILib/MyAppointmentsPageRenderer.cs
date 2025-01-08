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
        private int BigFontSize;
        private int GeneralFontSize;
        private Visitor _Visitor { get; set; }

        private MyBuddies _MyBuddies { get; set; }


        public MyAppointmentsPageRenderer(Appointment appointment, MyBuddies myBuddies)
        {
            _Visitor = new Visitor(0, "dummy", "dummy", 0);

            _MyBuddies = myBuddies;
        }

        public WrapPanel GetMyAppointmentsPage(List<Appointment> appointments)
        {
            WrapPanel myAppointmentsPanel = new WrapPanel();

            // Scrollviewer toevoegen
            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Height = _MainWindowSize.Height - 100,
                Margin = new Thickness(10)
            };

            // Hoofdpanel
            StackPanel mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Panel voor knoppen
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 10),
            };
            buttonPanel.Children.Add(GetMijnAfsprakenButton(300, 50));
            mainPanel.Children.Add(buttonPanel);

            // Appointment-panels dynamisch toevoegen
            double panelWidth = _MainWindowSize.Width - _NavBarWidth - 100;

            foreach (Appointment appointment in appointments)
            {
                AppointmentPanel appointmentPanel = new AppointmentPanel(_Visitor, _MainWindowSize.Width - _NavBarWidth - 75, 100, appointment, _MyBuddies);
                mainPanel.Children.Add(appointmentPanel);
            }

            // Voeg de mainPanel toe aan de ScrollViewer
            scrollViewer.Content = mainPanel;

            // Voeg de ScrollViewer toe aan het WrapPanel
            myAppointmentsPanel.Children.Add(scrollViewer);

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

