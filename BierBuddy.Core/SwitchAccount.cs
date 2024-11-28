using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class SwitchAccount
    {
        private IDataAccess _DataAccess;
        public SwitchAccount(IDataAccess dataAcces) 
        { 
            _DataAccess = dataAcces;
        }

        public Visitor SwitchClientProfile(string input)
        {
            long newAccountID = int.Parse(input);

            Visitor? newClientProfile = _DataAccess.GetAccount(newAccountID);

            if (newClientProfile == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            return newClientProfile;
        }
    }
    
}
