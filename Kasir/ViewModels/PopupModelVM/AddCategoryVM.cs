using Kasir.Commons;
using Kasir.Commons.Commands;
using Kasir.Commons.Validations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Kasir.ViewModels.PopupModelVM
{
    public class AddCategoryVM : PopupFormModelBase
    {
        public AddCategoryVM() : base()
        {
        }

        private string _categoryName;

        public string CategoryName
		{
			get
			{
                RegisterError(new ValidationRule[] { new TextNotEmptyValidation() { FieldName = "Nama Kategori", CustomErrorMessage = ValidationMessages.RequiredError } });
                return _categoryName;
			}
			set
			{
                ValidateColumn(value);
                _categoryName = value;
                OnPropertyChanged();
            }
        }
    }
}
