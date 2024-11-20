namespace BierBuddy.Core;

public class MatchedEventArgs : EventArgs
{
    public ulong Visitor1ID { get; set; }
    public ulong Visitor2ID { get; set; }

    public MatchedEventArgs(ulong visitor1Id, ulong visitor2Id)
    {
        Visitor1ID = visitor1Id;
        Visitor2ID = visitor2Id;
    }
}