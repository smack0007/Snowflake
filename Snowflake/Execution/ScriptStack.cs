using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	/// <summary>
	/// Represents a stack.
	/// </summary>
	public sealed class ScriptStack
	{		
		List<Dictionary<string, ScriptVariableReference>> stack;

		public IDictionary<string, ScriptVariableReference> Globals
		{
			get { return this.stack[0]; }
		}

		public ScriptVariableReference this[string name]
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
			this.stack = new List<Dictionary<string, ScriptVariableReference>>();
			this.stack.Add(new Dictionary<string, ScriptVariableReference>());
		}

		/// <summary>
		/// Returns true if a variable is already declared in the current frame of the stack.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool IsDeclaredInFrame(string name)
		{
			return this.stack[this.stack.Count - 1].ContainsKey(name);
		}

		/// <summary>
		/// Pushes a new function onto the stack.
		/// </summary>
		public void Push()
		{
			this.stack.Add(new Dictionary<string, ScriptVariableReference>());
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
