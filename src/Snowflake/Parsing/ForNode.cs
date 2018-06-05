using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class ForNode : StatementNode
	{
		SyntaxNode initializeSyntax;
		ExpressionNode evaluateExpression;
		ExpressionNode incrementExpression;
		StatementBlockNode bodyStatementBlock;

		public SyntaxNode InitializeSyntax
		{
			get { return this.initializeSyntax; }
			set { this.initializeSyntax = SetParent(this.initializeSyntax, value); }
		}

		public ExpressionNode EvaluateExpression
		{
			get { return this.evaluateExpression; }
			set { this.evaluateExpression = SetParent(this.evaluateExpression, value); }
		}

		public ExpressionNode IncrementExpression
		{
			get { return this.incrementExpression; }
			set { this.incrementExpression = SetParent(this.incrementExpression, value); }
		}

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}

		public ForNode()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.InitializeSyntax != null)
			{
				foreach (T node in this.InitializeSyntax.Find<T>())
				{
					yield return node;
				}
			}

			if (this.EvaluateExpression != null)
			{
				foreach (T node in this.EvaluateExpression.Find<T>())
				{
					yield return node;
				}
			}

			if (this.IncrementExpression != null)
			{
				foreach (T node in this.IncrementExpression.Find<T>())
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
		}
	}
}
