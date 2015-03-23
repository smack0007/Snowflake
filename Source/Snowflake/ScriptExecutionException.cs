using System;
using System.Collections.Generic;

namespace Snowflake
{
	public class ScriptExecutionException : ScriptException
	{
        public ScriptStackFrame[] Stack
		{
			get;
			private set;
		}

        public ScriptExecutionException(string message)
            : base(message)
        {
            this.Stack = null;
        }

        public ScriptExecutionException(string message, ScriptStackFrame[] stack)
            : base(message)
        {
            this.Stack = stack;
        }

        public ScriptExecutionException(string message, ScriptStackFrame[] stack, Exception innerException)
			: base(message, innerException)
		{
            this.Stack = stack;
		}
	}
}
