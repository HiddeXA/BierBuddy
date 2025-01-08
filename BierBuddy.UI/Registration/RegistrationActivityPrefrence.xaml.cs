using System.Windows;
using System.Windows.Controls;
using BierBuddy.Core;
using BierBuddy.DataAccess;
using MySql.Data.MySqlClient;

namespace BierBuddy.UI.Registration;

public partial class RegistrationActivityPrefrence : Window
{
    
    private Visitor RegistrationVisitor { get; set; }
    private IDataAccess DataAccess { get; set; }
    
    public RegistrationActivityPrefrence(IDataAccess dataAccess) : this(new Visitor(), dataAccess)
    {
    }
    public RegistrationActivityPrefrence(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        DataAccess = dataAccess;
        
        Console.WriteLine();
        
        InitializeComponent();
        
        ListBox.ItemsSource = dataAccess.GetPossibleActivities().Values.ToList();
        
        foreach (var item in ListBox.Items)
        {
            if (RegistrationVisitor.ActivityPreference.Contains(item.ToString()))
            {
                
                ListBox.SelectedItems.Add(item);
            }
        }
        
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (ListBox.SelectedItems.Count == 0)
        {
            MessageBox.Show("Selecteer minstens 1 activiteit");
            return;
        }
        
        foreach (var activity in ListBox.SelectedItems)
        {
            RegistrationVisitor.AddToActivityPreference(activity.ToString());
        }
        
        RegistrationPictures registrationConfirmation = new RegistrationPictures(RegistrationVisitor, DataAccess);
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
        RegistrationInterests registrationInterests = new RegistrationInterests(RegistrationVisitor, DataAccess);
        registrationInterests.Show();
        this.Close();
    }
}