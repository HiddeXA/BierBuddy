using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BierBuddy.UILib
{
    internal class TimePicker : Grid
    {
        public TimeSpan SelectedTime { get; set; }
        public event EventHandler<TimeSpan>? TimeChanged;

        public TimePicker()
        {
            Background = UIUtils.Onyx;
            SelectedTime = DateTime.Now.TimeOfDay;
            SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, 0);

            ComboBox hours = new ComboBox { Margin = new Thickness(0, 0, 5, 0) };
            ComboBox minutes = new ComboBox { Margin = new Thickness(0, 0, 5, 0) };
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

            TextBlock seperator = new TextBlock { Text = ":", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 5, 0) };

            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            SetColumn(hours, 0);
            SetColumn(seperator, 1);
            SetColumn(minutes, 2);
            Children.Add(hours);
            Children.Add(seperator);
            Children.Add(minutes);
        }

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
