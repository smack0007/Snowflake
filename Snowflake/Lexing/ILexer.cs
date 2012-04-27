using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public interface ILexer
	{
		IList<Lexeme> Lex(string text);
	}
}
