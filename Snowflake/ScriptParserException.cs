using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptParserException : ScriptException
	{
		public ScriptParserException(string message)
			: base(message)
		{
		}
	}
}
