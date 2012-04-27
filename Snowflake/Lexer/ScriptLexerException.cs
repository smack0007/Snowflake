using System;

namespace Snowsoft.SnowflakeScript.Lexer
{
	public class ScriptLexerException : ScriptException
	{
		public ScriptLexerException(string message)
			: base(message)
		{
		}
	}
}
