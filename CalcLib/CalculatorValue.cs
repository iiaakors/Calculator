using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLib
{
	public class CalculatorValue : CalculatorInput
	{
		internal CalculatorValue(Calculator calculator, string value, System.Windows.Input.Key key)
			: base(calculator, value, key)
		{
		}

	}
}
