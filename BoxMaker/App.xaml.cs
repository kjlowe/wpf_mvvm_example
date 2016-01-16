using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using Model;

namespace BoxMaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
	{
        /// <summary>
        /// This is the entry point of the application.
		/// If you are wondering how the MainWindow is created look in App.xaml
        /// </summary>
        public App()
        {
			// Initialize the engine. The engine starts up the buisness logic.
            Task.Factory.StartNew(Engine.Initialize);
        }
    }
}
