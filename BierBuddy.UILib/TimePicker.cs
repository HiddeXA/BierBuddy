﻿using System.Windows;
using System.Windows.Controls;

namespace BierBuddy.UILib
{
    internal class TimePicker : Border
    {
        public TimeSpan SelectedTime { get; private set; }
        public event EventHandler<TimeSpan>? TimeChanged;

        public TimePicker()
        {
            Background = UIUtils.Outer_Space;
            SelectedTime = DateTime.Now.TimeOfDay;
            SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, 0); //haal de secondes weg
            CornerRadius = UIUtils.UniversalCornerRadius;
            Padding = new Thickness(10, 0, 10, 0);
            Grid grid = new Grid();

            //maak de comboboxes, voeg de items toe en selecteer de juiste tijd
            CustomComboBox hours = new CustomComboBox { Margin = new Thickness(0, 0, 5, 0) };
            CustomComboBox minutes = new CustomComboBox { Margin = new Thickness(0, 0, 5, 0) };
            for (int i = 0; i < 24; i++)
            {
                hours.Items.Add(i.ToString("D2"));
            }
            for (int i = 0; i < 60; i++)
            {
                minutes.Items.Add(i.ToString("D2"));
            }
            hours.SelectedItem = SelectedTime.Hours.ToString("D2");
            minutes.SelectedItem = SelectedTime.Minutes.ToString("D2");
            hours.SelectionChanged += Hours_SelectionChanged;
            minutes.SelectionChanged += Minutes_SelectionChanged;
            hours.Background = UIUtils.Transparent;
            minutes.Background = UIUtils.Transparent;
            hours.Foreground = UIUtils.BabyPoeder;
            minutes.Foreground = UIUtils.BabyPoeder;

            TextBlock seperator = new TextBlock { Text = ":", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 0), Foreground = UIUtils.BabyPoeder, FontFamily = UIUtils.UniversalFontFamily, FontWeight = FontWeights.Bold };

            //maak de grid en voeg de items toe
            //uur : minuut
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            Grid.SetColumn(hours, 0);
            Grid.SetColumn(seperator, 1);
            Grid.SetColumn(minutes, 2);
            grid.Children.Add(hours);
            grid.Children.Add(seperator);
            grid.Children.Add(minutes);
            Child = grid;
        }

        //deze functies zijn apart omdat bij de een de uur aangepast moet worden en bij de ander de minuut
        private void Hours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTime = new TimeSpan(int.Parse((string)((ComboBox)sender).SelectedItem), SelectedTime.Minutes, 0);
            TimeChanged?.Invoke(this, SelectedTime);
        }

        private void Minutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTime = new TimeSpan(SelectedTime.Hours, int.Parse((string)((ComboBox)sender).SelectedItem), 0);
            TimeChanged?.Invoke(this, SelectedTime);
        }
    }
}
