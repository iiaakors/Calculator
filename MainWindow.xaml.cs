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
using CalcLib;

namespace CalculatorApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = Calculator;
			Loaded += new RoutedEventHandler(MainWindow_Loaded);
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			foreach (var item in Calculator.InputButtons)
				InputBindings.Add(new InputBinding(item.InputExecutedCommand, new KeyGesture(item.Key)));
			foreach (var item in Calculator.OperatorButtons)
				InputBindings.Add(new InputBinding(item.InputExecutedCommand, new KeyGesture(item.Key)));
			InputBindings.Add(new InputBinding(Calculator.ClearCommand, new KeyGesture(Key.Escape)));
			InputBindings.Add(new InputBinding(Calculator.ClearEntryCommand, new KeyGesture(Key.Back)));
		}

		private Calculator calculator;
		public Calculator Calculator
		{
			get { return (calculator ?? (calculator = new Calculator())); }
		}
	}
}
