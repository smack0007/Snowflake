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

		public IList<ExpressionNode> Arguments
		{
			get;
			set;
		}

		public FunctionCallNode()
			: base()
		{
			this.Arguments = new List<ExpressionNode>();
		}
	}
}
