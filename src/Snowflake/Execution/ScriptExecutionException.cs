using System;

namespace Snowflake.Execution
{
    public class ScriptExecutionException : ScriptException
	{
        public ScriptStackFrame[] ScriptStack { get; }

        public ScriptExecutionException(string message)
            : base(message)
        {
            this.ScriptStack = null;
        }

        public ScriptExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.ScriptStack = null;
        }

        public ScriptExecutionException(string message, ScriptStackFrame[] stack)
            : base(message)
        {
            this.ScriptStack = stack;
        }

        public ScriptExecutionException(string message, ScriptStackFrame[] stack, Exception innerException)
			: base(message, innerException)
		{
            this.ScriptStack = stack;
		}
	}
}
