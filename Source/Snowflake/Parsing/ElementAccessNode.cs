using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
	public class ElementAccessNode : ExpressionNode
	{
		ExpressionNode sourceExpression;
		ExpressionNode elementExpression;

		public ExpressionNode SourceExpression
		{
			get { return this.sourceExpression; }
			set { this.sourceExpression = SetParent(this.sourceExpression, value); }
		}

		public ExpressionNode ElementExpression
		{
			get { return this.elementExpression; }
			set { this.elementExpression = SetParent(this.elementExpression, value); }
		}

		public ElementAccessNode()
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

			if (this.ElementExpression != null)
			{
				foreach (T node in this.ElementExpression.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
