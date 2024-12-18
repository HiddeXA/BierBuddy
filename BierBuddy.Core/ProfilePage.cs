using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class ProfilePage
    {
        private IDataAccess _DataAccess;
        public Main Main;

        public ProfilePage(IDataAccess dataAccess, Main main)
        {
            _DataAccess = dataAccess;
            Main = main;
        }

        public List<string> GetPossiblePreferences(string type, List<string> exceptions)
        {
            Dictionary<long, string> possiblePreferences;
            if (type == "Drinks")
            {
                possiblePreferences = _DataAccess.GetPossibleDrinks();
            }
            else if (type == "Interests")
            {
                possiblePreferences = _DataAccess.GetPossibleInterests();
            }
            else if (type == "Activities")
            {
                possiblePreferences = _DataAccess.GetPossibleActivities();
            }
            else
            {
                throw new Exception("Invalid type");
            }

            List<string> preferences = possiblePreferences.Values.ToList();
            foreach (string exception in exceptions)
            {
                preferences.Remove(exception);
            }
            return preferences;
        }

        public void UpdateProfile(Visitor visitor)
        {
            List<long> drinks = new List<long>();
            List<long> interests = new List<long>();
            List<long> activities = new List<long>();
            Dictionary<long, string> possibleDrinks = _DataAccess.GetPossibleDrinks();
            Dictionary<long, string> possibleInterests = _DataAccess.GetPossibleInterests();
            Dictionary<long, string> possibleActivities = _DataAccess.GetPossibleActivities();
            foreach (string drink in visitor.DrinkPreference)
            {
                drinks.Add(possibleDrinks.FirstOrDefault(x => x.Value == drink).Key);
            }
            foreach (string interest in visitor.Interests)
            {
                interests.Add(possibleInterests.FirstOrDefault(x => x.Value == interest).Key);
            }
            foreach (string activity in visitor.ActivityPreference)
            {
                activities.Add(possibleActivities.FirstOrDefault(x => x.Value == activity).Key);
            }
            _DataAccess.UpdateAccount(visitor, activities, drinks, interests);
        }
    }
}
