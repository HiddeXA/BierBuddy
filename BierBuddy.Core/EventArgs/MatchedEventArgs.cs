namespace BierBuddy.Core;

public class MatchedEventArgs : EventArgs
{
    public long Visitor1ID { get; set; }
    public long Visitor2ID { get; set; }

    public MatchedEventArgs(long visitor1Id, long visitor2Id)
    {
        Visitor1ID = visitor1Id;
        Visitor2ID = visitor2Id;
    }
}