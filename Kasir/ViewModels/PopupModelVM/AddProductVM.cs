using Kasir.Commons.Commands;
using Kasir.Commons.Validations;
using Kasir.Utils.Controls;
using Kasir.Utils.Dialog;
using Kasir.Views.PopupModalView;
using System.Windows.Input;

namespace Kasir.ViewModels.PopupModelVM
{
    public class AddProductVM : PopupFormModelBase
    {
        private readonly ModalDialogManager _modalDialogManager;
        public ICommand BarcodeScanCommand { get; }
        public AddProductVM(ModalDialogManager modalDialog = null) : base()
        {
            _modalDialogManager = modalDialog;
            BarcodeScanCommand = new RelayCommand(BarcodeScan);
        }


        private void BarcodeScan(object parameter)
        {
            if (_modalDialogManager != null)
            {
                BarcodeScan barcodeScanner = new BarcodeScan();
                PopupModal modal = _modalDialogManager.CreateNewPopupModal("Scan barcode", barcodeScanner);
                barcodeScanner.BarcodeDetected += delegate(object sender, BarcodeDetectedEventArgs e)
                {
                    Barcode = e.BarcodeData;
                    modal.IsOpen = false;
                    barcodeScanner.Dispose();
                };
                modal.PopupClosed += delegate
                {
                    barcodeScanner.Dispose();
                };
                modal.IsOpen = true;
            }
            else
            {

            }
        }

        private string _productName;
        public string ProductName
        {
            get
            {
                RegisterError(new[] { new TextNotEmptyValidation() { FieldName = "Nama Produk", CustomErrorMessage = ValidationMessages.RequiredError } });
                return _productName;
            }
            set
            {
                ValidateColumn(value);
                _productName = value;
                OnPropertyChanged();
            }
        }

        private int _categoryId;
        public int CategoryId
        {
            get
            {
                return _categoryId;
            }
            set
            {
                _categoryId = value;
                OnPropertyChanged();
            }
        }

        private string _barcode;
        public string Barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        private int _stock;
        public int Stock
        {
            get
            {
                return _stock;
            }
            set
            {
                _stock = value;
                OnPropertyChanged();
            }
        }

        private object _price = 0;
        public object Price
        {
            get
            {
                RegisterError(new[] { new LongValidationRule() { FieldName = "Harga", Min = 0, MinMaxErrorMessage = ValidationMessages.MinMaxError, TypeErrorMessage = ValidationMessages.NumericError } });
                return _price;
            }
            set
            {
                ValidateColumn(value);
                _price = value;
                OnPropertyChanged();
            }
        }

        private object _promoPrice = 0;
        public object PromoPrice
        {
            get
            {
                RegisterError(new[] { new LongValidationRule() { FieldName = "Harga Promo", Min = 0, MinMaxErrorMessage = ValidationMessages.MinMaxError, TypeErrorMessage = ValidationMessages.NumericError } }); 
                return _promoPrice;
            }
            set
            {
                ValidateColumn(value);
                _promoPrice = value;
                OnPropertyChanged();
            }   
        }

        private string _notes;
        public string Notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
    }
}
