using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;

namespace BoxMaker
{
    /// <summary>
    /// Interaction logic for ProductionStatsView.xaml
    /// </summary>
    public partial class WorkerView : UserControl
    {
		public WorkerView()
        {
			InitializeComponent();
        }

		/// <summary>
		/// Clicking the Allow Editing button allows the user to change the name of
		/// the Worker. I decided not to use Command binding for this button because this
		/// action does not directly affect the Model. 
		/// </summary>
		private void m_AllowEditButton_Click( object sender, RoutedEventArgs e )
		{
			if( m_WorkerNameTextBox.IsEnabled )
			{
				m_WorkerNameTextBox.IsEnabled = false;
				m_AllowEditButton.Content = "Allow Editing";
			}
			else
			{
				m_WorkerNameTextBox.IsEnabled = true;
				m_AllowEditButton.Content = "Finish Editing";
			}
		}
    }
}
