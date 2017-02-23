using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Infrastructure
{
	public class DelegateCommand<T> : ICommand, IExecuted
		where T:class
	{
		private readonly Predicate<T> _canExecute;
		private readonly Action<T> _execute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action<T> execute,
					   Predicate<T> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null)
			{
				return true;
			}

			return _canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			_execute((T)parameter);
			if (CommandExecuted != null)
				CommandExecuted(this, EventArgs.Empty);
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler CommandExecuted;
	}

	public class DelegateCommand : ICommand, IExecuted
	{
		private readonly Predicate<object> _canExecute;
		private readonly Action<object> _execute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<object> execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action<object> execute,
					   Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute == null)
			{
				return true;
			}

			return _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
			if (CommandExecuted != null)
				CommandExecuted(this, EventArgs.Empty);
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler CommandExecuted;
	}
}
