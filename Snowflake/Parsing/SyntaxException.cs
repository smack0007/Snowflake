using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class SyntaxException : ScriptException
	{
		public SyntaxException(string message)
			: base(message)
		{
		}
	}
}