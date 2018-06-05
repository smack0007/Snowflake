using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class MemberAccessNode : ExpressionNode
	{
		ExpressionNode sourceExpression;
		
		public ExpressionNode SourceExpression
		{
			get { return this.sourceExpression; }
			set { this.sourceExpression = SetParent(this.sourceExpression, value); }
		}

		public string MemberName
		{
			get;
			set;
		}

		public MemberAccessNode()
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
