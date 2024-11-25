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
            Visitor rick = new Visitor(1, "Rick", "oikos for life <3", 19);
            rick.AddToDrinkPreference("sambuca");
            rick.AddToDrinkPreference("speciaal bier");
            rick.AddToInterests("oikos");
            return rick;
        }
    }
}
