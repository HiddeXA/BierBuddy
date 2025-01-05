using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class Visitor
    {
        // Basis atributen van de gebruiker.
        public long ID { get; private set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public int Age { get; set; }
        public String Mail { get; set; }
        public String Password { get; set; }

        // Voorkeuren interesses enz.
        public List<string> DrinkPreference = new List<string>();
        public List<string> Interests = new List<string>();
        public List<string> ActivityPreference = new List<string>();

        // Fotos
        public List<string> Photos = new List<string>();


        public Visitor(long id, string name, string bio, int age)
        {
            ID = id;
            Name = name;
            Bio = bio;
            Age = age;
        }

        //Lege constructor voor het aanmaken van een nieuwe gebruiker. 
        public Visitor()
        {
            
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

        public void AddToPhotos(string photoUrl)
        {
            Photos.Add(photoUrl);
        }
    }
}
