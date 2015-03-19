using System;
using System.Collections.Generic;

namespace Snowflake.Parsing
{
	public class ReturnNode : ExpressionNode
	{
		ExpressionNode expression;

		public ExpressionNode ValueExpression
		{
			get { return this.expression; }
			set { this.expression = SetParent(this.expression, value); }
		}

		public ReturnNode()
			: base()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.ValueExpression != null)
			{
				foreach (T node in this.ValueExpression.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
