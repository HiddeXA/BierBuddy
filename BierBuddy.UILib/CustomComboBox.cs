using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace BierBuddy.UILib
{
    internal class CustomComboBox : ComboBox
    {
        //deze klasse is gemaakt om de comboboxen in de TimePicker een custom style te geven
        public CustomComboBox()
        {
            Loaded += MyComboBox_Loaded;
        }

        private void MyComboBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //vind de juiste elementen en maak de box mooi
            if (Template.FindName("toggleButton", this) is ToggleButton toggleButton)
            {
                if (toggleButton.Template.FindName("templateRoot", toggleButton) is Border border)
                {
                    border.Background = Background;
                    border.BorderBrush = Background;
                }
                toggleButton.Foreground = Foreground;
            }
            SetItemContainerStyle();
        }

        //omdat de dropdown de textkleur van de box pakt, moet deze apart gezet worden
        private void SetItemContainerStyle() {
            Style style = new Style(typeof(ComboBoxItem));
            style.Setters.Add(new Setter(ForegroundProperty, Brushes.Black));
            style.Setters.Add(new Setter(FontFamilyProperty, FontFamily));
            style.Setters.Add(new Setter(FontWeightProperty, FontWeight));
            style.Setters.Add(new Setter(FontSizeProperty, FontSize));

            this.ItemContainerStyle = style;
        }
    }
}
