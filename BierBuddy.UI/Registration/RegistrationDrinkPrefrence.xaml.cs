using System.Windows;
using System.Windows.Controls;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationDrinkPrefrence : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public RegistrationDrinkPrefrence() : this(new Visitor())
    {
     
    }
    
    public RegistrationDrinkPrefrence(Visitor registrationVisitor)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        
        InitializeComponent();
        ListBox.ItemsSource = new List<string> {"Bier", "Wijn", "Whiskey", "Vodka", "Rum", "Gin", "Tequila"};
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // throw new NotImplementedException();
        Console.WriteLine("ListBox_SelectionChanged");
    }
}