using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using MySql.Data.MySqlClient;
using BierBuddy.Core;
using BierBuddy.DataAccess;

namespace BierBuddy.Tests
{
    internal class DataAccessTests
    {
        [Test]
        public void AddAccount_ShouldAddAccountToDatabase()
        {
            // Arrange
            Mock<IDbConnection> mockConnection = new Mock<IDbConnection>();
            Mock<MySqlCommand> mockCommand = new Mock<MySqlCommand>();
            Mock<MySqlTransaction> mockTransaction = new Mock<MySqlTransaction>();

            mockConnection.Setup(x => x.CreateCommand()).Returns(mockCommand.Object);
            mockConnection.Setup(x => x.BeginTransaction()).Returns(mockTransaction.Object);
            mockCommand.Setup(x => x.ExecuteNonQuery()).Returns(1); // Simuleer succesvolle query uitvoering
            mockCommand.Setup(x => x.LastInsertedId).Returns(123); // Simuleer een gegenereerd ID

            MySQLDatabase database = new MySQLDatabase(mockConnection.Object);
            string name = "Hans";
            string bio = "Ik ben Hans";
            int age = 25;
            List<long> activities = new List<long> { 1, 2, 3, 4 };
            List<long> drinks = new List<long> { 1, 2, 3, 4 };
            List<long> interests = new List<long> { 1, 2, 3, 4 };
            List<string> photos = new List<string> { "photo1", "photo2", "photo3", "photo4" };

            // Act
            database.AddAccount(name, bio, age, activities, drinks, interests, photos);

            // Assert
            mockCommand.Verify(x => x.ExecuteNonQuery(), Times.Exactly(4)); // Controleer of ExecuteNonQuery 4 keer is aangeroepen
                                                                            // ... andere assertions om te controleren of de juiste data naar de database is gestuurd ...
        }
    }
}
