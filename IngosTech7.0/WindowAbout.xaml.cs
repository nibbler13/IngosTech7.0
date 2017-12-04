using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IngosTech7._0 {
	/// <summary>
	/// Логика взаимодействия для WindowAbout.xaml
	/// </summary>
	public partial class WindowAbout : Window {
		public WindowAbout() {
			InitializeComponent();

			textBoxAbout.Text = Properties.Resources.StringAbout;
		}
	}
}
