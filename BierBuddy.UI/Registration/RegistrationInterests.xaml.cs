using System.Windows;
using System.Windows.Controls;
using BierBuddy.Core;
using BierBuddy.DataAccess;
using MySql.Data.MySqlClient;

namespace BierBuddy.UI.Registration;

public partial class RegistrationInterests : Window
{
    public Visitor RegistrationVisitor { get; set; }

    public RegistrationInterests()
    {
        InitializeComponent();
    }
    
    public RegistrationInterests(Visitor registrationVisitor)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;

        MySqlConnection connection =
            new MySqlConnection("server=localhost;database=BierBuddy;user=root;port=3306;password=");
        connection.Open();
        
        MySQLDatabase dataAccess = new MySQLDatabase(connection );
        
        Console.WriteLine();
        
        InitializeComponent();
        
        ListBox.ItemsSource = dataAccess.GetPossibleInterests().Values.ToList();
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
        
        RegistrationActivityPrefrence registrationConfirmation = new RegistrationActivityPrefrence(RegistrationVisitor);
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
}