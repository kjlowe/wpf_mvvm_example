using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using System.Threading;
using Model;
using Common;

namespace BoxMaker
{
    /// <summary>
    /// This class translates data from the Model into data that can be bound to
    /// from the UI View. It's useful for doing things like unit conversion, or
	/// language translation. Another scenario: If a view wants to display data 
	/// from 10 various classes in the Model, this ViewModel could listen and 
	/// collect that data. All the view has to do it bind to it here.
    /// </summary>
    class WorkerViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        private Dispatcher m_UIDispatcher;
		private Worker m_MyWorker;

        /// <summary>
        /// The values of these properties originate in the Model. This is just a copy which 
        /// updates each time the value in the Model changes. The user interface binds to 
        /// these values, not the values in the Model.
        /// </summary>
		
		#region Properties that support binding

		private int m_BoxesMade = 0;
		public int BoxesMade
		{
			get
			{
				return m_BoxesMade;
			}
			set
			{
				if( m_BoxesMade != value )
				{
					m_BoxesMade = value;
					NotifyPropertyChanged( "BoxesMade" );
				}
			}
		}

		private String m_WorkerName = "";
		public String WorkerName
		{
			get
			{
				return m_WorkerName;
			}
			set
			{
				if( m_WorkerName != value )
				{
					m_WorkerName = value;
					NotifyPropertyChanged( "WorkerName" );

					if( m_MyWorker != null )
					{
						m_MyWorker.Name = value;
					}
				}
			}
		}

		private Boolean m_Running = false;
		public Boolean Running
		{
			get
			{
				return m_Running;
			}
			set
			{
				if( m_Running != value )
				{
					m_Running = value;
					NotifyPropertyChanged( "Running" );
				}
			}
		}

		private RelayCommand m_StartCommand;
		public ICommand StartCommand
		{
			get
			{
				if( m_StartCommand == null )
				{
					m_StartCommand = new RelayCommand( parm => this.Start(), param => !this.Running );
				}
				return m_StartCommand;
			}
		}

		private RelayCommand m_StopCommand;
		public ICommand StopCommand
		{
			get
			{
				if( m_StopCommand == null )
				{
					m_StopCommand = new RelayCommand( parm => this.Stop(), param => this.Running );
				}
				return m_StopCommand;
			}
		}

		#endregion

		/// <summary>
		/// Constructor. So we have have a dummy instance in the xaml.
		/// </summary>
		public WorkerViewModel()
		{
		}

        /// <summary>
		/// This initialization function must be called on the UI thread. It registers 
		/// listeners on Model classes so we know where there is new information to display.
        /// </summary>
		public void InitializeBindings( Worker worker )
		{
			// This view model is responsible for pulling information from worker.
			m_MyWorker = worker;

			// This object must be created on the UI thread
			m_UIDispatcher = Dispatcher.CurrentDispatcher;

			// Listen to property changes in the worker
			m_MyWorker.PropertyChanged += new PropertyChangedEventHandler( OnSourcePropertyChangedSourceThread );

			OnSourcePropertyChangedSourceThread( m_MyWorker, new PropertyChangedEventArgs( "Name" ) );
		}

        /// <summary>
        /// This callback happens when a Model class updates one of it properties. This callback will be on 
        /// the Model thread, and it's purpose it to transfer the event to the UI thread.
        /// </summary>
		private void OnSourcePropertyChangedSourceThread( object sender, PropertyChangedEventArgs e )
		{
			// Ensure that we are on the UI Thread
			// All objects bound to the UI must be created on the UI thread
			m_UIDispatcher.BeginInvoke( DispatcherPriority.Normal, ( ThreadStart )delegate { OnSourcePropertyChangedUIThread( sender, e ); } );
		}

        /// <summary>
        /// This callback is on the UI thread. By it's parameters it knows which property changed, so it
        /// copies the new value to the local copy. The UI is bound to this local copy.
        /// </summary>
		private void OnSourcePropertyChangedUIThread( object sender, PropertyChangedEventArgs e )
		{
			// If we were binding to multiple classes in the Model it would be helpful to check
			// who is sending this update.
			if( sender == m_MyWorker )
			{
				switch( e.PropertyName )
				{
					case "BoxesMade":
						BoxesMade = m_MyWorker.BoxesMade;
						break;
					case "Name":
						WorkerName = m_MyWorker.Name;
						break;
					case "Running":
						Running = m_MyWorker.Running;
						break;
				}
			}
		}

		/// <summary>
		/// When a button binds to StartCommand, clicking it calls this function. 
		/// </summary>
		public void Start()
		{
			m_MyWorker.Start();
		}

		/// <summary>
		/// When a button binds to StopCommand, clicking it calls this function. 
		/// </summary>
		public void Stop()
		{
			m_MyWorker.Stop();
		}
	}
}
