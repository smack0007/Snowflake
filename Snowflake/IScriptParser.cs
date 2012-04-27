﻿using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public interface IScriptParser
	{
		ScriptSyntaxTreeNode Parse(IList<ScriptLexeme> lexemes);
	}
}
