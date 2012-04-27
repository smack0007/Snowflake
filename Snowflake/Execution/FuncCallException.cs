using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class FuncCallException : ScriptException
	{
		public FuncCallException(string message)
			: base(message)
		{
		}
	}
}