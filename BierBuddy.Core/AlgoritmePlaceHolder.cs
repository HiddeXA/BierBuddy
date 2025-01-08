using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO
//DELETE THIS

namespace BierBuddy.Core
{
    public class AlgoritmePlaceHolder
    {
        public Visitor GetVisitor()
        {
            Visitor Dummy = new Visitor(1, "BierBuddyBram", "De trotse mascotte van BierBuddy. Hij is zo cool, hij kan zelfs 500 bier opdrinken in een uur. Niemand is beter dan deze makker.", 21);
            Dummy.AddToDrinkPreference("Jägermeister");
            Dummy.AddToDrinkPreference("Hertog Jan");
            Dummy.AddToDrinkPreference("Speciaal bier");
            Dummy.AddToDrinkPreference("Dikke Vette Drank");
            Dummy.AddToInterests("Bierbrouwen");
            Dummy.AddToInterests("Jagen");
            Dummy.AddToInterests("Skiën");
            Dummy.AddToActivityPreference("Gezellig kletsen");
            Dummy.AddToActivityPreference("Darten");
            return Dummy;
        }
    }
}
