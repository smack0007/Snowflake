using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionNode : ExpressionNode
	{		
		public IList<string> Args
		{
			get;
			private set;
		}

		public StatementBlockNode BodyStatementBlock
		{
			get;
			set;
		}

		public FunctionNode()
			: base()
		{
			this.Args = new List<string>();
		}
	}
}
