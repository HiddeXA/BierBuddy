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
        public Main _Main;
        private List<Visitor> _PotentialMatches;

        public event EventHandler<MatchedEventArgs>? OnMatched;
        public FindBuddies(IDataAccess dataAccess, Main main) 
        { 
            _DataAccess = dataAccess;
            _Main = main;
            _Main.AccountSwitcher.OnClientProfileChanged += OnClientProfileChanged;
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

            List<Visitor> potentialMatches = _DataAccess.GetNotSeenAccounts(_Main.ClientVisitor.ID, 5);
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
            _DataAccess.SetLike(_Main.ClientVisitor.ID, visitor.ID);
            _PotentialMatches.Remove(visitor);

            if (_DataAccess.CheckIfMatch(_Main.ClientVisitor.ID, visitor.ID))
            {
                OnMatched?.Invoke(this, new MatchedEventArgs(_Main.ClientVisitor.ID, visitor.ID));
            }
        }

        public void DislikeVisitor(Visitor visitor)
        {
            _DataAccess.SetDislike(_Main.ClientVisitor.ID, visitor.ID);
            _PotentialMatches.Remove(visitor);
        }

        private void OnClientProfileChanged(object sender, ClientProfileChangedEventArgs args)
        {
            UpdatePotentialMatches();
        }

    }
}
