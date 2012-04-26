using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public interface IScriptParser
	{
		IList<ScriptLexeme> Parse(string text);
	}
}
