using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kasir.Utils.Controls
{
    public class NavigationMenuItem : RadioButton
    {
        static NavigationMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationMenuItem), new FrameworkPropertyMetadata(typeof(NavigationMenuItem)));
        }
    }
}
