using System;

namespace Snowsoft.SnowflakeScript.Executor
{
	public class ScriptVariableException : ScriptException
	{
		public ScriptVariableException(string message)
			: base(message)
		{
		}
	}
}