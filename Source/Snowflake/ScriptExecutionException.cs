using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptExecutionException : ScriptException
	{
        public string ScriptId
        {
            get;
            private set;
        }

        public int Line
        {
            get;
            private set;
        }

        public int Column
        {
            get;
            private set;
        }

		public IList<ScriptStackFrame> Stack
		{
			get;
			private set;
		}

        public ScriptExecutionException(string message)
            : base(message)
        {
        }

		public ScriptExecutionException(string message, string scriptId, int line, int column)
			: base(message)
		{
            this.ScriptId = scriptId;
            this.Line = line;
            this.Column = column;
		}

        public ScriptExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ScriptExecutionException(string message, Exception innerException, string scriptId, Stack<ScriptStackFrame> stack)
			: base(message, innerException)
		{
			this.ScriptId = scriptId;
			this.Stack = stack.ToList();
		}
	}
}
