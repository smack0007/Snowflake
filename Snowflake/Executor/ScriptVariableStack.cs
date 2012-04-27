using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Executor
{
	/// <summary>
	/// Represents a stack.
	/// </summary>
	public class ScriptVariableStack
	{
		Dictionary<string, ScriptVariable> globals;
		List<Dictionary<string, ScriptVariable>> stack;

		public ScriptVariable this[string name]
		{
			get
			{
				// If we current have a function stack check if it's in that.
				if (this.stack.Count > 0 && this.stack[this.stack.Count - 1].ContainsKey(name))
					return this.stack[this.stack.Count - 1][name];

				// See if it's in the globals.
				if (this.globals.ContainsKey(name))
					return this.globals[name];

				// We didn't find the variable so create it.
				ScriptVariable variable = new ScriptVariable();

				// If we have a function stack, add it to that.
				if (this.stack.Count > 0)
				{
					this.stack[this.stack.Count - 1].Add(name, variable);
				}
				else // Otherwise add it to the globals.
				{
					this.globals.Add(name, variable);
				}

				return variable;
			}
		}

		public ScriptVariableStack()
		{
			this.globals = new Dictionary<string, ScriptVariable>();
			this.stack = new List<Dictionary<string, ScriptVariable>>();
		}

		/// <summary>
		/// Pushes a new function onto the stack.
		/// </summary>
		public void Push()
		{
			this.stack.Add(new Dictionary<string, ScriptVariable>());
		}

		/// <summary>
		/// Pops a function off the stack.
		/// </summary>
		public void Pop()
		{
			if (this.stack.Count == 0)
				throw new InvalidOperationException("Cannot pop the stack.");

			this.stack.RemoveAt(this.stack.Count - 1);
		}
	}
}
