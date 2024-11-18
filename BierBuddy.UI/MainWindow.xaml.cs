using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BierBuddy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int NavBarSize { get; set; } = 400;
        private readonly double PreferedSizeNavBar = 20 / 100;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BierBuddyMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            NavBarSize = (int)(e.NewSize.Width * PreferedSizeNavBar);
        }
    }

}