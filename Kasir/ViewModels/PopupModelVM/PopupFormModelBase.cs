using Kasir.Commons;
using Kasir.Commons.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kasir.ViewModels.PopupModelVM
{
    public class AcceptEventArgs: EventArgs
    {
        public bool HasError { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public AcceptEventArgs(bool hasError, Dictionary<string, List<string>>? errors = null)
        {
            HasError = hasError;
            Errors = errors;
        }
    }

    public abstract class PopupFormModelBase : ViewModelBase, IPopupModel, INotifyDataErrorInfo, IDataErrorInfo
    {
        public PopupFormModelBase() : base()
        {
            ErrorList = new Dictionary<string, List<string>>();
            CancelCommand = new RelayCommand(Cancel);
            AcceptCommand = new RelayCommand(Accept, (x)=> CanAccept(x));
        }

        protected Dictionary<string, List<string>> ErrorList { get; set; }

        public ICommand CancelCommand { get; }
        public ICommand AcceptCommand { get; }

        public bool HasErrors => ErrorList.Count() > 0;

        private bool FirstValidation = false;

        public string Error => null;

        public string this[string columnName] => string.Join('\n', GetErrors(columnName).Cast<string>());
        public Dictionary<string, ValidationRule[]> registeredErrors = new Dictionary<string, ValidationRule[]>();

        public event EventHandler CancelClick;
        public event IPopupModel.AcceptEventHandler AcceptClick;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected virtual bool CanAccept(object x)
        {
            return !HasErrors && FirstValidation;
        }

        protected virtual void Cancel(object View)
        {
            CancelClick?.Invoke(this, new EventArgs());
        }

        protected virtual void Accept(object View)
        {
            ValidateAllErrors();
            if (CanAccept(AcceptCommand))
                AcceptClick?.Invoke(this, new AcceptEventArgs(false));
            else
                AcceptClick?.Invoke(this, new AcceptEventArgs(true, ErrorList));
        }
        protected void ValidateAllErrors()
        {
            if (this == null) throw new Exception("Form class is not registered");
            foreach (var registerError in registeredErrors)
            {
                var prop = GetType().GetProperties();
                ValidateColumn(prop.First(x => x.Name == registerError.Key).GetValue(this), registerError.Key);
            }
        }

        protected void OnErrorChanged([CallerMemberName] string? propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors([CallerMemberName] string? propertyName = null)
        {
            ErrorList.Remove(propertyName);
        }

        private void AddError(string error, [CallerMemberName] string? propertyName = null)
        {
            if (!ErrorList.Keys.Contains(propertyName))
                ErrorList.Add(propertyName, new List<string>());

            ErrorList[propertyName].Add(error);
        }

        public IEnumerable GetErrors([CallerMemberName] string? propertyName = null)
        {
            return ErrorList.GetValueOrDefault(propertyName, new List<string>());
        }

        protected void RegisterError(ValidationRule[] validation_rule, [CallerMemberName] string? propertyName = null)
        {
            registeredErrors.TryAdd(propertyName, validation_rule);
        }

        protected bool ValidateColumn(object value, [CallerMemberName] string? propertyName = null)
        {
            FirstValidation = true;
            var hasError = false;
            ClearErrors(propertyName);

            if (CheckError(value, out string[] errors, registeredErrors[propertyName]))
            {
                foreach (var error in errors)
                    AddError(error, propertyName);
                hasError = true;
            }
            OnErrorChanged(propertyName);
            return hasError;
        }

        public bool CheckError(object value, out string[] errors, params ValidationRule[] validation_rule)
        {
            List<string> Errors = new List<string>();
            bool errorAny = false;
            foreach(var rule in validation_rule)
            {
                var result = rule.Validate(value, CultureInfo.CurrentCulture);
                if (!result.IsValid)
                {
                    if (!errorAny) errorAny = true;
                    Errors.Add(result.ErrorContent.ToString());
                }
            }
            errors = Errors.ToArray();
            return errorAny;
        }

        public override void Dispose()
        {
            ErrorList.Clear();
            base.Dispose();
        }
    }
}
