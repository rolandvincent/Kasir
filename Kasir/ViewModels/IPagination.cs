using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasir.ViewModels
{
    public interface IPagination
    {
        public ICommand FirstCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LastCommand { get; }
        public int CurrentPage { get; set; }
        public int NumberOfPage { get; set; }
        public int SelectedRecord { get; set; }
        public int TotalItems { get; set; }
        public bool IsNextEnabled { get; set; }
        public bool IsPreviousEnabled { get; set; }
    }
}
