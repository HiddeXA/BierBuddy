using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace BierBuddy.UILib
{
    public class DateTimePlannerDialog : Window
    {
        private StackPanel DateTimeSelectors { get; }
        private StackPanel StackPanel { get; }
        public List<List<DateTime>> SelectedDateTimes => DateTimeSelectors.Children.OfType<Grid>().Select(grid => new List<DateTime>
        {
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[0]).SelectedTime,
            ((DatePicker)grid.Children[0]).SelectedDate.Value + ((TimePicker)grid.Children[2]).SelectedTime
        }).ToList();

        public DateTimePlannerDialog()
        {
            this.Title = "My Dialog";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel = new StackPanel { Margin = new Thickness(10) };
            TextBlock textBlock = new TextBlock { Text = "GEEF BESCHIKBAARHEID DOOR:" };
            StackPanel.Children.Add(textBlock);

            DateTimeSelectors = new StackPanel { Orientation = Orientation.Vertical };
            StackPanel.Children.Add(DateTimeSelectors);
            AddDateTimeSelector(DateTime.Now);

            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Button cancelButton = new Button { Content = "CANCEL", IsCancel = true };
            Button okButton = new Button { Content = "GEEF DOOR", IsDefault = true };
            okButton.Click += OK_Click;
            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(okButton);

            StackPanel.Children.Add(buttonPanel);
            this.Content = StackPanel;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to true (OK)
            this.DialogResult = true;
        }

        /// <summary>
        /// Voegt een DateTimeSelector toe aan de DateTimeSelectors stackpanel
        /// </summary>
        /// <param name="dateTime">De datum die standaard geselecteerd is, ook kan er geen datum meer hiervoor gekozen worden</param>
        private void AddDateTimeSelector(DateTime dateTime)
        {
            Grid dateTimeSelector = new Grid();
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            dateTimeSelector.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            DatePicker datePicker = new DatePicker { SelectedDate = dateTime.Date, IsTodayHighlighted = true, DisplayDateStart = dateTime.Date, Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(datePicker, 0);
            dateTimeSelector.Children.Add(datePicker);
            TimePicker timePickerStart = new TimePicker { SelectedTime = dateTime.TimeOfDay, Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(timePickerStart, 1);
            dateTimeSelector.Children.Add(timePickerStart);
            TextBlock textBlock = new TextBlock { Text = "TOT", Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(textBlock, 2);
            dateTimeSelector.Children.Add(textBlock);
            TimePicker timePickerEnd = new TimePicker { SelectedTime = dateTime.AddHours(1).TimeOfDay, Margin = new Thickness(0, 0, 5, 0) };
            Grid.SetColumn(timePickerEnd, 3);
            dateTimeSelector.Children.Add(timePickerEnd);
            DateTimeSelectors.Children.Add(dateTimeSelector);
            CheckSelectors();
        }

        private void CheckSelectors()
        {
            if (DateTimeSelectors.Children.Count > 1)
            {
                if (DateTimeSelectors.Children.Count == 2 && !CheckForLastButtonWithContent("ve"))
                {
                    StackPanel buttonPanel = StackPanel.Children.OfType<StackPanel>().Last();
                    StackPanel.Children.Remove(buttonPanel);
                    StackPanel.Children.Add(new Button { Content = "VERWIJDER LAATSTE DATUM", Margin = new Thickness(0, 5, 0, 0) });
                    StackPanel.Children.OfType<Button>().Last().Click += (sender, e) => {
                        DateTimeSelectors.Children.Remove(DateTimeSelectors.Children.OfType<Grid>().Last());
                        CheckSelectors();
                    };
                    StackPanel.Children.Add(buttonPanel);
                }
                else if (DateTimeSelectors.Children.Count == 6)
                {
                    StackPanel.Children.Remove(StackPanel.Children.OfType<Button>().First());
                }
            }
            else if (DateTimeSelectors.Children.Count == 1)
            {
                if (CheckForLastButtonWithContent("ve"))
                {
                    StackPanel.Children.Remove(StackPanel.Children.OfType<Button>().Last());
                }
                if (!CheckForLastButtonWithContent("vo"))
                {
                    StackPanel buttonPanel = StackPanel.Children.OfType<StackPanel>().Last();
                    if (buttonPanel.Children.OfType<Button>().Any())
                    {
                        StackPanel.Children.Remove(buttonPanel);
                        AddAddDateTimeSelectorButton();
                        StackPanel.Children.Add(buttonPanel);
                    } else
                    {
                        AddAddDateTimeSelectorButton();
                    }
                }
            }
        }

        private bool CheckForLastButtonWithContent(string content)
        {
            return StackPanel.Children.OfType<Button>().Any() && StackPanel.Children.OfType<Button>().Last().Content.ToString().StartsWith(content, StringComparison.CurrentCultureIgnoreCase);
        }

        private void AddAddDateTimeSelectorButton()
        {
            Button addDateTimeSelectorButton = new Button { Content = "VOEG DATUM TOE" };
            addDateTimeSelectorButton.Click += (sender, e) => AddDateTimeSelector(DateTime.Now);
            StackPanel.Children.Add(addDateTimeSelectorButton);
        }
    }
}