using System;

namespace Snowsoft.SnowflakeScript.Executor
{
	public class ScriptFunctionCallException : ScriptException
	{
		public ScriptFunctionCallException(string message)
			: base(message)
		{
		}
	}
}