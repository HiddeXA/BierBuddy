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
        private Visitor _ClientVisitor;
        private List<Visitor> _PotentialMatches;

        public event EventHandler<MatchedEventArgs>? OnMatched;
        public FindBuddies(IDataAccess dataAccess, Visitor clientVisitor) 
        { 
            _DataAccess = dataAccess;
            _ClientVisitor = clientVisitor;
            _PotentialMatches = GetPotentialMatches(_ClientVisitor);
        }
        public Visitor GetPotentialMatch()
        {
            Visitor potentialMatch = _PotentialMatches.First();
            return potentialMatch;
        }

        public List<Visitor> GetPotentialMatches(Visitor visitor)
        {
            //TODO: algorithm implementation

            #region Temporary algorithem implementation

            List<Visitor> potentialMatches = _DataAccess.GetNotSeenAccounts(visitor.ID, 5);
            //potentialMatches.AddRange(_DataAccess.GetLikedNotSeenAccounts(visitor.ID, 5));

            potentialMatches.OrderBy(x => Random.Shared.Next()).ToList();

            #endregion

            return potentialMatches;

        }

        public void LikeVisitor(Visitor visitor)
        {
            _DataAccess.SetLike(_ClientVisitor.ID, visitor.ID);

            if (_DataAccess.CheckIfMatch(_ClientVisitor.ID, visitor.ID))
            {
                OnMatched?.Invoke(this, new MatchedEventArgs(_ClientVisitor.ID, visitor.ID));
            }
        }

        public void DislikeVisitor(Visitor visitor)
        {
            _DataAccess.SetDislike(_ClientVisitor.ID, visitor.ID);
        }

    }
}
