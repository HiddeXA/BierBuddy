using System.Windows;
using BierBuddy.Core;

namespace BierBuddy.UI.Registration;

public partial class RegistrationComplete : Window
{
    public RegistrationComplete(Visitor registrationVisitor, IDataAccess dataAccess)
    {
        if (registrationVisitor == null)
            throw new ArgumentNullException(nameof(registrationVisitor), "Registration visitor cannot be null.");

        if (dataAccess == null)
            throw new ArgumentNullException(nameof(dataAccess), "Data access object cannot be null.");

        List<long> activities = new List<long>();
        List<long> drinks = new List<long>();
        List<long> interests = new List<long>();
        
        foreach (var activity in registrationVisitor.ActivityPreference)
        {
            activities.Add(dataAccess.GetActivityID(activity));
        }
        
        foreach (var drink in registrationVisitor.DrinkPreference)
        {
            drinks.Add(dataAccess.GetDrinkID(drink));
        }
        
        foreach (var interest in registrationVisitor.Interests)
        {
            interests.Add(dataAccess.GetInterestID(interest));
        }
        
        
        dataAccess.AddAccount(registrationVisitor.Name, registrationVisitor.Bio, registrationVisitor.Age, activities, drinks, interests, registrationVisitor.Photos, registrationVisitor.Mail, registrationVisitor.Password);
        
        InitializeComponent();
        
        
    }

    private void Next_OnClick(object sender, RoutedEventArgs e)
    {
        LoginScreen loginScreen = new LoginScreen();
        loginScreen.Show();
        this.Close();
    }
}