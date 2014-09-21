using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake
{
	public class ScriptExecutionException : ScriptException
	{
		public IList<ScriptStackFrame> Stack
		{
			get;
			private set;
		}
                
		public ScriptExecutionException(string message, Exception innerException, Stack<ScriptStackFrame> stack)
			: base(message, innerException)
		{
			this.Stack = stack.ToList();
		}
	}
}
