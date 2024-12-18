using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class FindBuddies
    {
        private IDataAccess _DataAccess;
        public Main Main;
        private List<Visitor> _PotentialMatches;
        private readonly int _PotentialMatchesTotalListSize = 10;

        public event EventHandler<MatchedEventArgs>? OnMatched;
        public FindBuddies(IDataAccess dataAccess, Main main) 
        { 
            _DataAccess = dataAccess;
            Main = main;
            Main.AccountSwitcher.OnClientProfileChanged += OnClientProfileChanged;
            _PotentialMatches = GetPotentialMatches();
        }
        
        public void LikeVisitor(Visitor visitor)
        {
            _DataAccess.SetLike(Main.ClientVisitor.ID, visitor.ID);
            _PotentialMatches.Remove(visitor);

        }

        public void DislikeVisitor(Visitor visitor)
        {
            _DataAccess.SetDislike(Main.ClientVisitor.ID, visitor.ID);
            _PotentialMatches.Remove(visitor);
        }
        public Visitor GetPotentialMatch()
        {
            if (_PotentialMatches.Count != 0)
            {
                Visitor potentialMatch = _PotentialMatches.First();
                return potentialMatch;
            }
            return null;
        }

        public List<Visitor> GetPotentialMatches()
        {
            List<long> idSelection = _DataAccess.GetNotSeenAccountIDs(Main.ClientVisitor.ID);
            return GetPotentialMatchesByID(idSelection);
        }
        public List<long> GetRandomAccountSelection(List<long> ids)
        {
            //
        }
        public void UpdatePotentialMatches()
        {
            _PotentialMatches = GetPotentialMatches();
        }
        private void OnClientProfileChanged(object sender, ClientProfileChangedEventArgs args)
        {
            UpdatePotentialMatches();
        }

        public List<Visitor> GetPotentialMatchesByID(List<long> ids)
        {
            //knip list

        }
        public List<long> GetIDsSelection(List<long> ids)
        {
            List<long> idSelection = new List<long>();

            int potentialMatchesHighscoreListSize = 10;
            
            for (int i = 0; idSelection.Count < potentialMatchesHighscoreListSize; i++)
            {

            }
            for (int i = potentialMatchesHighscoreListSize; idSelection.Count < _PotentialMatchesTotalListSize is ++)
            {

            }
            return idSelection;
        }

        public int GetInterestsPoints(Visitor clientVisitor, Visitor potentialMatchVisitor)
        {
            int points = 0;
            if (clientVisitor.DrinkPreference.Count != 0)
            {
                foreach (string drinkPreference in clientVisitor.DrinkPreference)
                {
                    if (potentialMatchVisitor.DrinkPreference.Count != 0 && potentialMatchVisitor.DrinkPreference.Contains(drinkPreference))
                    {
                        points++;
                    }
                }
            }
            if(clientVisitor.Interests.Count != 0)
            {
                foreach (string interesse in clientVisitor.Interests)
                {
                    if (potentialMatchVisitor.Interests.Count != 0 && potentialMatchVisitor.Interests.Contains(interesse))
                    {
                        points++;
                    }
                }
            }
            if(clientVisitor.ActivityPreference.Count != 0)
            {
                foreach (string activityPreference in clientVisitor.ActivityPreference)
                {
                    if (potentialMatchVisitor.ActivityPreference.Count != 0 && potentialMatchVisitor.ActivityPreference.Contains(activityPreference))
                    {
                        points++;
                    }
                }
            }
            return points;
        }

        public int GetAgeDelta(Visitor clientVisitor, Visitor potentialMatchVisitor)
        {
            //zorg er voor dat ageDelta niet negatief of 0 kan zijn
            if (clientVisitor.Age == potentialMatchVisitor.Age)
            {
                return 1;
            }
            return Math.Abs(clientVisitor.Age - potentialMatchVisitor.Age);
        }

        public double GetVisitorPoints(Visitor ClientVisitor, Visitor PotentialMatchVisitor)
        {
            double ageModifier = 0.2;
            int pointsInterestsMatch = GetInterestsPoints(ClientVisitor, PotentialMatchVisitor);
            int ageDelta = GetAgeDelta(ClientVisitor, PotentialMatchVisitor);

            double points = pointsInterestsMatch / (ageModifier * ageDelta / ClientVisitor.Age);

            return points;
        }


        
    }
}
