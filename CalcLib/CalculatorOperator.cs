using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace CalcLib
{
	public class CalculatorOperator : CalculatorInput
	{
		internal CalculatorOperator(Calculator calculator, string value, System.Windows.Input.Key key, OperatorTypes operatortype)
			: base(calculator, value, key)
		{
			OperatorType = operatortype;
		}

		public OperatorTypes OperatorType { get; private set; }

	}
}
