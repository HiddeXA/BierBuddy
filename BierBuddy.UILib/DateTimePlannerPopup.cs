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
        public DateTimePlannerDialog()
        {
            this.Title = "My Dialog";
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel stackPanel = new StackPanel { Margin = new Thickness(10) };
            TextBlock textBlock = new TextBlock { Text = "This is my custom dialog box!" };
            stackPanel.Children.Add(textBlock);

            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };
            Button okButton = new Button { Content = "OK", IsDefault = true };
            okButton.Click += OK_Click;
            Button cancelButton = new Button { Content = "Cancel", IsCancel = true };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(buttonPanel);
            this.Content = stackPanel;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to true (OK)
            this.DialogResult = true;
        }
    }
}