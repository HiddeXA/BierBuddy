using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BierBuddy.UILib
{
    public interface IPageRenderer
    {
        void UpdatePageSize(double NavBarWidth, double ScreenWidth);
    }
}
