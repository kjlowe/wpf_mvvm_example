using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.Text;

namespace Common
{
	public class ObservableTabItemCollection : ObservableCollection<TabItem>
	{
		public ObservableTabItemCollection() : base() { }
	}
}
