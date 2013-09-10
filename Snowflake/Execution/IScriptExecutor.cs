using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Parsing;

namespace Snowsoft.SnowflakeScript.Execution
{
	public interface IScriptExecutor
	{
		VariableStack Stack { get; }

		void SetScript(ScriptNode scriptNode);

		void Run();
	}
}
