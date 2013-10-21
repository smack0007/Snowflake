using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutionException : ScriptException
	{
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

        public ScriptExecutionException(string message)
            : base(message)
        {
        }

		public ScriptExecutionException(string message, int line, int column)
			: base(message)
		{
            this.Line = line;
            this.Column = column;
		}

        public ScriptExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
