using System.Windows;
using System.Windows.Controls;
using BierBuddy.Core;
using BierBuddy.DataAccess;
using MySql.Data.MySqlClient;

namespace BierBuddy.UI.Registration;

public partial class RegistrationInterests : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public IDataAccess DataAccess { get; set; }

    public RegistrationInterests(IDataAccess dataAccess) : this(new Visitor(), dataAccess)
    {
    }
    
    public RegistrationInterests(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        
        DataAccess = dataAccess;
        
        Console.WriteLine();
        
        InitializeComponent();
        
        ListBox.ItemsSource = dataAccess.GetPossibleInterests().Values.ToList();
        
        foreach (var item in ListBox.Items)
        {
            if (RegistrationVisitor.Interests.Contains(item.ToString()))
            {
                
                ListBox.SelectedItems.Add(item);
            }
        }
    }


    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (ListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Selecteer minstens 1 interesse");
            return;
        }
        
        foreach (var interest in ListBox.SelectedItems)
        {
            RegistrationVisitor.AddToInterests(interest.ToString());
        }
        
        RegistrationActivityPrefrence registrationConfirmation = new RegistrationActivityPrefrence(RegistrationVisitor, DataAccess);
        registrationConfirmation.Show();
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
        RegistrationDrinkPrefrence registrationDrinkPrefrence = new RegistrationDrinkPrefrence(RegistrationVisitor, DataAccess);
        registrationDrinkPrefrence.Show();
        this.Close();
    }
}