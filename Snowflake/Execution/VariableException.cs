using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class VariableException : ScriptException
	{
		public VariableException(string message)
			: base(message)
		{
		}
	}
}