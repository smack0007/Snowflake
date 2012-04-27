using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public interface IScriptLexer
	{
		IList<ScriptLexeme> Lex(string text);
	}
}
