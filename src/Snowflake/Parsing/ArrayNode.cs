using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
	public class ArrayNode : ExpressionNode, IEnumerable<ExpressionNode>
	{
		public SyntaxNodeCollection<ExpressionNode> ValueExpressions
		{
			get;
			private set;
		}

		public ArrayNode()
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
