using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Material.Icons.WPF;
using Material.Icons;
using System.Windows.Controls.Primitives;

namespace BierBuddy.UILib
{
    internal class CustomDatePicker : DatePicker
    {
        public CustomDatePicker()
        {
            Loaded += CustomDatePicker_Loaded;
        }

        private void CustomDatePicker_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Ensure the template is applied
            ApplyTemplate();

            if (Template.FindName("PART_TextBox", this) is TextBox textBox)
            {
                textBox.Background = Background;
                textBox.Foreground = Foreground;
                textBox.BorderBrush = Background;
                textBox.BorderThickness = new Thickness(0);
                textBox.FontFamily = FontFamily;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
            }
            if (Template.FindName("PART_Button", this) is Button button)
            {
                button.Background = Background;
                button.BorderBrush = Background;
            }
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                if(popup.Child is Calendar calendar)
                {
                    calendar.Background = Foreground;
                }
            }
        }
    }
}
