using BierBuddy.Core;
using BierBuddy.DataAccess;
using BierBuddy.UILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BierBuddy.Core;
using BierBuddy.DataAccess;

namespace BierBuddy.UI
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private IDataAccess _DataAccess { get; }
        public LoginScreen()
        {
            InitializeComponent();
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=BierBuddy;user=root;port=3306;password=");
            connection.Open();
            _DataAccess = new MySQLDatabase(connection);

        }

        private void Loginclick(object sender, RoutedEventArgs e)
        {
            string email = EmailInput.Text;
            string passkey = PasskeyInput.Password;
            Visitor ?user = _DataAccess.GetAccount(email, passkey);
            if (user != null)
            {
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Email of wachtwoord is onjuist");
            }
        }

        private void RegitreerClick(object sender, RoutedEventArgs e)
        {
            // <todo> Aanvullen door Hidde
        }
    }
}
