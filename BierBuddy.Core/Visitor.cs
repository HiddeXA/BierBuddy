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
        public int Age { get; private set; }

        // Voorkeuren interesses enz.
        public List<string> DrinkPreference = new List<string>();
        public List<string> Interests = new List<string>();
        public List<string> ActivityPreference = new List<string>();

        // Fotos
        public List<byte[]> Photos = new List<byte[]>();


        public Visitor(long id, string name, string bio, int age)
        {
            ID = id;
            Name = name;
            Bio = bio;
            Age = age;
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

        public void AddToPhotos(byte[] photoBlob)
        {
            //geen url is geen url, hoort dus niet thuis in de lijst
            if (Encoding.UTF8.GetString(photoBlob).Equals("Geen URL gevonden")) return;
            Photos.Add(photoBlob);
        }

        public void RemoveFromDrinkPreference(string drink)
        {
            DrinkPreference.Remove(drink);
        }

        public void RemoveFromInterests(string interest)
        {
            Interests.Remove(interest);
        }

        public void RemoveFromActivityPreference(string activity) 
        { 
            ActivityPreference.Remove(activity); 
        }

        public void RemoveFromPhotos(byte[] photoBlob)
        {
            Photos.Remove(photoBlob);
        }
    }
}
