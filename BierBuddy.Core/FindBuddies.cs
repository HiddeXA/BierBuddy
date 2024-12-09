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

            if (_DataAccess.CheckIfMatch(Main.ClientVisitor.ID, visitor.ID))
            {
                OnMatched?.Invoke(this, new MatchedEventArgs(Main.ClientVisitor.ID, visitor.ID));
            }
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

    }
}
