using System;

namespace Snowsoft.SnowflakeScript.Parser
{
	public class ScriptSyntaxException : ScriptException
	{
		public ScriptSyntaxException(string message)
			: base(message)
		{
		}
	}
}