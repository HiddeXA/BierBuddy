using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class MyAppointments
    {

        private IDataAccess _DataAccess;
        private Main _Main;

        public MyAppointments(IDataAccess dataAccess, Main main)
        {
            _DataAccess = dataAccess;
            _Main = main;
        }

      
    }
}
