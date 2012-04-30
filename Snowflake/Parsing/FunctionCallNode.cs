using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionCallNode : ExpressionNode
	{
		public string FunctionName
		{
			get;
			set;
		}

		public IList<ExpressionNode> ArgExpressions
		{
			get;
			set;
		}

		public FunctionCallNode()
			: base()
		{
			this.ArgExpressions = new List<ExpressionNode>();
		}
	}
}
