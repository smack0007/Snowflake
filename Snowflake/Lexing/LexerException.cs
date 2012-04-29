using System;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public class LexerException : ScriptException
	{
		public LexerException(string message)
			: base(message)
		{
		}
	}
}
