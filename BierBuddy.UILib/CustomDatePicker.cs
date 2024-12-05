using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BierBuddy.UILib
{
    internal class CustomDatePicker : DatePicker
    {
        //deze klasse is gemaakt om de DatePicker een custom style te geven
        public CustomDatePicker()
        {
            Loaded += CustomDatePicker_Loaded;
        }

        private void CustomDatePicker_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplyTemplate();

            //vind de juiste elementen en maak de box mooi
            //de textbox is de box waar de datum in staat
            if (Template.FindName("PART_TextBox", this) is TextBox textBox)
            {
                textBox.Background = Background;
                textBox.Foreground = Foreground;
                textBox.BorderBrush = Background;
                textBox.BorderThickness = new Thickness(0);
                textBox.FontFamily = FontFamily;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
            }
            //de button is de knop waar je op klikt om de kalender te openen
            if (Template.FindName("PART_Button", this) is Button button)
            {
                button.Background = Background;
                button.BorderBrush = Background;
            }
            //de popup is de kalender, deze moet een andere achtergrond hebben omdat de text niet aanpasbaar is zonder extreem veel extra werk
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
