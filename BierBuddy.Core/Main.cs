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

        public Main(IDataAccess dataAccess) 
        {
            _DataAccess = dataAccess;
            ClientVisitor = _DataAccess.GetAccount(1);

            _DataAccess = dataAccess;
            AccountSwitcher = new SwitchAccount(_DataAccess, this);
        }

    }
}
