using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasir.ViewModels.PopupModelVM
{
    public interface IPopupModel
    {
        public delegate void AcceptEventHandler(object sender, AcceptEventArgs args);

        public event EventHandler CancelClick;
        public event AcceptEventHandler AcceptClick;

        public ICommand CancelCommand { get; }
        public ICommand AcceptCommand { get; }
    }
}
