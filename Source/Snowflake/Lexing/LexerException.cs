using System;

namespace Snowflake.Lexing
{
	public class LexerException : ScriptException
	{
		public LexerException(string message)
			: base(message)
		{
		}
	}
}
