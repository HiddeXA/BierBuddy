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

    
    
    
}