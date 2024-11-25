using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BierBuddy.UILib
{
    public interface IPageRenderer
    {
        void UpdatePageSize(double NavBarWidth, Size ScreenWidth);
    }
}
