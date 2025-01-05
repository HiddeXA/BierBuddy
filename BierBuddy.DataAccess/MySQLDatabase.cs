using BierBuddy.Core;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace BierBuddy.DataAccess
{
    public class MySQLDatabase : IDataAccess
    {
        public Dictionary<long, string> PossibleInterests { get; set; }
        public Dictionary<long, string> PossibleDrinkPref { get; set; }
        public Dictionary<long, string> PossibleActivities { get; set; }

        private MySqlConnection _conn;
        /// <summary>
        /// Maakt een nieuwe MySQLDatabase aan, DIT OPENT NIET DE CONNECTIE, DOE DIT ZELF NOG
        /// </summary>
        /// <param name="connection"></param>
        public MySQLDatabase(MySqlConnection connection)
        {
            _conn = connection;

            PossibleInterests = GetPossibleInterests();
            PossibleDrinkPref = GetPossibleDrinks();
            PossibleActivities = GetPossibleActivities();
        }

        public List<Visitor> GetLikedNotSeenAccounts(long ID, int maxAmount = 10)
        {
            throw new NotImplementedException();
        }

        public Visitor? AddAccount(string name, string bio, int age, List<long> activities, List<long> drinks, List<long> interests, List<string> photos, string mail, string psw)
        {
            psw = ComputeSHA512(psw);
            if (activities.Count < 1 || activities.Count > 4)
            {
                throw new ArgumentException("Er moeten minimaal 1 en maximaal 4 activiteiten worden meegegeven.");
            }
            if(drinks.Count < 1 || drinks.Count > 4)
            {
                throw new ArgumentException("Er moeten minimaal 1 en maximaal 4 drankjes worden meegegeven.");
            }
            if (interests.Count < 1 || interests.Count > 4)
            {
                throw new ArgumentException("Er moeten minimaal 1 en maximaal 4 interesses worden meegegeven.");
            }
            if (photos.Count < 1 || photos.Count > 4)
            {
                throw new ArgumentException("Er moeten minimaal 1 en maximaal 4 foto's worden meegegeven.");
            }
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand activitiesCommand = _conn.CreateCommand();
            activitiesCommand.CommandText = "INSERT INTO activitypreferences (Activities_ActivityID1, Activities_ActivityID2, Activities_ActivityID3, Activities_ActivityID4) VALUES (@ActivityID1, @ActivityID2, @ActivityID3, @ActivityID4)";
            activitiesCommand.Parameters.AddWithValue("@ActivityID1", activities[0]);
            activitiesCommand.Parameters.AddWithValue("@ActivityID2", activities[1]);
            activitiesCommand.Parameters.AddWithValue("@ActivityID3", activities[2]);
            activitiesCommand.Parameters.AddWithValue("@ActivityID4", activities[3]);
            activitiesCommand.ExecuteNonQuery();
            long activitiesID = activitiesCommand.LastInsertedId;
            MySqlCommand drinksCommand = _conn.CreateCommand();
            drinksCommand.CommandText = "INSERT INTO drinkpreferences (Drinks_DrinkID1, Drinks_DrinkID2, Drinks_DrinkID3, Drinks_DrinkID4) VALUES (@DrinkID1, @DrinkID2, @DrinkID3, @DrinkID4)";
            drinksCommand.Parameters.AddWithValue("@DrinkID1", drinks[0]);
            drinksCommand.Parameters.AddWithValue("@DrinkID2", drinks[1]);
            drinksCommand.Parameters.AddWithValue("@DrinkID3", drinks[2]);
            drinksCommand.Parameters.AddWithValue("@DrinkID4", drinks[3]);
            drinksCommand.ExecuteNonQuery();
            long drinksID = drinksCommand.LastInsertedId;
            MySqlCommand interestsCommand = _conn.CreateCommand();
            interestsCommand.CommandText = "INSERT INTO interests (PossibleInterests_InterestID1, PossibleInterests_InterestID2, PossibleInterests_InterestID3, PossibleInterests_InterestID4) VALUES (@InterestID1, @InterestID2, @InterestID3, @InterestID4)";
            interestsCommand.Parameters.AddWithValue("@InterestID1", interests[0]);
            interestsCommand.Parameters.AddWithValue("@InterestID2", interests[1]);
            interestsCommand.Parameters.AddWithValue("@InterestID3", interests[2]);
            interestsCommand.Parameters.AddWithValue("@InterestID4", interests[3]);
            interestsCommand.ExecuteNonQuery();
            long interestsID = interestsCommand.LastInsertedId;
            MySqlCommand photosCommand = _conn.CreateCommand();
            photosCommand.CommandText = "INSERT INTO photo (Photo1URL, Photo2URL, Photo3URL, Photo4URL) VALUES (@Photo1, @Photo2, @Photo3, @Photo4)";
            photosCommand.Parameters.AddWithValue("@Photo1", photos[0]);
            photosCommand.Parameters.AddWithValue("@Photo2", photos[1]);
            photosCommand.Parameters.AddWithValue("@Photo3", photos[2]);
            photosCommand.Parameters.AddWithValue("@Photo4", photos[3]);
            photosCommand.ExecuteNonQuery();
            long photosID = photosCommand.LastInsertedId;
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT INTO visitor (Name, Bio, Age, Photo_PhotoID, DrinkPreferences_DrinkPreferencesID, Interests_InterestsID, ActivityPreferences_ActivityPreferencesID, Email, Passkey) VALUES (@Name, @Bio, @Age, @Photo_ID, @DrinksID, @InterestsID, @ActivitiesID, @mail, @psw)";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Bio", bio);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Photo_ID", photosID);
            cmd.Parameters.AddWithValue("@DrinksID", drinksID);
            cmd.Parameters.AddWithValue("@InterestsID", interestsID);
            cmd.Parameters.AddWithValue("@ActivitiesID", activitiesID);
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@psw", psw);
            cmd.ExecuteNonQuery();
            long ID = cmd.LastInsertedId;
            transaction.Commit();
            return GetAccount(ID);
        }

        public Visitor? GetAccount(long ID)
        {
            PossibleInterests = GetPossibleInterests();
            PossibleDrinkPref = GetPossibleDrinks();
            PossibleActivities = GetPossibleActivities();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = 
                "SELECT VisitorID, Name, Bio, Age, Photo_PhotoID, DrinkPreferences_DrinkPreferencesID, ActivityPreferences_ActivityPreferencesID, Interests_InterestsID " +
                "FROM visitor " +
                "WHERE VisitorID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();

            Visitor? visitor = null;
            long photoID = -1;
            long drinkID = -1;
            long activityID = -1;
            long interestID = -1;
            while (reader.Read())
            {
                visitor = new Visitor(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                photoID = reader.GetInt64(4);
                drinkID = reader.GetInt64(5);
                activityID = reader.GetInt64(6);
                interestID = reader.GetInt64(7);
            }
            reader.Close();
            if (photoID == -1 || drinkID == -1 || activityID == -1 || interestID == -1 || visitor == null)
            {
                throw new ArgumentNullException("Deze bezoeker bestaat waarschijnlijk niet in de database");
            }

            MySqlCommand photoCmd = _conn.CreateCommand();
            photoCmd.CommandText = "SELECT Photo1URL, Photo2URL, Photo3URL, Photo4URL FROM photo WHERE PhotoID = @ID";
            photoCmd.Parameters.AddWithValue("@ID", photoID);
            photoCmd.ExecuteNonQuery();
            MySqlDataReader photoReader = photoCmd.ExecuteReader();
            while (photoReader.Read())
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!photoReader.IsDBNull(i))
                    {
                        visitor.AddToPhotos((byte[])photoReader.GetValue(i));
                    }
                }
            }
            photoReader.Close();

            MySqlCommand drinkCmd = _conn.CreateCommand();
            drinkCmd.CommandText = "SELECT Drinks_DrinkID1, Drinks_DrinkID2, Drinks_DrinkID3, Drinks_DrinkID4 FROM drinkpreferences WHERE DrinkPreferencesID = @ID";
            drinkCmd.Parameters.AddWithValue("@ID", drinkID);
            drinkCmd.ExecuteNonQuery();
            MySqlDataReader drinkReader = drinkCmd.ExecuteReader();
            while (drinkReader.Read()) 
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!drinkReader.IsDBNull(i))
                    {
                        visitor.AddToDrinkPreference(PossibleDrinkPref[drinkReader.GetInt64(i)]);
                    }
                }
            }
            drinkReader.Close();

            MySqlCommand activityCmd = _conn.CreateCommand();
            activityCmd.CommandText = "SELECT Activities_ActivityID1, Activities_ActivityID2, Activities_ActivityID3, Activities_ActivityID4 FROM activitypreferences WHERE ActivityPreferencesID = @ID";
            activityCmd.Parameters.AddWithValue("@ID", activityID);
            activityCmd.ExecuteNonQuery();
            MySqlDataReader activityReader = activityCmd.ExecuteReader();
            while (activityReader.Read())
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!activityReader.IsDBNull(i))
                    {
                        visitor.AddToActivityPreference(PossibleActivities[activityReader.GetInt64(i)]);
                    }
                }
            }
            activityReader.Close();

            MySqlCommand interestCmd = _conn.CreateCommand();
            interestCmd.CommandText = "SELECT PossibleInterests_InterestID1, PossibleInterests_InterestID2, PossibleInterests_InterestID3, PossibleInterests_InterestID4 FROM interests WHERE InterestsID = @ID";
            interestCmd.Parameters.AddWithValue("@ID", interestID);
            interestCmd.ExecuteNonQuery();
            MySqlDataReader interestReader = interestCmd.ExecuteReader();
            while (interestReader.Read()) {
                for (int i = 0; i < 4; i++)
                {
                    if (!interestReader.IsDBNull(i))
                    {
                        visitor.AddToInterests(PossibleInterests[interestReader.GetInt64(i)]);
                    }
                }
            }
            interestReader.Close();

            return visitor;
        }

        public Visitor? GetAccount(string mail, string passkey)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText =
                "SELECT V.VisitorID, V.Name, V.BIO, V.Age, P.Photo1URL, P.Photo2URL, P.Photo3URL, P.Photo4URL, D.Drinks_DrinkID1, D.Drinks_DrinkID2, D.Drinks_DrinkID3, D.Drinks_DrinkID4, A.Activities_ActivityID1, A.Activities_ActivityID2, A.Activities_ActivityID3, A.Activities_ActivityID4, I.PossibleInterests_InterestID1, I.PossibleInterests_InterestID2, I.PossibleInterests_InterestID3, I.PossibleInterests_InterestID4 " +
                "FROM visitor V " +
                "JOIN photo P ON V.Photo_PhotoID = P.PhotoID " +
                "JOIN drinkpreferences D ON V.DrinkPreferences_DrinkPreferencesID = D.DrinkPreferencesID " +
                "JOIN activitypreferences A ON V.ActivityPreferences_ActivityPreferencesID = A.ActivityPreferencesID " +
                "JOIN interests I ON V.Interests_InterestsID = I.interestsID " +
                "WHERE V.Email = @mail AND V.Passkey = @passkey";
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@passkey", ComputeSHA512(passkey));
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();

            Visitor? visitor = null;
            while (reader.Read())
            {
                visitor = new Visitor(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                for (int i = 4; i < 8; i++)
                {
                    if (!reader.IsDBNull(i))
                    {
                        visitor.AddToPhotos(reader.GetString(i));
                    }
                }
                for (int i = 8; i < 12; i++)
                {
                    string? drinkPref = PossibleDrinkPref.GetValueOrDefault(reader.GetInt64(i));
                    if (drinkPref != null)
                    {
                        visitor.AddToDrinkPreference(drinkPref);
                    }
                }
                for (int i = 12; i < 16; i++)
                {
                    string? activityPref = PossibleActivities.GetValueOrDefault(reader.GetInt64(i));
                    if (activityPref != null)
                    {
                        visitor.AddToActivityPreference(activityPref);
                    }
                }
                for (int i = 16; i < 20; i++)
                {
                    string? interests = PossibleInterests.GetValueOrDefault(reader.GetInt64(i));
                    if (interests != null)
                    {
                        visitor.AddToInterests(interests);
                    }
                }
            }
            reader.Close();

            return visitor;
        }

        public List<Visitor> GetAccounts(int maxAmount)
        {
            throw new NotImplementedException();
        }

        public List<Visitor> GetBuddies(long clientID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText =
                "SELECT V.VisitorID, V.Name, V.BIO, V.Age " +
                "FROM matches m " +
                "JOIN Visitor V " +
                "ON (m.Visitor_VisitorID1 = v.VisitorID AND m.Visitor_VisitorID2 = @ID) " +
                "OR (m.Visitor_VisitorID2 = v.VisitorID AND m.Visitor_VisitorID1 = @ID) " +
                "WHERE v.VisitorID != @ID; ";
            cmd.Parameters.AddWithValue("@ID", clientID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Visitor> buddyList = new();
            while (reader.Read())
            {
                Visitor visitor = new Visitor(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));

                buddyList.Add(visitor);
            }
            reader.Close();
            return buddyList;
        }

        public List<Visitor> GetNotSeenAccounts(long clientID, int maxAmount = 10)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText =
                "SELECT V.VisitorID, V.Name, V.BIO, V.Age, P.Photo1URL, P.Photo2URL, P.Photo3URL, P.Photo4URL, D.Drinks_DrinkID1, D.Drinks_DrinkID2, D.Drinks_DrinkID3, D.Drinks_DrinkID4, A.Activities_ActivityID1, A.Activities_ActivityID2, A.Activities_ActivityID3, A.Activities_ActivityID4, I.PossibleInterests_InterestID1, I.PossibleInterests_InterestID2, I.PossibleInterests_InterestID3, I.PossibleInterests_InterestID4 " +
                "FROM visitor V " +
                "JOIN photo P ON V.Photo_PhotoID = P.PhotoID " +
                "JOIN drinkpreferences D ON V.DrinkPreferences_DrinkPreferencesID = D.DrinkPreferencesID " +
                "JOIN activitypreferences A ON V.ActivityPreferences_ActivityPreferencesID = A.ActivityPreferencesID " +
                "JOIN interests I ON V.Interests_InterestsID = I.interestsID " +
                "WHERE V.VisitorID NOT IN (" +
                    "SELECT likedID " +
                    "FROM likes " +
                    "WHERE likerID = @ID)" +
                "AND V.VisitorID NOT IN (" +
                    "SELECT dislikedID " +
                    "FROM dislikes " +
                    "WHERE dislikerID = @ID) " +
                "AND NOT V.VisitorID = @ID;";
            cmd.Parameters.AddWithValue("@ID", clientID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Visitor> visitors = new();
            while (reader.Read())
            {
                Visitor visitor = new Visitor(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                for (int i = 4; i < 8; i++)
                {
                    if (!reader.IsDBNull(i))
                    {
                        visitor.AddToPhotos((byte[])reader.GetValue(i));
                    }
                }
                for (int i = 8; i < 12; i++)
                {
                    string? drinkPref = PossibleDrinkPref.GetValueOrDefault(reader.GetInt64(i));
                    if (drinkPref != null)
                    {
                        visitor.AddToDrinkPreference(drinkPref);
                    }
                }
                for (int i = 12; i < 16; i++)
                {
                    string? activityPref = PossibleActivities.GetValueOrDefault(reader.GetInt64(i));
                    if (activityPref != null)
                    {
                        visitor.AddToActivityPreference(activityPref);
                    }
                }
                for (int i = 16; i < 20; i++)
                {
                    string? interests = PossibleInterests.GetValueOrDefault(reader.GetInt64(i));
                    if (interests != null)
                    {
                        visitor.AddToInterests(interests);
                    }
                }
                visitors.Add(visitor);
            }
            reader.Close();

            return visitors;
        }

        public bool CheckIfMatch(long ID1, long ID2)
        {
            throw new NotImplementedException();
        }

        public List<long> GetGivenLikes(long ID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM likes WHERE LikerID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<long> likes = new List<long>();
            while (reader.Read())
            {
                likes.Add(reader.GetInt64(1));
            }
            reader.Close();
            return likes;
        }

        public List<long> GetMatches(long ID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM matches WHERE Visitor_VisitorID1 = @ID OR Visitor_VisitorID2 = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<long> matches = new List<long>();
            while (reader.Read())
            {
                if (reader.GetInt64(0) == ID)
                {
                    matches.Add(reader.GetInt64(1));
                }
                else
                {
                    matches.Add(reader.GetInt64(0));
                }
            }
            reader.Close();
            return matches;
        }

        public Dictionary<long, string> GetPossibleActivities()
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM possibleactivities";
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            Dictionary<long, string> activities = new Dictionary<long, string>();
            while (reader.Read())
            {
                activities.Add(reader.GetInt64(0), reader.GetString(1));
            }
            reader.Close();
            return activities;
        }

        public Dictionary<long, string> GetPossibleDrinks()
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM possibledrinks";
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            Dictionary<long, string> drinks = new Dictionary<long, string>();
            while (reader.Read())
            {
                drinks.Add(reader.GetInt64(0), reader.GetString(1));
            }
            reader.Close();
            return drinks;
        }

        public Dictionary<long, string> GetPossibleInterests()
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM possibleinterests";
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            Dictionary<long, string> interests = new Dictionary<long, string>();
            while (reader.Read())
            {
                interests.Add(reader.GetInt64(0), reader.GetString(1));
            }
            reader.Close();
            return interests;
        }

        public List<long> GetReceivedLikes(long ID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM likes WHERE LikedID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<long> likes = new List<long>();
            while (reader.Read())
            {
                likes.Add(reader.GetInt64(0));
            }
            reader.Close();
            return likes;
        }

        public void SetDislike(long dislikerID, long dislikedID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT INTO dislikes (DislikerID, DislikedID) VALUES (@DislikerID, @DislikedID)";
            cmd.Parameters.AddWithValue("@DislikerID", dislikerID);
            cmd.Parameters.AddWithValue("@DislikedID", dislikedID);
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }

        public void SetLike(long likerID, long likedID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT INTO likes (LikerID, LikedID) VALUES (@LikerID, @LikedID)";
            cmd.Parameters.AddWithValue("@LikerID", likerID);
            cmd.Parameters.AddWithValue("@LikedID", likedID);
            cmd.ExecuteNonQuery();
            // Check of er een match is
            if (GetReceivedLikes(likerID).Contains(likedID))
            {
                MySqlCommand cmd2 = _conn.CreateCommand();
                cmd2.CommandText = "INSERT INTO matches (Visitor_VisitorID1, Visitor_VisitorID2) VALUES (@LikerID, @LikedID)";
                cmd2.Parameters.AddWithValue("@LikerID", likerID);
                cmd2.Parameters.AddWithValue("@LikedID", likedID);
                cmd2.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        public void AddAppointment(long clientID, long visitorID, DateTime from, DateTime to)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT INTO appointments (Visitor_VisitorID1, Visitor_VisitorID2, Start, End) Values (@clientID, @visitorID, @from, @to)";
            cmd.Parameters.AddWithValue("@clientID", clientID);
            cmd.Parameters.AddWithValue("@visitorID", visitorID);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }

        public void ApproveAppointment(long appointmentID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "UPDATE appointments SET Accepted = 1 WHERE AppointmentID = @ID";
            cmd.Parameters.AddWithValue("@ID", appointmentID);
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }

        public void DeclineAppointment(long appointmentID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "DELETE FROM appointments WHERE AppointmentID = @ID";
            cmd.Parameters.AddWithValue("@ID", appointmentID);
            int rows = cmd.ExecuteNonQuery();
            if(rows > 1)
            {
                transaction.Rollback();
                throw new Exception("Meer dan 1 rij verwijderd. Transactie niet uitgevoerd.");
            } else
            {
                transaction.Commit();
            }
        }

        public List<Appointment> GetAppointmentsFromUser(long clientID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM appointments WHERE Visitor_VisitorID1 = @ID OR Visitor_VisitorID2 = @ID";
            cmd.Parameters.AddWithValue("@ID", clientID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Appointment> appointments = new List<Appointment>();
            while(reader.Read()) {
                appointments.Add(new Appointment(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetBoolean(5)));
            }
            reader.Close();
            transaction.Commit();
            return appointments;
        }

        public List<Appointment> GetAppointmentsWithUser(long clientID, long visitorID)
        {
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM appointments WHERE (Visitor_VisitorID1 = @clientID AND Visitor_VisitorID2 = @visitorID) OR (Visitor_VisitorID1 = @visitorID AND Visitor_VisitorID2 = @clientID)";
            cmd.Parameters.AddWithValue("@clientID", clientID);
            cmd.Parameters.AddWithValue("@visitorID", visitorID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<Appointment> appointments = new List<Appointment>();
            while(reader.Read()) {
                appointments.Add(new Appointment(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetBoolean(5)));
            }
            reader.Close();
            transaction.Commit();
            return appointments;
        }


        // Encrypts password using SHA512
        static string ComputeSHA512(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }

        public void UpdateAccount(Visitor visitor, List<long> activities, List<long> drinks, List<long> interests)
        {
            long activityID = -1;
            long drinkID = -1;
            long interestID = -1;
            List<byte[]> photos = visitor.Photos;
            long photosID = -1;
            //ids ophalen van de voorkeuren lijsten
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand GetIDs = _conn.CreateCommand();
            GetIDs.CommandText = "SELECT DrinkPreferences_DrinkPreferencesID, Interests_InterestsID, ActivityPreferences_ActivityPreferencesID, Photo_PhotoID FROM visitor WHERE VisitorID = @ID";
            GetIDs.Parameters.AddWithValue("@ID", visitor.ID);
            GetIDs.ExecuteNonQuery();
            MySqlDataReader reader = GetIDs.ExecuteReader();
            while (reader.Read())
            {
                drinkID = reader.GetInt64(0);
                interestID = reader.GetInt64(1);
                activityID = reader.GetInt64(2);
                photosID = reader.GetInt64(3);
            }
            reader.Close();
            if(photosID == -1 || activityID == -1 || drinkID == -1 || interestID == -1)
            {
                throw new ArgumentNullException("Deze bezoeker bestaat waarschijnlijk niet in de database");
            }

            //alles updaten
            MySqlCommand activitiesCommand = _conn.CreateCommand();
            activitiesCommand.CommandText = "UPDATE activitypreferences SET Activities_ActivityID1 = @ActivityID1, Activities_ActivityID2 = @ActivityID2, Activities_ActivityID3 = @ActivityID3, Activities_ActivityID4 = @ActivityID4 WHERE ActivityPreferencesID = @ActivityPreferencesID";
            activitiesCommand.Parameters.AddWithValue("@ActivityID1", activities[0]);
            activitiesCommand.Parameters.AddWithValue("@ActivityID2", activities.Count > 1 ? activities[1] : null);
            activitiesCommand.Parameters.AddWithValue("@ActivityID3", activities.Count > 2 ? activities[2] : null);
            activitiesCommand.Parameters.AddWithValue("@ActivityID4", activities.Count > 3 ? activities[3] : null);
            activitiesCommand.Parameters.AddWithValue("@ActivityPreferencesID", activityID);
            activitiesCommand.ExecuteNonQuery();
            MySqlCommand drinksCommand = _conn.CreateCommand();
            drinksCommand.CommandText = "UPDATE drinkpreferences SET Drinks_DrinkID1 = @DrinkID1, Drinks_DrinkID2 = @DrinkID2, Drinks_DrinkID3 = @DrinkID3, Drinks_DrinkID4 = @DrinkID4 WHERE DrinkPreferencesID = @DrinkPreferencesID";
            drinksCommand.Parameters.AddWithValue("@DrinkID1", drinks[0]);
            drinksCommand.Parameters.AddWithValue("@DrinkID2", drinks.Count > 1 ? drinks[1] : null);
            drinksCommand.Parameters.AddWithValue("@DrinkID3", drinks.Count > 2 ? drinks[2] : null);
            drinksCommand.Parameters.AddWithValue("@DrinkID4", drinks.Count > 3 ? drinks[3] : null);
            drinksCommand.Parameters.AddWithValue("@DrinkPreferencesID", drinkID);
            drinksCommand.ExecuteNonQuery();
            MySqlCommand interestsCommand = _conn.CreateCommand();
            interestsCommand.CommandText = "UPDATE interests SET PossibleInterests_InterestID1 = @InterestID1, PossibleInterests_InterestID2 = @InterestID2, PossibleInterests_InterestID3 = @InterestID3, PossibleInterests_InterestID4 = @InterestID4 WHERE InterestsID = @InterestsID";
            interestsCommand.Parameters.AddWithValue("@InterestID1", interests[0]);
            interestsCommand.Parameters.AddWithValue("@InterestID2", interests.Count > 1 ? interests[1] : null);
            interestsCommand.Parameters.AddWithValue("@InterestID3", interests.Count > 2 ? interests[2] : null);
            interestsCommand.Parameters.AddWithValue("@InterestID4", interests.Count > 3 ? interests[3] : null);
            interestsCommand.Parameters.AddWithValue("@InterestsID", interestID);
            interestsCommand.ExecuteNonQuery();
            MySqlCommand photosCommand = _conn.CreateCommand();
            photosCommand.CommandText = "UPDATE photo SET Photo1URL = @Photo1, Photo2URL = @Photo2, Photo3URL = @Photo3, Photo4URL = @Photo4 WHERE PhotoID = @PhotoID";
            photosCommand.Parameters.AddWithValue("@Photo1", photos[0]);
            photosCommand.Parameters.AddWithValue("@Photo2", photos.Count > 1 ? photos[1] : null);
            photosCommand.Parameters.AddWithValue("@Photo3", photos.Count > 2 ? photos[2] : null);
            photosCommand.Parameters.AddWithValue("@Photo4", photos.Count > 3 ? photos[3] : null);
            photosCommand.Parameters.AddWithValue("@PhotoID", photosID);
            photosCommand.ExecuteNonQuery();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "UPDATE visitor SET Name = @Name, Bio = @Bio WHERE VisitorID = @VisitorID";
            cmd.Parameters.AddWithValue("@Name", visitor.Name);
            cmd.Parameters.AddWithValue("@Bio", visitor.Bio);
            cmd.Parameters.AddWithValue("@VisitorID", visitor.ID);
            cmd.ExecuteNonQuery();
            transaction.Commit();
        }
    }
}
