using BierBuddy.Core;
using MySql.Data.MySqlClient;

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

        public Visitor? AddAccount(string name, string bio, int age, List<long> activities, List<long> drinks, List<long> interests, List<string> photos)
        {
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
            cmd.CommandText = "INSERT INTO visitor (Name, Bio, Age, Photo_PhotoID, DrinkPreferences_DrinkPreferencesID, Interests_InterestsID, ActivityPreferences_ActivityPreferencesID) VALUES (@Name, @Bio, @Age, @Photo_ID, @DrinksID, @InterestsID, @ActivitiesID)";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Bio", bio);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.Parameters.AddWithValue("@Photo_ID", photosID);
            cmd.Parameters.AddWithValue("@DrinksID", drinksID);
            cmd.Parameters.AddWithValue("@InterestsID", interestsID);
            cmd.Parameters.AddWithValue("@ActivitiesID", activitiesID);
            cmd.ExecuteNonQuery();
            long ID = cmd.LastInsertedId;
            transaction.Commit();
            return GetAccount(ID);
        }

        public Visitor? GetAccount(long ID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = 
                "SELECT V.VisitorID, V.Name, V.BIO, V.Age, P.Photo1URL, P.Photo2URL, P.Photo3URL, P.Photo4URL, D.Drinks_DrinkID1, D.Drinks_DrinkID2, D.Drinks_DrinkID3, D.Drinks_DrinkID4, A.Activities_ActivityID1, A.Activities_ActivityID2, A.Activities_ActivityID3, A.Activities_ActivityID4, I.PossibleInterests_InterestID1, I.PossibleInterests_InterestID2, I.PossibleInterests_InterestID3, I.PossibleInterests_InterestID4 " +
                "FROM visitor V " +
                "JOIN photo P ON V.Photo_PhotoID = P.PhotoID " +
                "JOIN drinkpreferences D ON V.DrinkPreferences_DrinkPreferencesID = D.DrinkPreferencesID " +
                "JOIN activitypreferences A ON V.ActivityPreferences_ActivityPreferencesID = A.ActivityPreferencesID " +
                "JOIN interests I ON V.Interests_InterestsID = I.interestsID " +
                "WHERE V.VisitorID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
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
                    "WHERE dislikerID = @ID " +
                "AND NOT V.VisitorID = @ID);";
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
                visitors.Add(visitor);
            }
            reader.Close();

            return visitors;
        }

        public bool CheckIfMatch(long ID1, long ID2)
        {
            //throw new NotImplementedException();
            return false;
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
    }
}
