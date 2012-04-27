using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public interface IScriptParser
	{
		ScriptSyntaxTreeNode Parse(IList<Lexeme> lexemes);
	}
}
