using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptNode : SyntaxTreeNode
	{
		public IList<FuncNode> Funcs
		{
			get;
			private set;
		}

		public ScriptNode()
		{
			this.Funcs = new List<FuncNode>();
		}
	}
}
