using BierBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;


namespace BierBuddy.UILib
{
    public class AppointmentPanel : Border
    {
        private Visitor _Visitor;
        private double _Width;
        private double _Height;
        private Appointment _Appointment { get; set; }

        private MyBuddies _MyBuddies { get; set; }

        public AppointmentPanel(Visitor visitor, double width, double height, Appointment appointment, MyBuddies myBuddies)
        {
            _Visitor = visitor;
            _Width = width;
            _Height = height;
            _Appointment = appointment;
            _MyBuddies = myBuddies;

            InitializePanel();
        }

        private void InitializePanel()
        {
            
            string appointmentDate = _Appointment.To.ToString("dd-MM-yyyy");
            //Knip de datum van de DateTime en krijg start-(from) en eindtijd(to)
            string appointmentTime = $"{_Appointment.From.ToShortTimeString()} tot {_Appointment.To.ToShortTimeString()}"; 
            bool status = _Appointment.Accepted; 

            // Stel de grid en layout in
            Grid AppointmentGrid = new Grid();
            AppointmentGrid.Width = _Width;
            AppointmentGrid.Height = _Height;

            // Definieer de kolommen en rijen
            AppointmentGrid.ColumnDefinitions.Add(new ColumnDefinition());
            AppointmentGrid.ColumnDefinitions.Add(new ColumnDefinition());
            AppointmentGrid.RowDefinitions.Add(new RowDefinition());
            AppointmentGrid.RowDefinitions.Add(new RowDefinition());

            // Voeg knoppen toe
            Button appointmentButton = CreateButton(appointmentDate + " - " + appointmentTime, 300, 40);
            Grid.SetColumn(appointmentButton, 0);
            Grid.SetRow(appointmentButton, 0);
            appointmentButton.HorizontalAlignment = HorizontalAlignment.Left;

            Button appointmentStatusButton = CreateStatusButton(status, 300, 40);
            Grid.SetColumn(appointmentStatusButton, 1);
            Grid.SetRow(appointmentStatusButton, 0);
            appointmentStatusButton.HorizontalAlignment = HorizontalAlignment.Right;

            Button buddyNameButton = CreateButton(_MyBuddies.visitorName(_Appointment.VisitorID), 300, 40);
            Grid.SetColumn(buddyNameButton, 0);
            Grid.SetRow(buddyNameButton, 1);
            buddyNameButton.HorizontalAlignment = HorizontalAlignment.Left;

            // Voeg de knoppen toe aan de grid
            AppointmentGrid.Children.Add(appointmentButton);
            AppointmentGrid.Children.Add(appointmentStatusButton);
            AppointmentGrid.Children.Add(buddyNameButton);

            // Stel de border in
            Child = AppointmentGrid;
            Background = UIUtils.Outer_Space;
            CornerRadius = UIUtils.UniversalCornerRadius;
            Margin = new Thickness(10);
        }

        private Button CreateStatusButton(bool status, double width, double height)
        {
            string statusText = status ? "Geaccepteerd" : "Geweigerd";
            Brush statusColor = status ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            Button statusButton = new Button
            {
                Content = new TextBlock
                {
                    Text = statusText,
                    FontFamily = new FontFamily("Bayon"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                Width = width,
                Height = height,
                Margin = new Thickness(10, 0, 10, 0),
                Template = ButtonTemplate(statusColor, HorizontalAlignment.Center),
            };

            return statusButton;
        }

        private Button CreateButton(string text, double width, double height)
        {
            Button button = new Button
            {
                Content = new TextBlock
                {
                    Text = text,
                    FontFamily = new FontFamily("Bayon"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(10, 0, 0, 0)
                },
                Width = width,
                Height = height,
                Margin = new Thickness(10, 0, 10, 0),
                Template = ButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Left),
            };
            return button;
        }

        private ControlTemplate ButtonTemplate(Brush brush, HorizontalAlignment contentAlignment)
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
            return template;
        }
    }
}

