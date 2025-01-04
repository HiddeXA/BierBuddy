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
        public readonly int PotentialMatchesTotalListSize = 12;
        public readonly int PotentialMatchesHighPointSelectionListSize = 5;
        public readonly int SelectionTotalsListSize = 7;


        public event EventHandler<MatchedEventArgs>? OnMatched;
        public FindBuddies(IDataAccess dataAccess, Main main) 
        { 
            _DataAccess = dataAccess;
            Main = main;
            Main.AccountSwitcher.OnClientProfileChanged += OnClientProfileChanged;
            _PotentialMatches = new List<Visitor>();
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
            if(_PotentialMatches.Count == 0)
            {
                UpdatePotentialMatches();
            }
            Visitor potentialMatch = _PotentialMatches.First();
            return potentialMatch;
        }
        public void UpdatePotentialMatches()
        {
            _PotentialMatches = GetPotentialMatches();
        }
        private void OnClientProfileChanged(object sender, ClientProfileChangedEventArgs args)
        {
            UpdatePotentialMatches();
        }
        public List<Visitor> GetPotentialMatches()
        {
            List<long> idSelection = _DataAccess.GetNotSeenAccountIDs(Main.ClientVisitor.ID);
            List<Visitor> VisitorSelection = GetVisitorSelectionByID(GetRandomAccountSelection(idSelection, PotentialMatchesTotalListSize));
            VisitorSelection = SortVisitorSelectionByPoints(SetVisitorPoints(VisitorSelection));
            List<Visitor> highRated = GetHighRatedVisitorSelection(VisitorSelection);
            List<Visitor> lowRated = GetLowRatedVisitorSelection(VisitorSelection, highRated);
            return FineTuneVisitorSelection(highRated, lowRated);
        }
        public List<Visitor> GetHighRatedVisitorSelection(List<Visitor> visitors)
        {
            if(visitors.Count < PotentialMatchesHighPointSelectionListSize)
            {
                return visitors;
            }
            return visitors.Take(PotentialMatchesHighPointSelectionListSize).ToList();
        }
        public List<Visitor> GetLowRatedVisitorSelection(List<Visitor> visitors, List<Visitor> highRated)
        {
            List<Visitor> lowRated = new List<Visitor>();
            foreach (Visitor v in visitors)
            {
                if (!highRated.Contains(v))
                {
                    lowRated.Add(v);
                }
            }
            return lowRated;
        }

        public List<Visitor> FineTuneVisitorSelection(List<Visitor> selectedVisitors, List<Visitor> lowRatedVisitors)
        {
            Random rnd = new Random();
            List<Visitor>  lowRatedSelection = lowRatedVisitors.OrderBy(i => rnd.Next()).Take(SelectionTotalsListSize - selectedVisitors.Count).ToList();
            while (lowRatedSelection.Count > 0)
            {
                selectedVisitors.Add(lowRatedSelection.First());
                lowRatedSelection.Remove(lowRatedSelection.First());
            }
            return selectedVisitors;
        }
        public List<Visitor> SetVisitorPoints(List<Visitor> visitorSelection)
        {
            foreach (Visitor Visitor in visitorSelection)
            {
                Visitor.Points = GetVisitorPoints(Main.ClientVisitor, Visitor);
            }
            return visitorSelection;
        }

        public List<Visitor> SortVisitorSelectionByPoints(List<Visitor> visitorSelection)
        {
            return [.. visitorSelection.OrderByDescending(i => i.Points)];
        }

        public List<long> GetRandomAccountSelection(List<long> ids, int count)
        {
            Random rnd = new Random();
            return ids.OrderBy(i => rnd.Next()).Take(count).ToList();
        }
        
        public List<Visitor> GetVisitorSelectionByID(List<long> ids)
        {
            return _DataAccess.GetAccountsFromList(ids);

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
