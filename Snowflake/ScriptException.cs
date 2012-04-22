using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptException : Exception
	{
		public ScriptException(string message)
			: base(message)
		{
		}
	}
}
