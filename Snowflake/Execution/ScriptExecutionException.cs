using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutionException : ScriptException
	{
		public ScriptExecutionException(string message)
			: base(message)
		{
		}

		public ScriptExecutionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
