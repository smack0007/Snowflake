using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptNode : SyntaxTreeNode
	{
		public IList<FunctionNode> Functions
		{
			get;
			private set;
		}

		public ScriptNode()
		{
			this.Functions = new List<FunctionNode>();
		}
	}
}
