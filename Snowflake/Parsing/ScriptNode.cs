using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptNode : SyntaxTreeNode
	{
		public IList<StatementNode> Statements
		{
			get;
			private set;
		}

		public ScriptNode()
		{
			this.Statements = new List<StatementNode>();
		}
	}
}
