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

		public StatementNode ElseStatement
		{
			get;
			set;
		}

		public IfNode()
		{
		}
	}
}
