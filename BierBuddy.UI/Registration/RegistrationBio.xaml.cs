﻿using System.Windows;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationBio : Window
{
    private Visitor RegistrationVisitor { get; set; }
    private IDataAccess DataAccess { get; set; }
    
    public RegistrationBio(IDataAccess dataAccess) : this(new Visitor(), dataAccess)
    {
     
    }
    public RegistrationBio(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        RegistrationVisitor = registrationVisitor;
        this.DataContext = RegistrationVisitor;
        DataAccess = dataAccess;
        
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        if (RegistrationVisitor.Bio == null)
        {
            MessageBox.Show("vergeet niet je bio in te vullen");
            return;
        }
        
        RegistrationDrinkPrefrence registrationDrinkPrefrence = new RegistrationDrinkPrefrence(RegistrationVisitor, DataAccess);
        registrationDrinkPrefrence.Show();
        this.Close();
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        RegistrationNameAge registrationNameAge = new RegistrationNameAge(RegistrationVisitor, DataAccess);
        registrationNameAge.Show();
        this.Close();
    }
}