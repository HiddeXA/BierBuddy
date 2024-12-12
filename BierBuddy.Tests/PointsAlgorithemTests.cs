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
        public void SetUp()
        {
            _dataAccessMock = new Mock<IDataAccess>();
            _main = new Main(_dataAccessMock.Object);
            _clientVisitor = new Visitor(1, "Client", "Test Bio", 25);
            _potentialMatchVisitor = new Visitor(2, "Match", "Test Bio", 27);
            _findBuddies = new FindBuddies(_dataAccessMock.Object, _main);
        }

        [Test]
        public void GetInterestsPoints_NullClientVisitor_ReturnsZero()
        {
            // Act
            int points = _findBuddies.GetInterestsPoints(null, _potentialMatchVisitor);

            // Assert
            Assert.That(points, Is.EqualTo(0));
        }

        [Test]
        public void GetInterestsPoints_NullPotentialMatchVisitor_ReturnsZero()
        {
            // Act
            int points = _findBuddies.GetInterestsPoints(_clientVisitor, null);

            // Assert
            Assert.That(points, Is.EqualTo(0));
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
    }
}
