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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IngosTech7._0 {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			string enteredText = textBoxEntered.Text;

			if (string.IsNullOrEmpty(enteredText) ||
				string.IsNullOrWhiteSpace(enteredText)) {
				textBoxResult.Text = string.Empty;
				MessageBox.Show(this, "Введите строку для вычисления", "", 
					MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}
			
			textBoxResult.Text = Parser.ParseString(enteredText);
		}

		private void Button_Click_Info(object sender, RoutedEventArgs e) {
			WindowAbout windowAbout = new WindowAbout() { Owner = this };
			windowAbout.ShowDialog();
		}
	}
}
