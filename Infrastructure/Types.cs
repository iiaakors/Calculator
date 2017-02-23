using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
	public enum OperatorTypes
	{
		Add,
		Subtract,
		Divide,
		Multiply,
		Equals
	}

	public interface ICalculatorInput
	{
		string Value { get; }
	}

	public interface IExecuted
	{
		event EventHandler CommandExecuted;
	}
}
