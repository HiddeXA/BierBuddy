using BierBuddy.Core;
using MySql.Data.MySqlClient;

namespace BierBuddy.DataAccess
{
    public class MySQLDatabase : IDataAccess
    {
        private IDbConnection _conn;
        public MySQLDatabase(IDbConnection connection)
        {
            _conn = connection;
            _conn.Open();
        }

        public Visitor? AddAccount(string name, string bio, int age, List<long> activities, List<long> drinks, List<long> interests, List<string> photos)
        {
            if (activities.Count < 1 || activities.Count > 4 || drinks.Count < 1 || drinks.Count > 4 || interests.Count < 1 || interests.Count > 4 || photos.Count < 1 || photos.Count > 4)
            {
                throw new ArgumentException("Er moeten minimaal 1 en maximaal 4 activiteiten, drankjes, interesses en foto's worden meegegeven.");
            }
            MySqlTransaction transaction = _conn.BeginTransaction();
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT INTO visitor (Name, Bio, Age) VALUES (@Name, @Bio, @Age)";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Bio", bio);
            cmd.Parameters.AddWithValue("@Age", age);
            cmd.ExecuteNonQuery();
            long ID = cmd.LastInsertedId;
            MySqlCommand cmd2 = _conn.CreateCommand();
            cmd2.CommandText = "INSERT INTO activitypreferences VALUES (@VisitorID, @ActivityID1, @ActivityID2, @ActivityID3, @ActivityID4)";
            cmd2.Parameters.AddWithValue("@VisitorID", ID);
            cmd2.Parameters.AddWithValue("@ActivityID1", activities[0]);
            cmd2.Parameters.AddWithValue("@ActivityID2", activities[1]);
            cmd2.Parameters.AddWithValue("@ActivityID3", activities[2]);
            cmd2.Parameters.AddWithValue("@ActivityID4", activities[3]);
            cmd2.ExecuteNonQuery();
            MySqlCommand cmd3 = _conn.CreateCommand();
            cmd3.CommandText = "INSERT INTO drinkpreferences VALUES (@VisitorID, @DrinkID1, @DrinkID2, @DrinkID3, @DrinkID4)";
            cmd3.Parameters.AddWithValue("@VisitorID", ID);
            cmd3.Parameters.AddWithValue("@DrinkID1", drinks[0]);
            cmd3.Parameters.AddWithValue("@DrinkID2", drinks[1]);
            cmd3.Parameters.AddWithValue("@DrinkID3", drinks[2]);
            cmd3.Parameters.AddWithValue("@DrinkID4", drinks[3]);
            cmd3.ExecuteNonQuery();
            MySqlCommand cmd4 = _conn.CreateCommand();
            cmd4.CommandText = "INSERT INTO interests VALUES (@VisitorID, @InterestID1, @InterestID2, @InterestID3, @InterestID4)";
            cmd4.Parameters.AddWithValue("@VisitorID", ID);
            cmd4.Parameters.AddWithValue("@InterestID1", interests[0]);
            cmd4.Parameters.AddWithValue("@InterestID2", interests[1]);
            cmd4.Parameters.AddWithValue("@InterestID3", interests[2]);
            cmd4.Parameters.AddWithValue("@InterestID4", interests[3]);
            cmd4.ExecuteNonQuery();
            transaction.Commit();
            return GetAccount(ID);
        }

        public Visitor? GetAccount(long ID)
        {
            MySqlCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM visitor WHERE ID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            Visitor? visitor = null;
            while (reader.Read())
            {
                visitor = new Visitor(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            }
            reader.Close();
            return visitor;
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
            cmd.CommandText = "SELECT * FROM matches WHERE LikerID = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<long> matches = new List<long>();
            while (reader.Read())
            {
                matches.Add(reader.GetInt64(1));
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
                cmd2.CommandText = "INSERT INTO matches (LikerID, LikedID) VALUES (@LikerID, @LikedID)";
                cmd2.Parameters.AddWithValue("@LikerID", likerID);
                cmd2.Parameters.AddWithValue("@LikedID", likedID);
                cmd2.ExecuteNonQuery();
            }
            transaction.Commit();
        }
    }
}
