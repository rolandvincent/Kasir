using Kasir.Commons;
using Kasir.Utils.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.ViewModels
{
    public class HistoryTransactionVM : ViewModelBase
    {
        private ModalDialogManager _modalDialogManager;
        public HistoryTransactionVM(ModalDialogManager dialogManager)
        {
            _modalDialogManager = dialogManager;
        }
    }
}
