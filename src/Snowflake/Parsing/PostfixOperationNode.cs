using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class PostfixOperationNode : ExpressionNode
	{
		ExpressionNode sourceExpression;

		public PostfixOperationType Type
		{
			get;
			set;
		}

		public ExpressionNode SourceExpression
		{
			get { return this.sourceExpression; }
			set { this.sourceExpression = SetParent(this.sourceExpression, value); }
		}

		public PostfixOperationNode()
			: base()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.SourceExpression != null)
			{
				foreach (T node in this.SourceExpression.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
