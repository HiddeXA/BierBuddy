using MySql.Data.MySqlClient;
using BierBuddy.Core;
using BierBuddy.DataAccess;
using System.Diagnostics;

namespace BierBuddy.Tests
{
    [TestFixture]
    [Category("DataAccessTests")]
    internal class DataAccessTests
    {
        private MySqlConnection _conn;
        [OneTimeSetUp]
        public void Setup()
        {
            // Maak een tijdelijke database aan
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=;"))
            {
                conn.Open();
                DropDatabase();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "CREATE DATABASE test_db";
                cmd.ExecuteNonQuery();
            }

            // Maak een connectie met de tijdelijke database
            string scriptConnectionString = "server=localhost;database=test_db;user=root;port=3306;password=;Allow User Variables=True";
            _conn = new MySqlConnection(scriptConnectionString);
            _conn.Open();

            // Run alle sql bestanden in een folder
            string[] files = Directory.GetFiles("../../../../BierBuddy/migrations", "V*.sql");
            foreach (string file in files)
            {
                // Lees de SQL-query uit het bestand
                string sqlQuery = File.ReadAllText(file);

                // Voer de query uit
                using (MySqlCommand cmd = new MySqlCommand(sqlQuery, _conn))
                {
                    cmd.ExecuteScalar();
                }
                Thread.Sleep(1000); // Wacht 1 seconde

            }
            _conn.Close();
            string testConnectionString = "server=localhost;database=test_db;user=root;port=3306;password=";
            _conn = new MySqlConnection(testConnectionString);
            _conn.Open();
            using (var command = new MySqlCommand($"SHOW DATABASES LIKE 'test_db';", _conn))
            {
                using (var reader = command.ExecuteReader())
                {
                    bool has = reader.HasRows;
                }
            }
        }

        [Test]
        [Order(0)]
        public void AddAccount_ShouldThrowArgumentException_WhenInvalidNumberOfItems()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => database.AddAccount("Test", "Bio", 30, new List<long>(), new List<long> { 1, 2, 3, 4 }, new List<long> { 1, 2, 3, 4 }, new List<string> { "a", "b", "c", "d" }, "Test@mail", "768f966ef3f73b7703903e0bf4222dabdc8e5cc910255af2bd10adc5d3032a164aeedb9ba252fe48c028e11cec2d73bf411d90aa88e92af9297d155caebafa28"));
        }

        [Test]
        [Order(1)]
        public void AddAccount_ShouldAddAccount_WhenValidNumberOfItems()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Visitor? account = database.AddAccount("Test", "Bio", 30, new List<long> { 1, 2, 3, 4 }, new List<long> { 1, 2, 3, 4 }, new List<long> { 1, 2, 3, 4 }, new List<string> { "a", "b", "c", "d" }, "Test@mail", "768f966ef3f73b7703903e0bf4222dabdc8e5cc910255af2bd10adc5d3032a164aeedb9ba252fe48c028e11cec2d73bf411d90aa88e92af9297d155caebafa28");

            // Assert
            Assert.IsNotNull(account);
        }

        [Test]
        [Order(2)]
        public void GetAccount_ShouldReturnAccount_WhenAccountExists()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Visitor? account = database.GetAccount(1);

            // Assert
            Assert.IsNotNull(account);
        }

        [Test]
        [Order(3)]
        public void GetAccount_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Visitor? account = database.GetAccount(2);

            // Assert
            Assert.IsNull(account);
        }

        [Test]
        [Order(4)]
        public void AddAccount_ShouldAddAccount_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Visitor? account = database.AddAccount("Test", "Bio", 30, new List<long> { 1, 2, 3, 4 }, new List<long> { 1, 2, 3, 4 }, new List<long> { 1, 2, 3, 4 }, new List<string> { "a", "b", "c", "d" }, "Test@mail", "768f966ef3f73b7703903e0bf4222dabdc8e5cc910255af2bd10adc5d3032a164aeedb9ba252fe48c028e11cec2d73bf411d90aa88e92af9297d155caebafa28");

            // Assert
            Assert.IsNotNull(account);
        }

        [Test]
        [Order(5)]
        public void SetLike_ShouldAddLike_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            database.SetLike(1, 2);

            // Assert
            List<long> likes = database.GetReceivedLikes(2);
            Assert.Contains(1, likes);
        }

        [Test]
        [Order(6)]
        public void SetDislike_ShouldAddDislike_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            database.SetDislike(1, 2);

            Assert.IsTrue(true);
        }

        [Test]
        [Order(7)]
        public void GetGivenLikes_ShouldReturnLikes_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            List<long> likes = database.GetGivenLikes(1);

            // Assert
            Assert.Contains(2, likes);
        }

        [Test]
        [Order(8)]
        public void GetReceivedLikes_ShouldReturnLikes_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            List<long> likes = database.GetReceivedLikes(2);

            // Assert
            Assert.Contains(1, likes);
        }

        [Test]
        [Order(9)]
        public void SetLike_ShouldAddLike_WhenValid2()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            database.SetLike(2, 1);

            // Assert
            List<long> likes = database.GetReceivedLikes(1);
            Assert.Contains(2, likes);
        }

        [Test]
        [Order(10)]
        public void GetMatches_ShouldReturnMatches_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            List<long> matches = database.GetMatches(1);

            // Assert
            Assert.Contains(2, matches);
        }

        [Test]
        [Order(11)]
        public void GetPossibleActivities_ShouldReturnActivities_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Dictionary<long, string> activities = database.GetPossibleActivities();

            // Assert
            Assert.Greater(activities.Count, 0);
        }

        [Test]
        [Order(12)]
        public void GetPossibleDrinks_ShouldReturnDrinks_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Dictionary<long, string> drinks = database.GetPossibleDrinks();

            // Assert
            Assert.Greater(drinks.Count, 0);
        }

        [Test]
        [Order(13)]
        public void GetPossibleInterests_ShouldReturnInterests_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            Dictionary<long, string> interests = database.GetPossibleInterests();

            // Assert
            Assert.Greater(interests.Count, 0);
        }

        [Test]
        [Order(14)]
        public void AddAppointment_ShouldAddAppointment_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            database.AddAppointment(1, 2, DateTime.Now, DateTime.Now);

            // Assert
            List<Appointment> appointments = database.GetAppointmentsWithUser(1, 2);
            Assert.Greater(appointments.Count, 0);
        }

        [Test]
        [Order(15)]
        public void ApproveAppointment_ShouldApproveAppointment_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            List<Appointment> appointments = database.GetAppointmentsWithUser(1, 2);
            database.ApproveAppointment(appointments.First().AppointmentID);

            // Assert
            appointments = database.GetAppointmentsWithUser(1, 2);
            Assert.IsTrue(appointments.First().Accepted);
        }

        [Test]
        [Order(16)]
        public void DeclineAppointment_ShouldDeclineAppointment_WhenValid()
        {
            MySQLDatabase database = new MySQLDatabase(_conn);

            // Act
            List<Appointment> appointments = database.GetAppointmentsWithUser(1, 2);
            database.DeclineAppointment(appointments.First().AppointmentID);

            // Assert
            appointments = database.GetAppointmentsFromUser(1);
            Assert.IsFalse(appointments.Any());
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            // Sluit de verbinding
            _conn.Close();

            //// Verwijder de tijdelijke database
            DropDatabase();
        }

        public void DropDatabase()
        {
            // Verwijder de tijdelijke database
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=;"))
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DROP DATABASE IF EXISTS test_db";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
