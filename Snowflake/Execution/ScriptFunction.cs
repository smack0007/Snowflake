using Snowsoft.SnowflakeScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptFunction : ScriptObject
	{
		public sealed class Argument
		{
			public string Name { get; set; }
			public ExpressionNode DefaultValueExpression { get; set; }
		}

		public override string TypeName
		{
			get { return "func"; }
		}

		public StatementBlockNode StatementBlock
		{
			get;
			private set;
		}

		public Argument[] Args
		{
			get;
			private set;
		}

		public Dictionary<string, ScriptVariableReference> VariableReferences
		{
			get;
			private set;
		}

		public ScriptFunction(StatementBlockNode statementBlock, Argument[] args, Dictionary<string, ScriptVariableReference> variableReferences)
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
