using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptFunctionCallException : ScriptException
	{
		public ScriptFunctionCallException(string message)
			: base(message)
		{
		}
	}
}