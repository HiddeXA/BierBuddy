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

        public void AddAppointments(Visitor visitor, List<List<DateTime>> dateTimes)
        {
            foreach (List<DateTime> dateTime in dateTimes)
            {
                _DataAccess.AddAppointment(_Main.ClientVisitor.ID, visitor.ID, dateTime[0], dateTime[1]);
            }
        }

        public List<Appointment> GetAppointments(Visitor visitor)
        {
            return _DataAccess.GetAppointmentsWithUser(_Main.ClientVisitor.ID, visitor.ID);
        }

        public void HandleAppointment(Appointment appointment, bool accepted)
        {
            if (accepted)
            {
                _DataAccess.ApproveAppointment(appointment.AppointmentID);
            }
            else
            {
                _DataAccess.DeclineAppointment(appointment.AppointmentID);
            }
        }

        public string visitorName(long visitorID)
        {
            return _DataAccess.GetVisitorNameByID(visitorID);
        }
    }
}
