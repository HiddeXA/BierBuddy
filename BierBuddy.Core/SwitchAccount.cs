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
        private Main _Main;
        public event EventHandler<ClientProfileChangedEventArgs>? OnClientProfileChanged;
        public SwitchAccount(IDataAccess dataAcces, Main main)
        { 
            _DataAccess = dataAcces;
            _Main = main;
        }

        public void SwitchClientProfile(string input)
        {
            long newAccountID = int.Parse(input);

            Visitor? newClientProfile = _DataAccess.GetAccount(newAccountID);

            if (newClientProfile == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            _Main.ClientVisitor = newClientProfile;
            OnClientProfileChanged?.Invoke(this, new ClientProfileChangedEventArgs(newAccountID));
        }
    }
    
}
