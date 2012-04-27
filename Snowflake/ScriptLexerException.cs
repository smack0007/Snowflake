using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptLexerException : ScriptException
	{
		public ScriptLexerException(string message)
			: base(message)
		{
		}
	}
}
