using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ForNode : StatementNode
	{
		StatementNode initializeStatement;
		ExpressionNode evaluateExpression;
		AssignmentNode incrementStatement;
		StatementBlockNode bodyStatementBlock;

		public StatementNode InitializeStatement
		{
			get { return this.initializeStatement; }
			set { this.initializeStatement = SetParent(this.initializeStatement, value); }
		}

		public ExpressionNode EvaluateExpression
		{
			get { return this.evaluateExpression; }
			set { this.evaluateExpression = SetParent(this.evaluateExpression, value); }
		}

		public AssignmentNode IncrementStatement
		{
			get { return this.incrementStatement; }
			set { this.incrementStatement = SetParent(this.incrementStatement, value); }
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

			if (this.InitializeStatement != null)
			{
				foreach (T node in this.InitializeStatement.Find<T>())
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

			if (this.IncrementStatement != null)
			{
				foreach (T node in this.IncrementStatement.Find<T>())
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
