using System.Windows;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationNameAge : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public RegistrationNameAge()
    {
        RegistrationVisitor = new Visitor();
        this.DataContext = RegistrationVisitor;
        
        InitializeComponent();
    }
    public RegistrationNameAge(Visitor visitor)
    {
        RegistrationVisitor = visitor;
        this.DataContext = RegistrationVisitor;

        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {

        if (RegistrationVisitor.Name == null || RegistrationVisitor.Age == 0)
        {
            MessageBox.Show("vergeet niet je naam en leeftijd in te vullen");
            return;
        }
        
        RegistrationBio registrationBio = new RegistrationBio(RegistrationVisitor);
        registrationBio.Show();
        this.Close();
    }
}