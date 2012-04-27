using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexer;

namespace Snowsoft.SnowflakeScript.Parser
{
	public interface IScriptParser
	{
		ScriptSyntaxTreeNode Parse(IList<ScriptLexeme> lexemes);
	}
}
