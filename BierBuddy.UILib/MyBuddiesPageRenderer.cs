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

//TO DO:
//Afspraak maken knop fucntionaliteit
//Afspraak goedkeuren functionaliteit

namespace BierBuddy.UILib
{
    public class MyBuddiesPageRenderer : IPageRenderer
    {
        
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private Canvas _buddyPanel;

        private int BigFontSize = 28;
        private int GeneralFontSize = 16;

        private readonly int _MinFontSizeBig = 20;
        private readonly int _MinFontSizeGeneral = 12;

        private Visitor _Visitor { get; set; }

        private MyBuddies _MyBuddies { get; set; }
        private MySQLDatabase _MySQLDatabase;


        public MyBuddiesPageRenderer(MyBuddies myBuddies)
        {
            _buddyPanel = new Canvas();
            _Visitor = new(0, "dummy", "dummy", 0);

            _MyBuddies = myBuddies;
        }

        public WrapPanel GetMyBuddiesPage(Visitor visitor)
        {
            WrapPanel MyBuddiesPanel = new();
            double panelWidth = _MainWindowSize.Width - _NavBarWidth;
            _Visitor = visitor;

            // Maak een panel voor de buttons Mijn Buddies en Mijn Afspraken
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 10),
            };


            buttonPanel.Children.Add(GetMijnBuddiesButton(300, 50));
            buttonPanel.Children.Add(GetMijnAfsprakenButton(300, 50));
            
            SetBuddyPanel(1000, 100); 
            Border buddyBorder = (Border)GetBuddyBorder();
            buddyBorder.Margin = new Thickness(45, 20, 0, 20);
            

            // StackPanel voor de knoppen en de buddies
            StackPanel mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            
            mainPanel.Children.Add(buttonPanel);  
            mainPanel.Children.Add(buddyBorder);


            MyBuddiesPanel.Children.Add(mainPanel);  

            MyBuddiesPanel.VerticalAlignment = VerticalAlignment.Center;
            MyBuddiesPanel.HorizontalAlignment = HorizontalAlignment.Center;

            return MyBuddiesPanel;
        }

        private UIElement GetBuddyBorder()
        {
            // Maak een nieuw Canvas aan om Child issues te voorkomen 
            Canvas newBuddyPanel = new Canvas
            {
                Width = _MainWindowSize.Width - _NavBarWidth - 100,  
                Height = _buddyPanel.Height,  
                Margin = _buddyPanel.Margin  
            };

            
            UIElement buddyContent = GetBuddyContentPanel(newBuddyPanel.Width, newBuddyPanel.Height);
            newBuddyPanel.Children.Add(buddyContent);

            Border buddyBorder = new Border
            {
                Background = UIUtils.Outer_Space,
                CornerRadius = UIUtils.UniversalCornerRadius,
                Child = newBuddyPanel,  
            };

            return buddyBorder;
        }

        private void SetBuddyPanel(double width, double height)
        {
            _buddyPanel.Width = width;
            _buddyPanel.Height = height;
            _buddyPanel.Margin = new Thickness(10);

            UIElement buddyContent = GetBuddyContentPanel(width, height);
            _buddyPanel.Children.Add(buddyContent);
        }


        private UIElement GetAppointmentButton(double width, double height) 
        {           
            Button AppointmentButton = new();
            AppointmentButton.Template = GetButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Center);
            AppointmentButton.Content = new TextBlock
            {
                Text = "Maak afspraak",
                FontFamily = new FontFamily("Bayon"), 
                FontWeight = FontWeights.Bold,
                FontSize = 20, 
            };

            AppointmentButton.Width = width;
            AppointmentButton.Height = height;

            AppointmentButton.Click += AppointmentButton_Click;
            
            return AppointmentButton;
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            DateTimePlannerDialog dialog = new DateTimePlannerDialog();
            if (dialog.ShowDialog() == true)
            {
                _MyBuddies.AddAppointments(_Visitor, dialog.SelectedDateTimes);
            }
        }

        private UIElement GetAppointmentAcceptButton(double width, double height)
        {            
            Button AppointmentAcceptButton = new();
            AppointmentAcceptButton.Template = GetButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Center);
            AppointmentAcceptButton.Content = new TextBlock
            {
                Text = "Afspraak goedkeuren",
                FontFamily = new FontFamily("Bayon"),
                FontWeight = FontWeights.Bold,
                FontSize = 20,
            };

            AppointmentAcceptButton.Width = width;
            AppointmentAcceptButton.Height = height; 
            
            AppointmentAcceptButton.Click += AppointmentAcceptButton_Click;
           
            return AppointmentAcceptButton;
        }

        private void AppointmentAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> appointments = _MyBuddies.GetAppointments(_Visitor);
            List<List<DateTime>> dateTimes = new List<List<DateTime>>();
            foreach (Appointment appointment in appointments)
            {
                List<DateTime> dateTime = new List<DateTime> { appointment.From, appointment.To };
                dateTimes.Add(dateTime);
            }
            DateTimeAccepterDialog dialog = new DateTimeAccepterDialog(dateTimes);
            if (dialog.ShowDialog() == true)
            {
                List<bool> result = dialog.AcceptedNotAcceptedDateTimes;
                for (int i = 0; i < result.Count; i++)
                {
                    _MyBuddies.HandleAppointment(appointments[i], result[i]);
                }
            }
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

        private UIElement GetBuddyNameButton(double width, double height)
        {

            Button BuddyNameButton = new();
            BuddyNameButton.Template = GetButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Left);

            ProfileContentLabel nameLabel = new ProfileContentLabel(_Visitor.Name, BigFontSize)
            {
                FontFamily = new FontFamily("Bayon"),
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Black),
                FontWeight = FontWeights.Bold,
            };

            BuddyNameButton.Width = width;
            BuddyNameButton.Height = height;

            BuddyNameButton.Content = nameLabel;

            //TO DO:
            //BuddyNameButton.Click += BuddyNameButton_Click;

            return BuddyNameButton;
        }


        private ControlTemplate GetButtonTemplate(Brush brush, HorizontalAlignment contentAlignment)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            
            borderFactory.SetValue(Border.BackgroundProperty, UIUtils.BabyPoeder);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(20, 20, 20, 20));
            borderFactory.Name = "BorderElement";
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, contentAlignment);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;

            //Wijzig de kleur van de button bij hovering
            Trigger mouseOverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };         
            mouseOverTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xB3, 0xFC, 0xFF, 0xF7)), "BorderElement")); 
            template.Triggers.Add(mouseOverTrigger);

            return template;
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

        private UIElement GetBuddyContentPanel(double width, double height)
        {
            // Create the Grid
            Grid buddyGrid = new Grid();
            buddyGrid.Width = width;
            buddyGrid.Height = height;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            buddyGrid.ColumnDefinitions.Add(colDef1);
            buddyGrid.ColumnDefinitions.Add(colDef2);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            buddyGrid.RowDefinitions.Add(rowDef1);
            buddyGrid.RowDefinitions.Add(rowDef2);

            Button AppointmentButton = GetAppointmentButton(300, 40) as Button;           
            Grid.SetColumn(AppointmentButton, 1); 
            Grid.SetRow(AppointmentButton, 0);    
            AppointmentButton.HorizontalAlignment = HorizontalAlignment.Right; 
            
            Button AppoinmentAcceptButton = GetAppointmentAcceptButton(300, 40) as Button;
            Grid.SetColumn(AppoinmentAcceptButton, 1);
            Grid.SetRow(AppoinmentAcceptButton, 1);
            AppoinmentAcceptButton.HorizontalAlignment = HorizontalAlignment.Right;

            Button BuddyNameButton = GetBuddyNameButton(300, 40) as Button;
            Grid.SetColumn(BuddyNameButton, 0);
            Grid.SetRow(BuddyNameButton, 0);
            BuddyNameButton.HorizontalAlignment = HorizontalAlignment.Center;

            buddyGrid.Children.Add(AppointmentButton);
            buddyGrid.Children.Add(AppoinmentAcceptButton);
            buddyGrid.Children.Add(BuddyNameButton);

            Border buddyContentBorder = new Border
            {
                Child = buddyGrid
            };

            return buddyContentBorder;
        }

        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;

            // Fontsize aanpassen
            if (_MainWindowSize.Width < 1500)
            {
                BigFontSize = _MinFontSizeBig;
                GeneralFontSize = _MinFontSizeGeneral;
            }
            else
            {
                BigFontSize = 28;
                GeneralFontSize = 16;
            }
        }


 

    }
}
