using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Common
{
	public class RelayCommand : ICommand
	{
		#region Fields

		readonly Action<object> m_Execute;
		readonly Predicate<object> m_CanExecute;

		#endregion // Fields

		#region Constructors

		public RelayCommand( Action<object> execute )
			: this( execute, null )
		{
		}

		public RelayCommand( Action<object> execute, Predicate<object> canExecute )
		{
			if( execute == null )
				throw new ArgumentNullException( "execute" );

			m_Execute = execute;
			m_CanExecute = canExecute;
		}
		#endregion // Constructors

		#region ICommand Members

		[DebuggerStepThrough]
		public bool CanExecute( object parameter )
		{
			return m_CanExecute == null ? true : m_CanExecute( parameter );
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute( object parameter )
		{
			m_Execute( parameter );
		}

		#endregion // ICommand Members
	}
}
