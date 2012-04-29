using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FuncNode : SyntaxTreeNode
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

		public FuncNode()
			: base()
		{
			this.Args = new List<string>();
		}
	}
}
