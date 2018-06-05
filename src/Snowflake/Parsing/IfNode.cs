using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class IfNode : StatementNode
	{
		ExpressionNode evaluateExpression;
		StatementBlockNode bodyStatementBlock;
		StatementBlockNode elseStatementBlock;

		public ExpressionNode EvaluateExpression
		{
			get { return this.evaluateExpression; }
			set { this.evaluateExpression = SetParent(this.evaluateExpression, value); }
		}

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}

		public StatementBlockNode ElseStatementBlock
		{
			get { return this.elseStatementBlock; }
			set { this.elseStatementBlock = SetParent(this.elseStatementBlock, value); }
		}

		public IfNode()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.EvaluateExpression != null)
			{
				foreach (T node in this.EvaluateExpression.Find<T>())
				{
					yield return node;
				}
			}

			if (this.BodyStatementBlock != null)
			{
				foreach (T node in this.BodyStatementBlock.Find<T>())
				{
					yield return node;
				}
			}

			if (this.ElseStatementBlock != null)
			{
				foreach (T node in this.ElseStatementBlock.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
