using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymApp.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        private readonly Action<object> _executeWithParam;
        private readonly Func<object, bool> _canExecuteWithParam;
        private readonly bool _isParameterized;
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _isParameterized = false;
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute;
            _canExecuteWithParam = canExecute;
            _isParameterized = true;
        }
        public bool CanExecute(object parameter)
        {
            if (_isParameterized)
                return _canExecuteWithParam?.Invoke(parameter) ?? true;
            else
                return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            if (_isParameterized)
                _executeWithParam(parameter);
            else
                _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
