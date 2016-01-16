using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Model
{
    /// <summary>
    /// This is where we do what makes the application valuable. The Worker
    /// executes our "business logic" (http://en.wikipedia.org/wiki/Business_logic)
    /// </summary>
    public class Worker
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

        #region Properties that support data binding
        private volatile int m_BoxesMade = 0;
        public int BoxesMade
        {
            get { return m_BoxesMade; }
            set
            {
                if (m_BoxesMade != value)
                {
                    m_BoxesMade = value;
                    NotifyPropertyChanged("BoxesMade");
                }
            }
        }

		private volatile String m_Name = "Worker";
		public String Name
		{
			get { return m_Name; }
			set
			{
				if( m_Name != value )
				{
					m_Name = value;
					NotifyPropertyChanged( "Name" );
				}
			}
		}

		private Boolean m_Running = false;
		public Boolean Running
		{
			get { return m_Running; }
			set
			{
				if( m_Running != value )
				{
					m_Running = value;
					NotifyPropertyChanged( "Running" );
				}
			}
		}

        #endregion

		private Thread m_Thread = null;

        // Constructor
		public Worker() { }

        /// <summary>
        /// Starts the worker thread.
        /// </summary>
        public void Start()
        {
			Running = true;
			m_Thread = new Thread( BoxMakingThread );
			m_Thread.Start();
        }

        /// <summary>
        /// Stops the worker thread.
        /// </summary>
        public void Stop()
        {
			Running = false;
        }

        /// <summary>
        /// This is the most important thread in the application. Here it's "making boxes", but
        /// in a real system, it would do all the heavy processing like connecting to cameras,
        /// setting the camera parameters and then runing the algorithms. If this is a time 
        /// senstive application this thread should be very high priority.
        /// </summary>
        private void BoxMakingThread()
        {
            do
            {
                BoxesMade += 1;
                System.Threading.Thread.Sleep(10);

			} while( Running );
        }
    }
}
