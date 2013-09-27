using Snowsoft.SnowflakeScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptFunction : ScriptObject
	{
		public override string TypeName
		{
			get { return "func"; }
		}

		public StatementBlockNode StatementBlock
		{
			get;
			private set;
		}

		public string[] Args
		{
			get;
			private set;
		}

		public Dictionary<string, ScriptVariableReference> VariableReferences
		{
			get;
			private set;
		}

		public ScriptFunction(StatementBlockNode statementBlock, string[] args, Dictionary<string, ScriptVariableReference> variableReferences)
		{
			if (statementBlock == null)
				throw new ArgumentNullException("statementBlock");

			if (args == null)
				throw new ArgumentNullException("args");

			if (variableReferences == null)
				throw new ArgumentNullException("variableReferences");

			this.StatementBlock = statementBlock;
			this.Args = args;
			this.VariableReferences = variableReferences;
		}
	}
}
