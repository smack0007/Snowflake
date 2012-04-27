using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Lexer
{
	public interface IScriptLexer
	{
		IList<ScriptLexeme> Lex(string text);
	}
}
