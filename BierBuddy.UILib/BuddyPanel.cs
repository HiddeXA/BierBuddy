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
        private Visitor _visitor;
        private double _width;
        private double _height;

        public BuddyPanel(Visitor visitor, double width, double height)
        {
            _visitor = visitor;
            _width = width;
            _height = height;

            InitializePanel();
        }

        private void InitializePanel()
        {
            // Stel de grid en layout in
            Grid buddyGrid = new Grid();
            buddyGrid.Width = _width;
            buddyGrid.Height = _height;

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

            Button appointmentAcceptButton = CreateButton("Afspraak goedkeuren", 300, 40);
            Grid.SetColumn(appointmentAcceptButton, 1);
            Grid.SetRow(appointmentAcceptButton, 1);
            appointmentAcceptButton.HorizontalAlignment = HorizontalAlignment.Right;

            Button buddyNameButton = CreateNameButton(_visitor.Name, 300, 40);
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
                    Foreground = Brushes.Black
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
    }
}
