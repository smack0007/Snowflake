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

		internal StatementBlockNode StatementBlock
		{
			get;
			set;
		}

		internal string[] Args
		{
			get;
			set;
		}

		public ScriptFunction(StatementBlockNode statementBlock, string[] args)
		{
			if (statementBlock == null)
				throw new ArgumentNullException("statementBlock");

			if (args == null)
				throw new ArgumentNullException("args");

			this.StatementBlock = statementBlock;
			this.Args = args;
		}
	}
}
