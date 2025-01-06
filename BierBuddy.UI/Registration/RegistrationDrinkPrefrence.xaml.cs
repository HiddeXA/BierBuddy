using System.Windows;
using System.Windows.Controls;
using BierBuddy.Core;
using BierBuddy.DataAccess;
using MySql.Data.MySqlClient;

namespace BierBuddy.UI.Registration;

public partial class RegistrationDrinkPrefrence : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public IDataAccess DataAccess { get; set; }
    public RegistrationDrinkPrefrence(IDataAccess dataAccess) : this(new Visitor(), dataAccess)
    {
     
    }
    
    public RegistrationDrinkPrefrence(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        DataAccess = dataAccess;
        
        InitializeComponent();
        
        ListBox.ItemsSource = dataAccess.GetPossibleDrinks().Values.ToList();

        foreach (var item in ListBox.Items)
        {
            if (RegistrationVisitor.DrinkPreference.Contains(item.ToString()))
            {
                
                ListBox.SelectedItems.Add(item);
            }
        }
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (ListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Selecteer minstens 1 drankje");
            return;
        }
        
        foreach (var drink in ListBox.SelectedItems)
        {
            RegistrationVisitor.AddToDrinkPreference(drink.ToString());
        }
        
        RegistrationInterests registrationInterests = new RegistrationInterests(RegistrationVisitor, DataAccess);
        registrationInterests.Show();
        this.Close();
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
     //Max 4 selected items
        if (ListBox.SelectedItems.Count > 4)
        {
            ListBox.SelectedItems.RemoveAt(4);
        }
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        RegistrationBio registrationBio = new RegistrationBio(RegistrationVisitor, DataAccess);
        registrationBio.Show();
        this.Close();
    }
}