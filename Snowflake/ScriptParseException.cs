using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptParseException : ScriptException
	{
		public ScriptParseException(string message)
			: base(message)
		{
		}
	}
}
