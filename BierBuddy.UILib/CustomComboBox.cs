using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.Intrinsics.X86;

namespace BierBuddy.UILib
{
    internal class CustomComboBox : ComboBox
    {
        public CustomComboBox()
        {
            Loaded += MyComboBox_Loaded;
        }

        private void MyComboBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
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
