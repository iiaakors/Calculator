using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Infrastructure;

namespace CalcLib
{
	public class Calculator : INotifyPropertyChanged
	{
		public string Input
		{
			get { return string.Join("", InputHistory.Select(i => (i is CalculatorValue ? i.Value : string.Format(" {0} ", i.Value))).ToArray()); }
		}

		private string result = "0";
		public string Result
		{
			get { return result; }
			set
			{
				result = value;
				FireChange("Result");
			}
		}

		private List<ICalculatorInput> inputHistory;
		private List<ICalculatorInput> InputHistory
		{
			get { return (inputHistory??(inputHistory = new List<ICalculatorInput>())); }
		}

		internal void ApplyInput(ICalculatorInput calculatorinput)
		{
			if (!ValidateInput(calculatorinput)) return;
			var input = InputHistory.LastOrDefault();
			if (input is CalculatorOperator && calculatorinput is CalculatorOperator)
			{
				InputHistory.RemoveAt(InputHistory.Count - 1);
				InputHistory.Add(calculatorinput);
				FireChange("Input");
				return;
			}
			
			if ((input == null || input is CalculatorOperator) && calculatorinput.Value == ".")
			{
				
					InputHistory.Add(InputButtons.FirstOrDefault(b=>b.Value == "0"));
					InputHistory.Add(calculatorinput);
			}
			else
			{
				
				InputHistory.Add(calculatorinput);
			}
			CurrentInput = Input.Replace(" ", "").Split(OperatorButtons.Select(b => b.Value).ToArray(), StringSplitOptions.RemoveEmptyEntries).Where(s => !string.IsNullOrEmpty(s)).LastOrDefault();
			FireChange("Input");
			if (calculatorinput is CalculatorOperator)
			{
				CurrentOperator = calculatorinput as CalculatorOperator;
				Calculate();
			}
		}

		private List<CalculatorInput> inputButtons;
		public List<CalculatorInput> InputButtons
		{
			get { return (inputButtons ?? (inputButtons = new List<CalculatorInput>() 
			{ 
				new CalculatorValue(this, "7", System.Windows.Input.Key.NumPad7),
				new CalculatorValue(this, "8", System.Windows.Input.Key.NumPad8),
				new CalculatorValue(this, "9", System.Windows.Input.Key.NumPad9),
				new CalculatorValue(this, "4", System.Windows.Input.Key.NumPad4),
				new CalculatorValue(this, "5", System.Windows.Input.Key.NumPad5),
				new CalculatorValue(this, "6", System.Windows.Input.Key.NumPad6),
				new CalculatorValue(this, "1", System.Windows.Input.Key.NumPad1),
				new CalculatorValue(this, "2", System.Windows.Input.Key.NumPad2),
				new CalculatorValue(this, "3", System.Windows.Input.Key.NumPad3),
				new CalculatorValue(this, "0", System.Windows.Input.Key.NumPad0),
				
				new CalculatorValue(this, ".", System.Windows.Input.Key.Decimal)
			})); }
		}

		private List<CalculatorInput> operatorButtons;
		public List<CalculatorInput> OperatorButtons
		{
			get
			{
				return (operatorButtons ?? (operatorButtons = new List<CalculatorInput>() 
			{ 
				new CalculatorOperator(this, "+", System.Windows.Input.Key.Add, OperatorTypes.Add),
				new CalculatorOperator(this, "-", System.Windows.Input.Key.Subtract, OperatorTypes.Subtract),
				new CalculatorOperator(this, "*", System.Windows.Input.Key.Multiply, OperatorTypes.Multiply),
				new CalculatorOperator(this, "/", System.Windows.Input.Key.Divide, OperatorTypes.Divide),
				new CalculatorOperator(this, "=", System.Windows.Input.Key.Enter, OperatorTypes.Equals)
			}));
			}
		}

		internal bool ValidateInput(ICalculatorInput calculatorinput)
		{
			if (calculatorinput == null) return false;
			var input = InputHistory.LastOrDefault();
			if (input == null && double.Parse(Result) == 0) return (calculatorinput is CalculatorValue || calculatorinput == null);
			if (input is CalculatorOperator && ((CalculatorOperator)input).OperatorType == OperatorTypes.Divide)
				return !(calculatorinput.Value == "0");
			var last = Input.Replace(" ", "").Split(OperatorButtons.Select(o => o.Value).ToArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
			if ((last == "0." || last == "0") && (calculatorinput is CalculatorOperator && ((CalculatorOperator)calculatorinput).OperatorType == OperatorTypes.Equals))
				return false;
			if ((calculatorinput.Value == "." || calculatorinput.Value == "0.") && last != null && last.Contains(".") && !(input is CalculatorOperator))
				return false;
			return true;
		}

		private CalculatorOperator CurrentOperator { get; set; }
		private string CurrentInput { get; set; }

		private void Calculate()
		{
			var calculation = Input.Replace(" ", "");
			var items = calculation.Split(OperatorButtons.Select(o => o.Value).ToArray(), StringSplitOptions.RemoveEmptyEntries);
			if (items.Count() < 2) return;
			List<CalculatorOperator> operators = new List<CalculatorOperator>();
			foreach (string s in calculation.Where(o => OperatorButtons.Select(b => b.Value).Contains(o.ToString())).Select(o => o.ToString()))
			{
				operators.Add(OperatorButtons.FirstOrDefault(o => o.Value == s) as CalculatorOperator);
			}
			int opindex = 0;
			string calcresult = null;
			foreach (var item in items)
			{
				if (calcresult == null)
				{
					calcresult = item;
					continue;
				}
				var op = operators[opindex];
				switch (op.OperatorType)
				{
					case OperatorTypes.Add:
						{
							calcresult = (double.Parse(calcresult) + double.Parse(item)).ToString();
						} break;

					case OperatorTypes.Subtract:
						{
							calcresult = (double.Parse(calcresult) - double.Parse(item)).ToString();
						} break;

					case OperatorTypes.Multiply:
						{
							calcresult = (double.Parse(calcresult) * double.Parse(item)).ToString();
						} break;
					case OperatorTypes.Divide:
						{
							calcresult = (double.Parse(calcresult) / double.Parse(item)).ToString();
						} break;
				}
				opindex++;
			}
			Result = Math.Round(decimal.Parse(calcresult), 2).ToString();
			if (CurrentOperator.OperatorType == OperatorTypes.Equals)
			{
				InputHistory.Clear();
				foreach (var c in Result)
				{
					ApplyInput(InputButtons.FirstOrDefault(i => i.Value.ToString() == c.ToString()));
				}

			}
			FireChange("Input");
		}

		private DelegateCommand clearEntryCommand;
		public DelegateCommand ClearEntryCommand
		{
			get
			{
				return (clearEntryCommand ?? (clearEntryCommand = new DelegateCommand((o) =>
				{
					if (InputHistory.Count > 0)
					{
						InputHistory.RemoveAt(InputHistory.Count - 1);
					}
					Result = "0";
					FireChange(null);
				})));
			}
		}

		private DelegateCommand clearCommand;
		public DelegateCommand ClearCommand
		{
			get
			{
				return (clearCommand ?? (clearCommand = new DelegateCommand((o) =>
					{
						InputHistory.Clear();
						Result = "0";
						FireChange(null);
					})));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void FireChange(string propertyname)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
		}
	}
}
