using BierBuddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BierBuddy.UILib
{
    /// <summary>
    /// Interaction logic for SwitchAccountDialog.xaml
    /// </summary>
    public partial class SwitchAccountDialog : Window
    {
        private readonly Main _Main;
        private Popup _Popup;
        private TextBlock _Message;


        public SwitchAccountDialog(Main main)
        {
            InitializeComponent();
            _Popup = new Popup
            {
                Placement = PlacementMode.Mouse,
                StaysOpen = false,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade
            };
            _Message = new TextBlock
            {
                FontSize = 16,
                Foreground = UIUtils.BabyPoeder,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            InitilizePopUp();
            _Main = main;
        }

        private void InitilizePopUp()
        {
            Border border = new Border
            {
                Background = UIUtils.Onyx,
                BorderBrush = UIUtils.BabyPoeder,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(5)
            };

            StackPanel contentPanel = new StackPanel();
            

            contentPanel.Children.Add(_Message);
            border.Child = contentPanel;
            _Popup.Child = border;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Main.AccountSwitcher.SwitchClientProfile(NewAccountID_Text.Text);
                this.Close();
            }
            catch (ArgumentOutOfRangeException)
            {
                _Message.Text = "AccountID bestaat niet";
                _Popup.IsOpen = true;
            }
            catch (Exception) 
            {
                _Message.Text = "invoer niet correct";
                _Popup.IsOpen = true;
            }
        }
    }
}
