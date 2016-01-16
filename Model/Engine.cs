using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

/// <summary>
/// The Model project is a library with all the valuable business logic. It's just a library,
/// so it cannot be run on it's own. In this solution the BoxMaker user interface instantiates
/// the Model but it would be very easy to make a console or Windows Phone application that 
/// uses the Model as well. Not that you would do this, but keeping the separation makes maintaining
/// code MUCH easier.
/// 
/// The entry point of the application is App.xaml.cs in the BoxMaker project.
/// </summary>
namespace Model
{
    /// <summary>
    /// The Engine runs the core funcionality of the application, also known as the 
    /// business logic or Model.
    /// </summary>
    public class Engine
    {
        // Singleton Object 
        private static readonly Engine Instance = new Engine();

		private static AutoResetEvent m_DelayHandle = new AutoResetEvent( false );

		#region INotifyPropertyChanged implementation
		public static event PropertyChangedEventHandler PropertyChanged;

		private static void NotifyPropertyChanged( String info )
		{
			if( PropertyChanged != null )
			{
				PropertyChanged( Instance, new PropertyChangedEventArgs( info ) );
			}
		}
		#endregion

 		/// <summary>
		/// List of all the workers doing all the important work for our appliation.
		/// </summary>
		public static ObservableCollection<Worker> Workers { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		private Engine()
		{
			Workers = new ObservableCollection<Worker>();
		}

		/// <summary>
		/// This methods starts the Model. If we don't call this function, all we'll have
		/// is the user interface. There won't be anything valuable happening underneath.
		/// </summary>
		public static void Initialize()
		{
			// Event allows us to interrupt initialization
			if( m_DelayHandle.WaitOne( 1000 ) )
			{
				return;
			}

			for(int i = 0; i < 5; ++i)
			{
				Worker worker = new Worker();
				worker.Name = "Worker " + (i + 1).ToString();
				Workers.Add( worker );

				// Event allows us to interrupt initialization
				if( m_DelayHandle.WaitOne( 1000 ) )
				{
					return;
				}
			}
		}

		/// <summary>
		/// This method releases all elements in the Model.
		/// </summary>
		public static void Terminate()
		{
			// Interrupt initialization if it's still running.
			m_DelayHandle.Set();

			// Stop the worker threads
			foreach( Worker worker in Workers )
			{
				if( worker != null && worker.Running )
				{
					worker.Stop();
				}
			}
		}
    }
}
 