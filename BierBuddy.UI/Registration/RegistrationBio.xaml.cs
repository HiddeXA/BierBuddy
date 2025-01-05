using System.Windows;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationBio : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public RegistrationBio()
    {
        RegistrationVisitor = new Visitor();
        this.DataContext = RegistrationVisitor;
        
        InitializeComponent();
    }
    public RegistrationBio(Visitor registrationVisitor)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (RegistrationVisitor.Bio == null)
        {
            MessageBox.Show("vergeet niet je bio in te vullen");
            return;
        }
        
        RegistrationDrinkPrefrence registrationDrinkPrefrence = new RegistrationDrinkPrefrence(RegistrationVisitor);
        registrationDrinkPrefrence.Show();
        this.Close();
    }
}