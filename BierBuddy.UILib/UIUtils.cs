using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BierBuddy.UILib
{
    static internal class UIUtils
    {
        internal static readonly Brush BabyPoeder = new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0xFF, 0xF7));
        internal static readonly Brush Onyx = new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0x35, 0x32));
        internal static readonly Brush Onyx70 = new SolidColorBrush(Color.FromArgb(178, 0x2E, 0x35, 0x32));
        internal static readonly FontFamily ÜniversalFontFamily = new FontFamily("Bayon");
        internal static readonly FontWeight UniversalFontWeight = new FontWeight();
        internal static readonly CornerRadius UniversalCornerRadius = new CornerRadius(40);
    }
}
