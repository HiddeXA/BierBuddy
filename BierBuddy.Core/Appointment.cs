using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class Appointment
    {
        public long AppointmentID { get; }
        public long ClientID { get; }
        public long VisitorID { get; }
        public DateTime From { get; }
        public DateTime To { get; }
        public bool Accepted { get; }

        private IDataAccess _DataAccess;
        private Main _Main;

        public Appointment(long appointmentID, long clientID, long visitorID, DateTime from, DateTime to, bool accepted)
        {
            AppointmentID = appointmentID;
            ClientID = clientID;
            VisitorID = visitorID;
            From = from;
            To = to;
            Accepted = accepted;


        }
    }
}
