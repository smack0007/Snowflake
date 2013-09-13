using Snowsoft.SnowflakeScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptFunction : ScriptObject
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

		public ScriptFunction(StatementBlockNode statementBlock)
		{
			if (statementBlock == null)
				throw new ArgumentNullException("statementBlock");

			this.StatementBlock = statementBlock;
		}
	}
}
