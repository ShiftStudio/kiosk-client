using System;
using System.Windows.Input;

namespace dimigo_meal.Common
{
    public class DelegateCommand<T> : ICommand
    {
        public Action<T> executeMethod = null;
        public Func<T, bool> canExecuteMethod = null;

        public DelegateCommand() { }

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(T parameter)
        {
            if (canExecuteMethod == null) return true;
            return canExecuteMethod(parameter);
        }
        public void Execute(T parameter)
        {
            if (executeMethod != null)
                executeMethod(parameter);
        }

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            if (this.executeMethod == null) return;
            Execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        #endregion
    }
}
