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

        public event EventHandler<MatchedEventArgs>? OnMatched;
        public FindBuddies(IDataAccess dataAccess, Main main) 
        { 
            _DataAccess = dataAccess;
            Main = main;
            Main.AccountSwitcher.OnClientProfileChanged += OnClientProfileChanged;
            _PotentialMatches = GetPotentialMatches();
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
            //TODO: algorithm implementation

            #region Temporary algorithem implementation
            

            List<Visitor> potentialMatches = _DataAccess.GetNotSeenAccounts(Main.ClientVisitor.ID, 5);
            //potentialMatches.AddRange(_DataAccess.GetLikedNotSeenAccounts(visitor.ID, 5));

            potentialMatches.OrderBy(x => Random.Shared.Next()).ToList();

            #endregion

            return potentialMatches;

        }
        public void UpdatePotentialMatches()
        {
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

        private void OnClientProfileChanged(object sender, ClientProfileChangedEventArgs args)
        {
            UpdatePotentialMatches();
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
            int ageDelta;
            //zorg er voor dat ageDelta niet negatief of 0 kan zijn
            if (clientVisitor.Age == potentialMatchVisitor.Age)
            {
                return 1;
            }
            ageDelta = Math.Abs(clientVisitor.Age - potentialMatchVisitor.Age);
            return ageDelta;
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
