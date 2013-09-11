using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	/// <summary>
	/// Represents a stack.
	/// </summary>
	public class ScriptStack
	{		
		List<Dictionary<string, ScriptVariable>> stack;

		public ScriptVariable this[string name]
		{
			get
			{
				for (int i = this.stack.Count - 1; i >= 0; i--)
				{
					if (this.stack[i].ContainsKey(name))
						return this.stack[i][name];
				}

				return null;
			}

			set
			{
				this.stack[this.stack.Count - 1][name] = value;
			}
		}

		public ScriptStack()
		{
			this.stack = new List<Dictionary<string, ScriptVariable>>();
			this.stack.Add(new Dictionary<string, ScriptVariable>());
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
			if (this.stack.Count == 1)
				throw new InvalidOperationException("Cannot pop the stack.");

			this.stack.RemoveAt(this.stack.Count - 1);
		}
	}
}
