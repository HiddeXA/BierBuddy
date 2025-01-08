using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class ClientProfileChangedEventArgs : EventArgs
    {
        public long NewClientProfileID { get; set; }

        public ClientProfileChangedEventArgs(long newClientProfileID)
        {
            NewClientProfileID = newClientProfileID;
        }
    }
}
