using System;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public class ScriptLexerException : ScriptException
	{
		public ScriptLexerException(string message)
			: base(message)
		{
		}
	}
}
