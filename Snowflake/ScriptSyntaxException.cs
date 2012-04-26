using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptSyntaxException : ScriptException
	{
		public ScriptSyntaxException(string message)
			: base(message)
		{
		}
	}
}