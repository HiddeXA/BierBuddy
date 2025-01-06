using System.ComponentModel.DataAnnotations;
using System.Windows;
using BierBuddy.Core;
using BierBuddy.UI.Registration;

namespace BierBuddy.UI;

public partial class RegistrationMain : Window
{
    public Visitor RegistrationVisitor { get; set; }
    public IDataAccess DataAccess { get; set; }
    public RegistrationMain(IDataAccess dataAccess) : this(dataAccess, new Visitor())
    {
    
    }
    
    public RegistrationMain(IDataAccess dataAccess, Visitor visitor)
    {
        RegistrationVisitor = visitor;
        this.DataContext = RegistrationVisitor;
        DataAccess = dataAccess;
        InitializeComponent();
    }


    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine(RegistrationVisitor.Mail);
        Console.WriteLine(RegistrationVisitor.Password);

        if (string.IsNullOrEmpty(RegistrationVisitor.Mail) || string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(RepeatedPassword.Password))
        {
            MessageBox.Show("Vull alle velden in");
            return;
        }
        
        if (new EmailAddressAttribute().IsValid(RegistrationVisitor.Mail) == false)
        {
            MessageBox.Show("Vul een geldig email adres in");
            return;
        }
        
        if (Password.Password != RepeatedPassword.Password)
        {
            MessageBox.Show("Wachtwoorden komen niet overeen");
            return;
        }
        
        RegistrationVisitor.Password = Password.Password;
        
        
        RegistrationNameAge registrationNameAge = new RegistrationNameAge(RegistrationVisitor, DataAccess); 
        registrationNameAge.Show(); 
        this.Close();
        
    }
}