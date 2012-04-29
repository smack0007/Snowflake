using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FuncNode
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

		public IList<StatementNode> Statements
		{
			get;
			private set;
		}

		public FuncNode()
			: base()
		{
			this.Args = new List<string>();
			this.Statements = new List<StatementNode>();
		}
	}
}
