using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class OperationNode : ExpressionNode
	{
		public OperationType Type
		{
			get;
			set;
		}

		public ExpressionNode LHS
		{
			get;
			set;
		}

		public ExpressionNode RHS
		{
			get;
			set;
		}

		public OperationNode()
			: base()
		{
		}
	}
}
