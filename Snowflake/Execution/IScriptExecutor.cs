﻿using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public interface IScriptExecutor
	{
		Variable CallFunc(ScriptNode script, string funcName, IList<Variable> args, VariableStack stack);
	}
}