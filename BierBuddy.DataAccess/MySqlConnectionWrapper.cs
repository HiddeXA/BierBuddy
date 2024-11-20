using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.DataAccess
{
    public class MySqlConnectionWrapper : IDbConnection
    {
        private readonly MySqlConnection _conn;

        public MySqlConnectionWrapper(MySqlConnection conn)
        {
            _conn = conn;
        }

        public MySqlCommand CreateCommand()
        {
            return _conn.CreateCommand();
        }

        public MySqlTransaction BeginTransaction()
        {
            return _conn.BeginTransaction();
        }

        public void Open()
        {
            _conn.Open();
        }
    }
}
