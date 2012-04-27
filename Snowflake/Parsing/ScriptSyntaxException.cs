using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptSyntaxException : ScriptException
	{
		public ScriptSyntaxException(string message)
			: base(message)
		{
		}
	}
}