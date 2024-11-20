namespace BierBuddy.Core;

public class MatchHandler
{
    public MatchHandler()
    {
        
    }

    public List<Visitor> getPotentialMatches(Visitor visitor)
    {
        List<Visitor> potentialMatches = new List<Visitor>();

        //TODO: algorithm implementation

        #region Temporary dummy data 
        
        Visitor DummyVisitor = new Visitor(424242424242, "Alice",
            "Ik ben Alice een dummy account als je dit ziet verwijder mij dan als nodig", 42);
        
        potentialMatches.Add(DummyVisitor);
        
        return potentialMatches;

        #endregion 
        
    }
    
    
    
    
}