using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
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
				// See if it's currently anywhere in the stack
				for (int i = this.stack.Count - 1; i >= 0; i--)
				{
					if (this.stack[i].ContainsKey(name))
						return this.stack[i][name];
				}

				// We didn't find the variable so add it to the top of the stack
				Variable variable = new Variable();
				this.stack[stack.Count - 1].Add(name, variable);
				return variable;
			}
		}

		public VariableStack()
		{
			this.stack = new List<Dictionary<string, Variable>>();
			this.stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pushes a new layer onto the stack.
		/// </summary>
		public void Push()
		{
			this.stack.Add(new Dictionary<string, Variable>());
		}

		/// <summary>
		/// Pops a layer off the stack.
		/// </summary>
		public void Pop()
		{
			if (this.stack.Count == 1)
				throw new ScriptException(ScriptError.StackError, "Cannot pop as the stack size is only 1.");

			this.stack.RemoveAt(this.stack.Count - 1);
		}
	}
}
