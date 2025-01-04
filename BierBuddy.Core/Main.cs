using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.Core
{
    public class Main
    {
        public Visitor ClientVisitor { get; set; }
        public SwitchAccount AccountSwitcher { get; }

        private IDataAccess _DataAccess { get; }

        public Main(IDataAccess dataAccess, Visitor acc) 
        {
            _DataAccess = dataAccess;
            ClientVisitor = _DataAccess.GetAccount(acc.ID);

            _DataAccess = dataAccess;
            AccountSwitcher = new SwitchAccount(_DataAccess, this);
        }
    }
}
