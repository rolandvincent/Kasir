using Kasir.Commons;
using Kasir.Utils.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.ViewModels
{
    public class TransactionMenuVM : ViewModelBase
    {
        public TransactionMenuVM()
        {
			
			Task.Run(async () =>
			{
				while (true)
				{
					TimeDateView = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
					await Task.Delay(1000);
				}
			});

			UserFullName = "Roland Vincent";
        }

		private string _timeDateView;
		public string TimeDateView
		{
			get
			{
				return _timeDateView;
			}
			set
			{
				_timeDateView = value;
				OnPropertyChanged();
			}
		}

		private string _userFullName;
		public string UserFullName
		{
			get
			{
				return _userFullName;
			}
			set
			{
				_userFullName = value;
				OnPropertyChanged();
			}
		}
	}
}
