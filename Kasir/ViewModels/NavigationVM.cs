using Kasir.Commons;
using Kasir.Commons.Commands;
using Kasir.Utils.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kasir.ViewModels
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;
        private MainWindow _window;
        public object CurrentView {
            get => _currentView;
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private int _menuSelectedIndex;
        public int MenuSelectedIndex
        {
            get
            {
                return _menuSelectedIndex;
            }
            set
            {
                _menuSelectedIndex = value;
                OnPropertyChanged();
            }
        }

        public ICommand HomeCommand { get; }
        public ICommand UserManagementCommand { get; }
        public ICommand ProductManagementCommand { get; }
        public ICommand CategoryManagementCommand { get; }
        public ICommand CustomerManagementCommand { get; }
        public ICommand HistoryTransactionCommand { get; }
        public ICommand ProfileCommand { get; }

        private ModalDialogManager _modalDialogManager;

        private void Home(object view)
        {
            MenuSelectedIndex = 1; 
            CurrentView = new HomeVM(this);
        }

        private void ProductManagement(object view)
        {
            MenuSelectedIndex = 2;
            CurrentView = new ProductManagementVM(_modalDialogManager);
        }
        private void CategoryManagement(object view)
        {
            MenuSelectedIndex = 3;
            CurrentView = new CategoryManagementVM(_modalDialogManager);
        }
        private void CustomerManagement(object view)
        {
            MenuSelectedIndex = 4;
            CurrentView = new CustomerManagementVM(_modalDialogManager);
        }
        private void HistoryTransaction(object view)
        {
            MenuSelectedIndex = 5;
            //CurrentView = new TransactionVM(_modalDialogManager);
            _modalDialogManager.MessageEqueue("Hi Nama saya Vincent", null, promote: true, OnClick: (x)=>
            {
                MessageBox.Show("Hi");
                x.Dismiss();
            }, canClose:true);
            _modalDialogManager.MessageEqueue("Terima kasih sudah menggunakan aplikasi ini", isCanHit:false);
            _modalDialogManager.MessageEqueue("Silakan menikmati", OnClick: (x)=> { x.Dismiss(); });
            _modalDialogManager.MessageEqueue("Silakan dadjaoidj awdjoaidj awodijaoiwdj aiwdaoiwd adhoajd opiajdioaj adiojaoidj aowdnhaoudwh aoidnaoidj aoiwhduahdn aowhdou adhno awdiuahdi", TimeSpan.FromSeconds(10), OnClick: (x)=> { x.Dismiss(); });
        }

        private void Profile(object view)
        {
            
        }
        private void UserManagement(object view)
        {
            MenuSelectedIndex = 3;
            CurrentView = new UserManagementVM();
        }

        public NavigationVM(MainWindow window)
        {
            _window = window;
            HomeCommand = new RelayCommand(Home);
            UserManagementCommand = new RelayCommand(UserManagement);
            ProductManagementCommand = new RelayCommand(ProductManagement);
            CategoryManagementCommand = new RelayCommand(CategoryManagement);
            CustomerManagementCommand = new RelayCommand(CustomerManagement);
            HistoryTransactionCommand = new RelayCommand(HistoryTransaction);
            ProfileCommand = new RelayCommand(Profile);

            _modalDialogManager = new ModalDialogManager(_window.ModalContainer);

            MenuSelectedIndex = 1;
            CurrentView = new HomeVM(this);
        }
    }
}
