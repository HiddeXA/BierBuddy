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
            rick.AddToActivityPreference("je moeder");
            rick.AddToInterests("oikos");
            rick.AddToActivityPreference("oikos");
            rick.Photos.Add("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTDpFS-Zk2p0R8q579sthM7-zf2HfFaLnkq5A&s");
            rick.Photos.Add("https://i.imgur.com/ZySsvEN.jpeg");
            rick.Photos.Add("https://i.scdn.co/image/ab67616d00001e022abc26e73c19483eb6d50230");
            return rick;
        }
    }
}
