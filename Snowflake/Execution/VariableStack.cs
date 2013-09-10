using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
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
				if (this.stack[this.stack.Count - 1].ContainsKey(name))
					return this.stack[this.stack.Count - 1][name];

				return null;
			}

			set
			{
				this.stack[this.stack.Count - 1][name] = value;
			}
		}

		public VariableStack()
		{
			this.stack = new List<Dictionary<string, Variable>>();
			this.stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pushes a new function onto the stack.
		/// </summary>
		public void Push()
		{
			this.stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pops a function off the stack.
		/// </summary>
		public void Pop()
		{
			if (this.stack.Count == 1)
				throw new InvalidOperationException("Cannot pop the stack.");

			this.stack.RemoveAt(this.stack.Count - 1);
		}
	}
}
