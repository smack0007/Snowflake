using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionCallNode : ExpressionNode
	{
		public string FunctionName
		{
			get;
			set;
		}

		public IList<ExpressionNode> Args
		{
			get;
			set;
		}

		public FunctionCallNode()
			: base()
		{
			this.Args = new List<ExpressionNode>();
		}
	}
}
