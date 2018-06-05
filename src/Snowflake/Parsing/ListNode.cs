using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class ListNode : ExpressionNode, IEnumerable<ExpressionNode>
	{
		public SyntaxNodeCollection<ExpressionNode> ValueExpressions
		{
			get;
			private set;
		}

		public ListNode()
			: base()
		{
			this.ValueExpressions = new SyntaxNodeCollection<ExpressionNode>(this);
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			foreach (var expression in this.ValueExpressions)
			{
				foreach (T node in expression.Find<T>())
				{
					yield return node;
				}
			}
		}

		public IEnumerator<ExpressionNode> GetEnumerator()
		{
			return this.ValueExpressions.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.ValueExpressions.GetEnumerator();
		}
	}
}
