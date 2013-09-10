using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class StatementBlockNode : SyntaxTreeNode
	{
		public List<StatementNode> Statements
		{
			get;
			private set;
		}

		public StatementBlockNode()
		{
			this.Statements = new List<StatementNode>();
		}
	}
}
