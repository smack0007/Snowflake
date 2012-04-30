using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionNode : SyntaxTreeNode
	{
		public string Name
		{
			get;
			set;
		}

		public IList<string> Args
		{
			get;
			private set;
		}

		public StatementBlockNode StatementBlock
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
