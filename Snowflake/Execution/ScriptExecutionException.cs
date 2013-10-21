using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutionException : ScriptException
	{
        public string ScriptID
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

        public ScriptExecutionException(string message)
            : base(message)
        {
        }

		public ScriptExecutionException(string message, string scriptId, int line, int column)
			: base(message)
		{
            this.ScriptID = scriptId;
            this.Line = line;
            this.Column = column;
		}

        public ScriptExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
