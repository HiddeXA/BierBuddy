using BierBuddy.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Tests
{
    internal class PointsAlgorithemTests
    {
        private Mock<IDataAccess> _dataAccessMock;
        private Main _main;
        private Visitor _clientVisitor;
        private Visitor _potentialMatchVisitor;
        private FindBuddies _findBuddies;
 
        [SetUp]
        public void Setup()
        {
            Visitor Dummy = new Visitor(1, "BierBuddyBram", "De trotse mascotte van BierBuddy. Hij is zo cool, hij kan zelfs 500 bier opdrinken in een uur. Niemand is beter dan deze makker.", 21);
            Dummy.AddToDrinkPreference("Jägermeister");
            Dummy.AddToDrinkPreference("Hertog Jan");
            Dummy.AddToDrinkPreference("Speciaal bier");
            Dummy.AddToDrinkPreference("Dikke Vette Drank");
            Dummy.AddToInterests("Bierbrouwen");
            Dummy.AddToInterests("Jagen");
            Dummy.AddToInterests("Skiën");
            Dummy.AddToActivityPreference("Gezellig kletsen");
            Dummy.AddToActivityPreference("Darten");

            _dataAccessMock = new Mock<IDataAccess>();

            // Stel een dummy Visitor in voor de mock van _DataAccess
            _dataAccessMock.Setup(x => x.GetAccount(It.IsAny<long>()))
                           .Returns(new Visitor(1, "Client", "Test Bio", 25));

            _potentialMatchVisitor = new Visitor(2, "Match", "Test Bio", 27);
            List<Visitor> potentialMatches = new List<Visitor> { _potentialMatchVisitor };
            _dataAccessMock.Setup(x => x.GetNotSeenAccounts(It.IsAny<long>(), It.IsAny<int>()))
                           .Returns(potentialMatches);

            // Maak een concrete instantie van Main met de mock
            _main = new Main(_dataAccessMock.Object, Dummy);

            _findBuddies = new FindBuddies(_dataAccessMock.Object, _main);

            _clientVisitor = _main.ClientVisitor;

        }


        [Test]
        public void GetInterestsPoints_MatchingDrinkPreferences_ReturnsCorrectPoints()
        {
            // Arrange
            _clientVisitor.AddToDrinkPreference("Beer");
            _clientVisitor.AddToDrinkPreference("Wine");
            _potentialMatchVisitor.AddToDrinkPreference("Wine");

            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(1));
        }

        [Test]
        public void GetInterestsPoints_MatchingInterests_ReturnsCorrectPoints()
        {
            // Arrange
            _clientVisitor.AddToInterests("Hiking");
            _clientVisitor.AddToInterests("Cooking");
            _potentialMatchVisitor.AddToInterests("Cooking");

            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(1));
        }

        [Test]
        public void GetInterestsPoints_MatchingActivityPreferences_ReturnsCorrectPoints()
        {
            // Arrange
            _clientVisitor.AddToActivityPreference("Running");
            _clientVisitor.AddToActivityPreference("Yoga");
            _potentialMatchVisitor.AddToActivityPreference("Yoga");

            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(1));
        }

        [Test]
        public void GetInterestsPoints_MultipleMatchingPreferences_ReturnsCumulativePoints()
        {
            // Arrange
            _clientVisitor.AddToDrinkPreference("Beer");
            _clientVisitor.AddToInterests("Hiking");
            _clientVisitor.AddToActivityPreference("Yoga");

            _potentialMatchVisitor.AddToDrinkPreference("Beer");
            _potentialMatchVisitor.AddToInterests("Hiking");
            _potentialMatchVisitor.AddToActivityPreference("Yoga");

            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(3));
        }

        [Test]
        public void GetInterestsPoints_NoMatchingPreferences_ReturnsZeroPoints()
        {
            // Arrange
            _clientVisitor.AddToDrinkPreference("Beer");
            _clientVisitor.AddToInterests("Hiking");
            _clientVisitor.AddToActivityPreference("Yoga");

            _potentialMatchVisitor.AddToDrinkPreference("Wine");
            _potentialMatchVisitor.AddToInterests("Cooking");
            _potentialMatchVisitor.AddToActivityPreference("Running");

            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(0));
        }

        [Test]
        public void GetAgeDelta_PotentialMatchOlderThanClient_ReturnsCorrectDelta()
        {
            // Arrange
            _clientVisitor = new Visitor(1, "Client", "Test Bio", 25);
            _potentialMatchVisitor = new Visitor(2, "Match", "Test Bio", 30);

            // Act
            int ageDelta = _findBuddies.GetAgeDelta(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(ageDelta, Is.EqualTo(5));
        }

        [Test]
        public void GetAgeDelta_SameAge_ReturnsOne()
        {
            // Arrange
            _clientVisitor = new Visitor(1, "Client", "Test Bio", 30);
            _potentialMatchVisitor = new Visitor(2, "Match", "Test Bio", 30);

            // Act
            int ageDelta = _findBuddies.GetAgeDelta(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(ageDelta, Is.EqualTo(1));
        }

        [Test]
        public void GetVisitorPoints_ValidInputs_ReturnsCorrectPoints()
        {
            // Arrange
            _clientVisitor = new Visitor(1, "Client", "Test Bio", 30);
            _clientVisitor.AddToDrinkPreference("Beer");

            _potentialMatchVisitor = new Visitor(2, "Match", "Test Bio", 25);
            _potentialMatchVisitor.AddToDrinkPreference("Beer");

            double expectedPoints = 1 / (0.2 * 5 / 30);

            // Act
            double points = _findBuddies.GetVisitorPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(expectedPoints).Within(0.01));
        }

        [Test]
        public void GetVisitorPoints_NoMatchingInterests_ReturnsZeroPoints()
        {
            // Arrange
            _clientVisitor.AddToDrinkPreference("Beer");
            _potentialMatchVisitor.AddToDrinkPreference("Wine");

            // Act
            double points = _findBuddies.GetVisitorPoints(_clientVisitor, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(0));
        }
        [Test]
        public void GetHighRatedVisitorSelection_ReturnsCorrectCount()
        {
            // Arrange
            List<Visitor> visitors = new List<Visitor>();
            for (int i = 0; i < 20; i++)
            {
                visitors.Add(new Visitor(i, $"Visitor {i}", "Bio", 25) { Points = i });
            }

            // Act
            List<Visitor> result = _findBuddies.GetHighRatedVisitorSelection(visitors);

            // Assert
            Assert.That(result.Count, Is.EqualTo(_findBuddies.PotentialMatchesHighPointSelectionListSize));
        }

        [Test]
        public void FineTuneVisitorSelection_AddsLowRatedVisitorsCorrectly()
        {
            // Arrange
            List<Visitor> highRated = new List<Visitor>() { new Visitor(1, "High", "Bio", 25) { Points = 10 } };
            List<Visitor> lowRated = new List<Visitor>() { new Visitor(2, "Low", "Bio", 25) { Points = 5 } };

            // Act
            List<Visitor> result = _findBuddies.FineTuneVisitorSelection(highRated, lowRated);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void SortVisitorSelectionByPoints_SortsCorrectly()
        {
            // Arrange
            List<Visitor> visitors = new List<Visitor>()
            {
                new Visitor(1, "A", "Bio", 25) { Points = 10 },
                new Visitor(2, "B", "Bio", 25) { Points = 30 },
                new Visitor(3, "C", "Bio", 25) { Points = 20 }
            };

            // Act
            List<Visitor> result = _findBuddies.SortVisitorSelectionByPoints(visitors);

            // Assert
            Assert.That(result[0].Points, Is.EqualTo(30));
            Assert.That(result[1].Points, Is.EqualTo(20));
            Assert.That(result[2].Points, Is.EqualTo(10));
        }

    }
}
