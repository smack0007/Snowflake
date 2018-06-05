using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class ForEachNode : StatementNode
	{
		VariableDeclarationNode variableDeclaration;
		ExpressionNode sourceExpression;
		StatementBlockNode bodyStatementBlock;

		public VariableDeclarationNode VariableDeclaration
		{
			get { return this.variableDeclaration; }
			set { this.variableDeclaration = SetParent(this.variableDeclaration, value); }
		}

		public ExpressionNode SourceExpression
		{
			get { return this.sourceExpression; }
			set { this.sourceExpression = SetParent(this.sourceExpression, value); }
		}

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}

		public ForEachNode()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.VariableDeclaration != null)
			{
				foreach (T node in this.VariableDeclaration.Find<T>())
				{
					yield return node;
				}
			}

			if (this.SourceExpression != null)
			{
				foreach (T node in this.SourceExpression.Find<T>())
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
