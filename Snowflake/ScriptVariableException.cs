using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptVariableException : ScriptException
	{
		public ScriptVariableException(string message)
			: base(message)
		{
		}
	}
}