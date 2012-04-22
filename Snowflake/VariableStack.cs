using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class VariableStackException : ApplicationException
	{
		public VariableStackException(string message)
			: base(message)
		{
		}
	}

	/// <summary>
	/// Represents a stack.
	/// </summary>
	public class VariableStack
	{
		List<Dictionary<string, Variable>> stack;

		public Variable this[string name]
		{
			get
			{
				// See if it's currently anywhere in the stack
				for (int i = stack.Count - 1; i >= 0; i--)
				{
					if (stack[i].ContainsKey(name))
						return stack[i][name];
				}

				// We didn't find the variable so add it to the top of the stack
				Variable variable = new Variable();
				stack[stack.Count - 1].Add(name, variable);
				return variable;
			}
		}

		public VariableStack()
		{
			stack = new List<Dictionary<string, Variable>>();
			stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pushes a new layer onto the stack.
		/// </summary>
		public void Push()
		{
			stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pops a layer off the stack.
		/// </summary>
		public void Pop()
		{
			if (stack.Count == 1)
				throw new VariableStackException("The stack is not greater than 1");

			stack.RemoveAt(stack.Count - 1);
		}
	}
}
