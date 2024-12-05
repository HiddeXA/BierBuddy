using BierBuddy.Core;
using Microsoft.VisualBasic.CompilerServices;
using Moq;

namespace BierBuddy.Tests;

[TestFixture]
[TestOf(typeof(FindBuddies))]
public class MatchHandlerTest
{
    //private Mock<IDataAccess> DataAccessMoq;
        
    //[SetUp]
    //public void Init()
    //{
    //    DataAccessMoq = new Mock<IDataAccess>();
    //}

    //[Test]
    //public void MatchProtocolEventTest()
    //{
    //    DataAccessMoq.Setup(x => x.GetNotSeenAccounts(It.IsAny<long>(), It.IsAny<int>())).Returns(new List<Visitor>{
    //        new Visitor(3, "Piet", "Ik ben Piet en ik hou van bier", 19),
    //        new Visitor(4, "Klaas", "Ik ben Klaas en ik hou van bier", 27),
    //        });
    //    DataAccessMoq.Setup(x => x.GetLikedNotSeenAccounts(It.IsAny<long>(), It.IsAny<int>())).Returns(new List<Visitor>{
    //        new Visitor(2, "Janita", "Ik ben Janita en ik hou van bier", 23),
    //        });
    //    DataAccessMoq.Setup(x => x.CheckIfMatch(It.IsAny<long>(), It.IsAny<long>())).
    //        Returns((long id1, long id2) => id1 == 1 && id2 == 2 || id1 == 2 && id2 == 1);
    //    DataAccessMoq.Setup(x => x.GetAccount(It.IsAny<long>())).Returns(new Visitor(1, "Piet", "Ik ben Piet en ik hou van bier", 19));

    //    Main _Main = new Main(DataAccessMoq.Object);
    //    FindBuddies matchHandler = new FindBuddies(DataAccessMoq.Object, _Main);
        
    //    List<Visitor> potentialMatches = matchHandler.GetPotentialMatches();

    //    bool onMatchCalled = false;
    //    long matchid1 = 0;
    //    long matchid2 = 0;
    //    matchHandler.OnMatched += (o,e) =>
    //    {
    //        onMatchCalled = true;
    //        matchid1 = e.Visitor1ID;
    //        matchid2 = e.Visitor2ID;
    //    };
            
    //    while (potentialMatches.Count > 0)
    //    {
    //        Visitor potentialMatch = matchHandler.GetPotentialMatch();
    //        matchHandler.LikeVisitor(potentialMatch);
    //    }
        
    //    Assert.That(onMatchCalled, Is.True);
    //    Assert.That(matchid1, Is.EqualTo(1));
    //    Assert.That(matchid2, Is.EqualTo(2));

    //}
}