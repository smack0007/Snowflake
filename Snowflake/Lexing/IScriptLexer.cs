using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public interface IScriptLexer
	{
		IList<Lexeme> Lex(string text);
	}
}
