using Kasir.Commons;
using Kasir.Commons.Commands;
using Kasir.Commons.Input;
using Kasir.DbContexts;
using Kasir.Model;
using Kasir.ModelLinker;
using Kasir.Services.ProductCreators;
using Kasir.Services.ProductProviders;
using Kasir.Utils.Controls;
using Kasir.Utils.Dialog;
using Kasir.ViewModels.PopupModelVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kasir.ViewModels
{
    public class ProductManagementVM : ViewModelBase, IPagination
    {
        private IEnumerable<ProductLnk> _products;
        public IEnumerable<ProductLnk> Products => _products;

        private readonly ModalDialogManager _modalDialogManager;
        private DelayedEvent _searchDelay;

        public event EventHandler OnProductListChanged;

        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FirstCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LastCommand { get; }

        public ProductManagementVM(ModalDialogManager modalDialogManager)
        {
            NewCommand = new RelayCommand(NewProduct);
            DeleteCommand = new RelayCommand(DeleteProduct);
            EditCommand = new RelayCommand(EditProduct);
            FirstCommand = new RelayCommand(FirstPage);
            NextCommand = new RelayCommand(NextPage);
            PreviousCommand = new RelayCommand(PreviousPage);
            LastCommand = new RelayCommand(LastPage);

            _modalDialogManager = modalDialogManager;
            _searchDelay = new DelayedEvent(TimeSpan.FromSeconds(0.3));
            _searchDelay.EventTriggered += _searchDelay_EventTriggered;

            OnProductListChanged += ProductManagementVM_OnProductListChanged;
            UpdateData();
        }

        private async void _searchDelay_EventTriggered(object? sender, EventArgs e)
        {
            await UpdateData();
            if (string.IsNullOrEmpty(SearchText))
                CurrentPage = 1;
        }

        private void LastPage(object obj)
        {
            CurrentPage = NumberOfPage;
        }

        private void PreviousPage(object obj)
        {
            CurrentPage -= 1;
        }

        private void NextPage(object obj)
        {
            CurrentPage += 1;
        }

        private void FirstPage(object obj)
        {
            CurrentPage = 1;
        }

        private async Task UpdateData()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                await Task.Run(async () =>
                {
                    using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
                    {
                        using (DatabaseProductProvider productProvider = new DatabaseProductProvider(context))
                        {
                            IsLoading = true;
                            _products = productProvider.ConvertAll(await context.Products.OrderBy(x => x.Id).Skip((CurrentPage - 1) * SelectedRecord).Take(SelectedRecord).ToListAsync());
                            OnProductListChanged?.Invoke(this, EventArgs.Empty);
                            IsLoading = false;
                        }
                    }
                });
            }
            else
            {
                await Task.Run(async () =>
                {
                    using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
                    {
                        using (DatabaseProductProvider productProvider = new DatabaseProductProvider(context))
                        {
                            IsLoading = true;
                            _products = productProvider.ConvertAll(await context.Products.Where((x) => x.Name.ToLower().Contains(SearchText.ToLower())).Skip((CurrentPage - 1) * SelectedRecord).Take(SelectedRecord).ToListAsync());
                            OnProductListChanged?.Invoke(this, EventArgs.Empty);
                            IsLoading = false;
                        }
                    }
                });
            }

            UpdateRecordCount();
        }

        private void ProductManagementVM_OnProductListChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Products));
        }

    
        public void NewProduct(object view)
        {
            var AddProduct = new PopupModelVM.AddProductVM(_modalDialogManager);
            AddProduct.AcceptClick += AddProduct_AcceptClick; ;

            PopupModal popupModal = _modalDialogManager.CreateNewPopupModal("Tambah Produk", AddProduct);
            AddProduct.CancelClick += delegate {
                popupModal.IsOpen = false;
                AddProduct.Dispose();
                popupModal.Dispose();
                GC.Collect();
            };
            popupModal.IsOpen = true;
        }

        private async void AddProduct_AcceptClick(object sender, PopupModelVM.AcceptEventArgs args)
        {
            if (args.HasError)
            {
                _modalDialogManager.MessageEqueue("Lengkapi data terlebih dahulu!", promote:true);
                string errors = "";
                foreach (var errList in args.Errors)
                {
                    foreach (var err in errList.Value)
                        errors += err + '\n';
                }
                var modal = _modalDialogManager.CreateNewPopupModal("Error", new TextBlock() { Text = string.Join('\n', errors), Foreground= new SolidColorBrush(Colors.White), FontSize=16, Margin=new Thickness(20) });
                modal.IsOpen = true;
            }
            else
            {
                using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
                {
                    var productVM = sender as AddProductVM;
                    if (!string.IsNullOrEmpty(productVM.Barcode) && context.Products.Any(x => x.Barcode == productVM.Barcode))
                    {
                        _modalDialogManager.MessageEqueue("Kode barcode telah digunakan sebelumnya.");
                        return;
                    }
                    else if (!await context.Categories.ContainsAsync(productVM.SelectedCategory))
                    {
                        _modalDialogManager.MessageEqueue("Kategori tidak terdaftar");
                        return;
                    }
                    else
                    {
                        try
                        {
                            await context.Products.AddAsync(new Product()
                            {
                                Barcode = string.IsNullOrEmpty(productVM.Barcode) ? null : productVM.Barcode,
                                CategoryID = productVM.SelectedCategory.Id,
                                Name = productVM.ProductName,
                                Notes = string.IsNullOrEmpty(productVM.Notes) ? null : productVM.Notes,
                                Price = (long)productVM.Price,
                                PromoPrice = (long?)productVM.PromoPrice,
                            });
                        }
                        catch (Exception ex)
                        {
                            _modalDialogManager.MessageEqueue("Error. " + ex.InnerException?.Message);
                            return;
                        }
                        await context.SaveChangesAsync();

                        _modalDialogManager.MessageEqueue("Data ditambahkan.");
                        OnProductListChanged?.Invoke(sender, EventArgs.Empty);
                    }
                }
            }   
        }

        public async void DeleteProduct(object parameter)
        {
            ProductLnk produk = (ProductLnk)parameter;
            using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
            {
                context.Products.Remove(new DatabaseProductCreator(context).ToProduct(produk));
                await context.SaveChangesAsync();
            }
            _modalDialogManager.MessageQueueClear();
            _modalDialogManager.MessageEqueue("Data dihapus!");
            await UpdateData();
        }

        public void EditProduct(object parameter)
        {

        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value > NumberOfPage) value = NumberOfPage;
                if (value <= 0) value = 1;
                _currentPage = value;
                UpdateData();
                OnPropertyChanged();
                UpdateButtonState();
            }
        }

        private void UpdateButtonState()
        {
            IsPreviousEnabled = CurrentPage > 1;
            IsNextEnabled = CurrentPage < NumberOfPage;
        }

        private int _numberOfPage = 0;
        public int NumberOfPage
        {
            get
            {
                return _numberOfPage;
            }
            set
            {
                _numberOfPage = value;
                OnPropertyChanged();
                UpdateButtonState();
            }
        }

        private int _selectedRecord = 15;
        public int SelectedRecord
        {
            get
            {
                return _selectedRecord;
            }
            set
            {
                _selectedRecord = value;
                OnPropertyChanged();
                UpdateData();
            }
        }

        private bool _isNextEnabled;
        public bool IsNextEnabled
        {
            get
            {
                return _isNextEnabled;
            }
            set
            {
                _isNextEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _isPreviousEnabled;
        public bool IsPreviousEnabled
        {
            get
            {
                return _isPreviousEnabled;
            }
            set
            {
                _isPreviousEnabled = value;
                OnPropertyChanged();
            }
        }

        private int _totalItem;
        public int TotalItems
        {
            get
            {
                return _totalItem;
            }
            set
            {
                _totalItem = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _searchDelay.ResetAndStart();
            }
        }

        private void UpdateRecordCount()
        {
            using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(Array.Empty<string>()))
            {
                if (string.IsNullOrEmpty(SearchText))
                    TotalItems = context.Products.Count();
                else
                    TotalItems = context.Products.Where((x) => x.Name.ToLower().Contains(SearchText.ToLower())).Count();
                var newValue = (int)Math.Ceiling(TotalItems / (double)SelectedRecord);
                newValue = newValue == 0 ? 1 : newValue;
                if (NumberOfPage != newValue)
                {
                    int LastTotalPage = NumberOfPage;
                    NumberOfPage = newValue;
                    if (CurrentPage > newValue)
                        CurrentPage = newValue;
                }
            }
        }
    }
}
