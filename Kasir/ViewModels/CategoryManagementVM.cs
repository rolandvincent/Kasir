using Kasir.Commons;
using Kasir.Commons.Commands;
using Kasir.Commons.Input;
using Kasir.DbContexts;
using Kasir.Model;
using Kasir.ModelLinker;
using Kasir.Services.ProductProviders;
using Kasir.Utils.Controls;
using Kasir.Utils.Dialog;
using Kasir.ViewModels.PopupModelVM;
using Kasir.Views.PopupModalView;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasir.ViewModels
{
    public class CategoryManagementVM : ViewModelBase, IPagination
    {
        private IEnumerable<Category> _categories;
        public IEnumerable<Category> Categories => _categories;

        private readonly ModalDialogManager _modalDialogManager;
        private DelayedEvent _searchDelay;

        public event EventHandler OnCategoryListChanged;
        public ICommand NewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand FirstCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LastCommand { get; }

        public CategoryManagementVM(ModalDialogManager modalDialogManager)
        {
            NewCommand = new RelayCommand(NewCategory);
            DeleteCommand = new RelayCommand(DeleteCategory);
            EditCommand = new RelayCommand(EditCategory);
            FirstCommand = new RelayCommand(FirstPage);
            NextCommand = new RelayCommand(NextPage);
            PreviousCommand = new RelayCommand(PreviousPage);
            LastCommand = new RelayCommand(LastPage);

            _modalDialogManager = modalDialogManager;
            _searchDelay = new DelayedEvent(TimeSpan.FromSeconds(0.3));
            _searchDelay.EventTriggered += _searchDelay_EventTriggered;

            OnCategoryListChanged += CategoryManagementVM_OnCategoryListChanged; 
            UpdateData();
        }

        private async void _searchDelay_EventTriggered(object? sender, EventArgs e)
        {
            await UpdateData();
            if (string.IsNullOrEmpty(SearchText))
                CurrentPage = 1;
        }

        protected void LastPage(object obj)
        {
            CurrentPage = NumberOfPage;
        }

        protected void PreviousPage(object obj)
        {
            CurrentPage -= 1;
        }

        protected void NextPage(object obj)
        {
            CurrentPage += 1;
        }

        protected void FirstPage(object obj)
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
                        IsLoading = true;
                        _categories = await context.Categories.OrderBy(x => x.Id).Skip((CurrentPage - 1) * SelectedRecord).Take(SelectedRecord).ToListAsync();
                        OnCategoryListChanged?.Invoke(this, EventArgs.Empty);
                        IsLoading = false;
                    }
                });
            }
            else
            {
                await Task.Run(async () =>
                {
                    using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
                    {
                        IsLoading = true;
                        _categories = await context.Categories.Where((x)=>x.Name.ToLower().Contains(SearchText.ToLower())).Skip((CurrentPage - 1) * SelectedRecord).Take(SelectedRecord).ToListAsync();
                        OnCategoryListChanged?.Invoke(this, EventArgs.Empty);
                        IsLoading = false;
                    }
                });
            }

            UpdateRecordCount();
        }

        private void CategoryManagementVM_OnCategoryListChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Categories));
        }

        public void NewCategory(object view)
        {
            var AddProduct = new AddCategoryVM();
            AddProduct.AcceptClick += AddCategory_ApplyClick;
            var popupModal = _modalDialogManager.CreateNewPopupModal("Tambah Kategori", AddProduct);
            AddProduct.CancelClick += delegate
            {
                popupModal.IsOpen = false;
                AddProduct.Dispose();
                popupModal.Dispose();
                GC.Collect();
            };
            popupModal.IsOpen = true; 
        }

        public async void DeleteCategory(object parameter)
        {
            Category category = (Category) parameter;
            using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(new string[] { }))
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            _modalDialogManager.MessageQueueClear();
            _modalDialogManager.MessageEqueue("Data dihapus!");
            await UpdateData();
        }

        public void EditCategory(object parameter)
        {

        }

        private async void AddCategory_ApplyClick(object? sender, AcceptEventArgs e)
        {
            if (e.HasError)
            {
                _modalDialogManager.MessageEqueue("Tolong periksa kembali input data anda!", TimeSpan.FromSeconds(2), OnClick: (x) => { x.Dismiss(); });
            }
            else
            {
                try
                {
                    var addCategory = sender as AddCategoryVM;
                    using (iCassierDbContext context = new iCassierDbContextFactory().CreateDbContext(Array.Empty<string>()))
                    {
                        if (context.Categories.Any((x => x.Name.ToLower() == addCategory.CategoryName.ToLower()))){
                            _modalDialogManager.MessageEqueue("Data sudah ada!", OnClick: (x) => x.Dismiss(), promote: true);
                            _modalDialogManager.MessageQueueDropUntil(3);
                        }
                        else
                        {
                            await context.Categories.AddAsync(new Category()
                            {
                                Name = addCategory.CategoryName
                            });
                            context.SaveChanges();
                            _modalDialogManager.MessageEqueue("Data ditambahkan!", OnClick: (x) => x.Dismiss(), promote: true);
                            _modalDialogManager.MessageQueueDropUntil(3);
                            await UpdateData();
                        }            
                    }            
                }
                catch (DbUpdateException ex)
                {
                    _modalDialogManager.MessageEqueue((ex.InnerException?.Message.Contains("Duplicate") == true ? "Gagal menambahkan data. Data sudah ada pada database." : ex.InnerException?.Message ?? "Unknown"), OnClick: (x) => x.Dismiss(), promote: true);
                    _modalDialogManager.MessageQueueDropUntil(3);
                }
            }
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
                    TotalItems = context.Categories.Count();
                else
                    TotalItems = context.Categories.Where((x) => x.Name.ToLower().Contains(SearchText.ToLower())).Count();
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
