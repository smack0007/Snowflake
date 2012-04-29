using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public interface IScriptParser
	{
		ScriptNode Parse(IList<Lexeme> lexemes);
	}
}
