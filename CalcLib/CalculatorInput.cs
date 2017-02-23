using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace CalcLib
{
	public abstract class CalculatorInput : ICalculatorInput
	{
		protected CalculatorInput(Calculator calculator, string value, System.Windows.Input.Key key)
		{
			Calculator = calculator;
			Value = value;
			Key = key;
		}

		public System.Windows.Input.Key Key { get; set; }

		private Calculator Calculator { get; set; }

		public string Value
		{
			get;
			private set;
		}

		private DelegateCommand<CalculatorInput> inputExecutedCommand;
		public DelegateCommand<CalculatorInput> InputExecutedCommand
		{
			get
			{
				return (inputExecutedCommand ?? (inputExecutedCommand = new DelegateCommand<CalculatorInput>((o) =>
				{
					Calculator.ApplyInput(this);
				}, (o) =>
			{
				return true;
			})));
			}
		}
	}
}
