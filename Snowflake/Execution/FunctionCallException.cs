using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class FunctionCallException : ScriptException
	{
		public FunctionCallException(string message)
			: base(message)
		{
		}
	}
}