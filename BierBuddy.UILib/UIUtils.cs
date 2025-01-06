using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

namespace BierBuddy.UILib
{
    static internal class UIUtils
    {
        internal static readonly double ProfileConentHeight = 300;

        internal static readonly Brush AcceptGreen = new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72));
        internal static readonly Brush BabyPoeder = new SolidColorBrush(Color.FromArgb(0xFF, 0xFC, 0xFF, 0xF7));
        internal static readonly Brush DeclineRed = new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32));
        internal static readonly Brush Onyx = new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0x35, 0x32));
        internal static readonly Brush Onyx70 = new SolidColorBrush(Color.FromArgb(178, 0x2E, 0x35, 0x32));
        internal static readonly Brush Outer_Space = new SolidColorBrush(Color.FromArgb(0xFF, 0x43, 0x4D, 0x49));
        internal static readonly Brush testMarking = new SolidColorBrush(Color.FromRgb( 255, 255, 0));
        internal static readonly Brush PhantomShip = new SolidColorBrush(Color.FromArgb(255, 46, 53, 50));
        internal static readonly Brush BeerYellow = new SolidColorBrush(Color.FromArgb(255, 0xF1, 0x9F, 0x2C));
        internal static readonly Brush Transparent = new SolidColorBrush(Colors.Transparent);
        internal static readonly FontFamily UniversalFontFamily = new FontFamily("Bayon");
        internal static readonly FontWeight UniversalFontWeight = new FontWeight();
        internal static readonly CornerRadius UniversalCornerRadius = new CornerRadius(25);
        internal static readonly CornerRadius SquirqilCornerRadius = new CornerRadius(15);
        public static BitmapImage ConvertByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return null;
            }

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(byteArray))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // belangrijk voor performance
                    image.StreamSource = memoryStream;
                    image.EndInit();
                    image.Freeze(); // belangrijk voor cross-thread access
                    return image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting byte array to image: {ex.Message}");
                return null;
            }
        }
    }
}
