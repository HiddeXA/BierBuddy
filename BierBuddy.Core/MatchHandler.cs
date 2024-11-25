namespace BierBuddy.Core;

public class MatchHandler
{
    private IDataAccess DataAccess;
    public Visitor ClientVisitor;
    
    public event EventHandler<MatchedEventArgs> OnMatched;
    public MatchHandler(IDataAccess dataAccess, Visitor clientVisitor)
    {
        DataAccess = dataAccess;
        ClientVisitor = clientVisitor;
    }

    public List<Visitor> GetPotentialMatches(Visitor visitor)
    {
        //TODO: algorithm implementation

        #region Temporary algorithem implementation

        List<Visitor> potentialMatches = DataAccess.GetNotSeenAccounts(visitor.ID, 5);
        potentialMatches.AddRange(DataAccess.GetLikedNotSeenAccounts(visitor.ID ,5));
        
        potentialMatches.OrderBy(x=> Random.Shared.Next()).ToList();

        #endregion
        
        return potentialMatches;
        
    }

    public void LikeVisitor(Visitor visitor)
    {
        DataAccess.SetLike(ClientVisitor.ID, visitor.ID);
        
        if (DataAccess.CheckIfMatch(ClientVisitor.ID, visitor.ID))
        {
            OnMatched?.Invoke(this, new MatchedEventArgs(ClientVisitor.ID ,visitor.ID));
        }
    }
    
    public void DislikeVisitor(Visitor visitor)
    {
        DataAccess.SetDislike(ClientVisitor.ID, visitor.ID);
    }
    
    
    
    
}