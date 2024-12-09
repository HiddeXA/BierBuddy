using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class MyBuddies
    {
        private IDataAccess _DataAccess;
        private Main _Main;

        public MyBuddies(IDataAccess dataAccess, Main main)
        {
            _DataAccess = dataAccess;
            _Main = main;
        }

        public void AddAppointments(Visitor buddy, List<List<DateTime>> dateTimes)
        {
            foreach (List<DateTime> dateTime in dateTimes)
            {
                _DataAccess.AddAppointment(_Main.ClientVisitor.ID, buddy.ID, dateTime[0], dateTime[1]);
            }
        }
    }
}
