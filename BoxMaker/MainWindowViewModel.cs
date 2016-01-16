using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using System.Threading;
using Model;
using Common;

namespace BoxMaker
{
	class MainWindowViewModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged( String info )
		{
			if( PropertyChanged != null )
			{
				PropertyChanged( this, new PropertyChangedEventArgs( info ) );
			}
		}
		#endregion

		private Dispatcher m_UIDispatcher;
		private Dictionary<Worker, TabItem> m_TabItemsDictionary = new Dictionary<Worker, TabItem>();
		public ObservableTabItemCollection TabItems { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public MainWindowViewModel()
		{
			TabItems = new ObservableTabItemCollection();
			InitializeBindings();
		}

		/// <summary>
		/// InitializeBindings registers listeners on Model classes such that callbacks will be on the UI thread.
		/// </summary>
		public void InitializeBindings()
		{
			// This object must be created on the UI thread
			m_UIDispatcher = Dispatcher.CurrentDispatcher;

			// Hook up event to detect when workers have been added or removed
			Engine.Workers.CollectionChanged += new NotifyCollectionChangedEventHandler( Engine_OnWorkerCollectionChangedSourceThread );
		}

		/// <summary>
		/// The worker collection has changed (on engine thread)
		/// </summary>
		private void Engine_OnWorkerCollectionChangedSourceThread( object sender, NotifyCollectionChangedEventArgs e )
		{
			// Change to the UI Thread.
			m_UIDispatcher.BeginInvoke( DispatcherPriority.Normal, ( ThreadStart )delegate { Engine_OnWorkerCollectionChangedUIThread( sender, e ); } );
		}

		/// <summary>
		/// The worker collection has changed (on UI thread)
		/// </summary>
		private void Engine_OnWorkerCollectionChangedUIThread( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
				{
					// Check for new workers
					foreach( Worker worker in e.NewItems )
					{
						if( !m_TabItemsDictionary.ContainsKey( worker ) )
						{
							CreateWorkerTabItem( worker );
						}
					}
					break;
				}
				case NotifyCollectionChangedAction.Remove:
				{
					// Check for workers that were removed
					foreach( KeyValuePair<Worker, TabItem> pair in e.OldItems )
					{
						Worker worker = pair.Key as Worker;
						if( !e.NewItems.Contains( worker ) && m_TabItemsDictionary.ContainsKey( worker ) )
						{
							RemoveWorkerTabItem( worker );
						}
					}					
					break;
				}
			}
		}

		/// <summary>
		/// Creates the user interface elements for a Worker
		/// </summary>
		private void CreateWorkerTabItem( Worker worker )
		{
			// Create UI objects for this worker
			WorkerView view = new WorkerView();
			WorkerViewModel viewModel = new WorkerViewModel();
			TabItem tabItem = new TabItem();

			// Connect events, bindings and UI elements
			viewModel.InitializeBindings( worker ); ;
			view.DataContext = viewModel;
			tabItem.Header = viewModel.WorkerName;
			tabItem.Content = view;
			tabItem.DataContext = viewModel;
			tabItem.SetBinding( TabItem.HeaderProperty, "WorkerName" );

			// Add the Tab to the UI
			m_TabItemsDictionary[ worker ] = tabItem;
			TabItems.Add( tabItem );
		}

		/// <summary>
		/// Removes user interface elements for a worker
		/// </summary>
		private void RemoveWorkerTabItem( Worker worker )
		{
			TabItems.Remove( m_TabItemsDictionary[ worker ] );
			m_TabItemsDictionary.Remove( worker );
		}

		/// <summary>
		/// Shuts down the model
		/// </summary>
		public void TerminateEngine()
		{
			Engine.Terminate();
		}
	}
}
 