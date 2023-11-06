using Kasir.Commons;
using Kasir.Commons.Commands;
using Kasir.Views.WindowBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasir.ViewModels
{
    public class HomeVM : ViewModelBase
    {
        private readonly NavigationVM _navigationVM;
        public ICommand DataProdukCommand { get; }
        public ICommand KategoriCommand { get; }
        public ICommand BarangMasukCommand { get; }
        public ICommand DataPelangganCommand { get; }
        public ICommand HistoryTransactionCommand { get; }
        public HomeVM(NavigationVM navigationVM)
        {
            _navigationVM = navigationVM;
            StartTransactionCommand = new RelayCommand(StartTransaction);
            DataProdukCommand = _navigationVM.ProductManagementCommand;
            KategoriCommand = _navigationVM.CategoryManagementCommand;
            BarangMasukCommand = _navigationVM.CategoryManagementCommand;
            HistoryTransactionCommand = _navigationVM.HistoryTransactionCommand;
        }

        public ICommand StartTransactionCommand { get; }

        private WindowBaseFullscreen TransactionMenuView { get; set; } = null;

        private void StartTransaction(object View)
        {
            if (TransactionMenuView == null)
            {
                TransactionMenuView = new WindowBaseFullscreen();
                TransactionMenuView.Title = "Menu Transaksi";
                TransactionMenuView.Height = 760;
                TransactionMenuView.Width = 1180;
                TransactionMenuView.Pages.Content = new TransactionMenuVM();
                TransactionMenuView.Closed += TransactionMenuView_Closed;
                TransactionMenuView.Show();
            }
            if (TransactionMenuView?.WindowState == System.Windows.WindowState.Minimized) TransactionMenuView.WindowState = System.Windows.WindowState.Normal;
            if (TransactionMenuView?.Visibility == System.Windows.Visibility.Visible) TransactionMenuView.Activate();
        }

        private void TransactionMenuView_Closed(object? sender, EventArgs e)
        {
            TransactionMenuView = null;
        }
    }
}
