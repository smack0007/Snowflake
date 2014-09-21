using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake
{
	public class ScriptExecutionException : ScriptException
	{
        public string ScriptId
        {
            get;
            private set;
        }
                
		public IList<ScriptStackFrame> Stack
		{
			get;
			private set;
		}
                
		public ScriptExecutionException(string message, Exception innerException, string scriptId, Stack<ScriptStackFrame> stack)
			: base(message, innerException)
		{
			this.ScriptId = scriptId;
			this.Stack = stack.ToList();
		}
	}
}
