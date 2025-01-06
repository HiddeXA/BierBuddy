﻿using System.Windows;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationNameAge : Window
{
    public Visitor RegistrationVisitor { get; set; }
    
    public IDataAccess DataAccess { get; set; }
    
    public RegistrationNameAge(IDataAccess dataAccess) : this(new Visitor(), dataAccess)
    {
    }
    public RegistrationNameAge(Visitor visitor, IDataAccess dataAccess)
    {
        RegistrationVisitor = visitor;
        this.DataContext = RegistrationVisitor;
        DataAccess = dataAccess;
        
        InitializeComponent();
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {

        if (RegistrationVisitor.Name == null || RegistrationVisitor.Age == 0)
        {
            MessageBox.Show("vergeet niet je naam en leeftijd in te vullen");
            return;
        }

        if (RegistrationVisitor.Age < 18)
        {
            MessageBox.Show("Je moet minstens 18 jaar oud zijn om je te registreren");
            return;
        }
        
        RegistrationBio registrationBio = new RegistrationBio(RegistrationVisitor, DataAccess);
        registrationBio.Show();
        this.Close();
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        RegistrationMain registrationMain = new RegistrationMain(DataAccess, RegistrationVisitor);
        registrationMain.Show();
        this.Close();
    }
}