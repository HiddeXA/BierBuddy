using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.DataAccess
{
    public interface IDbConnection
    {
        MySqlCommand CreateCommand();
        MySqlTransaction BeginTransaction();
        void Open();
    }
}
