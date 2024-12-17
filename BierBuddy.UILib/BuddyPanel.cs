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
    public class BuddyPanel : Border
    {
        private Visitor _Visitor;
        private double _Width;
        private double _Height;
        private MyBuddies _MyBuddies { get; set; }

        public BuddyPanel(Visitor visitor, double width, double height, MyBuddies myBuddies)
        {
            _Visitor = visitor;
            _Width = width;
            _Height = height;
            _MyBuddies = myBuddies;

            InitializePanel();
        }

        private void InitializePanel()
        {
            // Stel de grid en layout in
            Grid buddyGrid = new Grid();
            buddyGrid.Width = _Width;
            buddyGrid.Height = _Height;

            // Definieer de kolommen en rijen
            buddyGrid.ColumnDefinitions.Add(new ColumnDefinition());
            buddyGrid.ColumnDefinitions.Add(new ColumnDefinition());
            buddyGrid.RowDefinitions.Add(new RowDefinition());
            buddyGrid.RowDefinitions.Add(new RowDefinition());

            // Voeg knoppen toe
            Button appointmentButton = CreateButton("Maak afspraak", 300, 40);
            Grid.SetColumn(appointmentButton, 1);
            Grid.SetRow(appointmentButton, 0);
            appointmentButton.HorizontalAlignment = HorizontalAlignment.Right;
            appointmentButton.Click += AppointmentButton_Click;

            Button appointmentAcceptButton = CreateButton("Afspraak goedkeuren", 300, 40);
            Grid.SetColumn(appointmentAcceptButton, 1);
            Grid.SetRow(appointmentAcceptButton, 1);
            appointmentAcceptButton.HorizontalAlignment = HorizontalAlignment.Right;
            appointmentAcceptButton.Click += AppointmentAcceptButton_Click;

            Button buddyNameButton = CreateNameButton(_Visitor.Name, 300, 40);
            Grid.SetColumn(buddyNameButton, 0);
            Grid.SetRow(buddyNameButton, 0);
            buddyNameButton.HorizontalAlignment = HorizontalAlignment.Center;

            // Voeg de knoppen toe aan de grid
            buddyGrid.Children.Add(appointmentButton);
            buddyGrid.Children.Add(appointmentAcceptButton);
            buddyGrid.Children.Add(buddyNameButton);

            // Stel de border in
            Child = buddyGrid;
            Background = UIUtils.Outer_Space;
            CornerRadius = UIUtils.UniversalCornerRadius;
            Margin = new Thickness(10);
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
                    Foreground = Brushes.Black
                },
                Width = width,
                Height = height,
                Margin = new Thickness(10, 0, 10, 0),
                Template = ButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)), HorizontalAlignment.Center),
            };
            return button;
        }

        private Button CreateNameButton(string text, double width, double height)
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

            Trigger mouseOverTrigger = new Trigger
            {
                Property = UIElement.IsMouseOverProperty,
                Value = true
            };
            mouseOverTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0xB3, 0xFC, 0xFF, 0xF7)), "BorderElement"));
            template.Triggers.Add(mouseOverTrigger);

            template.VisualTree = gridFactory;
            return template;
        }

        private void AppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            DateTimePlannerDialog dialog = new DateTimePlannerDialog();
            if (dialog.ShowDialog() == true)
            {
                _MyBuddies.AddAppointments(_Visitor, dialog.SelectedDateTimes);
            }
        }

        private void AppointmentAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            List<Appointment> appointments = _MyBuddies.GetAppointments(_Visitor).Where(app => !app.Accepted).ToList();
            if (appointments.Count == 0)
            {
                return;
            }
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
    }
}
