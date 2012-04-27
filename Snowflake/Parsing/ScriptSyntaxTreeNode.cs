using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	/// <summary>
	/// Base class for syntax tree nodes.
	/// </summary>
	public abstract class ScriptSyntaxTreeNode
	{
		protected List<ScriptSyntaxTreeNode> ChildNodes
		{
			get;
			private set;
		}

		public ScriptSyntaxTreeNode()
		{
			this.ChildNodes = new List<ScriptSyntaxTreeNode>();
		}
	}
}
