using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class IfNode : StatementNode
	{
		public ExpressionNode EvaluateExpression
		{
			get;
			set;
		}

		public StatementBlockNode BodyStatementBlock
		{
			get;
			set;
		}

		public StatementBlockNode ElseStatementBlock
		{
			get;
			set;
		}

		public IfNode()
		{
		}
	}
}
