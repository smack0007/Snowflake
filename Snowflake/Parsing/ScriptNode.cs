using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptNode
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
