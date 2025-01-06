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
using Org.BouncyCastle.Tls;
using MySql.Data.MySqlClient;

namespace BierBuddy.UI
{
    public partial class LoginScreen : Window
    {
        private IDataAccess _DataAccess { get; }
        private MySqlConnection _Connection { get; }
        public LoginScreen()
        {
            InitializeComponent();
            _Connection = new MySqlConnection("server=mysql-1284b19-bierbuddy.c.aivencloud.com;database=BierBuddyDB;user=avnadmin;port=26316;password=AVNS_IWkNAbagctkfuROBkTX");
            _Connection.Open();
            _DataAccess = new MySQLDatabase(_Connection);

        }

        private void Loginclick(object sender, RoutedEventArgs e)
        {
            string email = EmailInput.Text;
            string passkey = PasskeyInput.Password;
            Visitor? user = _DataAccess.GetAccount(email, passkey);
            if (user != null)
            {
                _Connection.Close();
                MainWindow mainWindow = new MainWindow(user, _DataAccess);
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
            
            RegistrationMain registrationMain = new RegistrationMain(_DataAccess);
            registrationMain.Show();
            this.Close();
        }
    }
}
