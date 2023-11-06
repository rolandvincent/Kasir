using System;
using System.Windows.Input;

namespace Kasir.Commons.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter);

        protected void OnCanExecuteChaned()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
