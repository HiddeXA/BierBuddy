using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    internal class Visitor
    {
        // Basis atributen van de gebruiker.
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public int age { get; private set; }

        // Voorkeuren interesses enz.
        public List<string> DrinkPreference = new List<string>();
        public List<string> Interests = new List<string>();
        public List<string> ActivityPreference = new List<string>();

        // De like, dislike en match atributen.
        public List<int> Likes = new List<int>();
        public List<int> Dislikes = new List<int>();
        public List<int> Matches = new List<int>();

        public List<string> Images = new List<string>();


        public Visitor(int id, string name, string bio, int age)
        {
            ID = id;
            Name = name;
            Bio = bio;
            this.age = age;
        }

        // Methodes voor het toevoegen van voorkeuren en interesses.
        public void AddToDrinkPreference(string drink)
        {
            DrinkPreference.Add(drink);
        }

        public void AddToInterests(string interest)
        {
            Interests.Add(interest);
        }

        public void AddToActivityPreference(string activity)
        {
            ActivityPreference.Add(activity);
        }
        
    }
}
