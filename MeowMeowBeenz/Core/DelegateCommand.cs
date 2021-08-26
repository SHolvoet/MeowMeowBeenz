using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeowMeowBeenz.Core
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecuteAction;

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public DelegateCommand(Action<T> executeAction)
            : this(executeAction, null)
        { }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeAction((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteAction == null)
            {
                return true;
            }

            return _canExecuteAction((T)parameter);
        }

        public void InvokeCanExecuteChanged()
        {
            var changeEvent = CanExecuteChanged;
            if (changeEvent != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public class DelegateCommand : DelegateCommand<object>
    {

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction) : base(executeAction, canExecuteAction)
        {
        }

        public DelegateCommand(Action<object> executeAction) : base(executeAction)
        {
        }

        public DelegateCommand(Action executeAction) : base(_ => executeAction())
        {
        }
    }
}
