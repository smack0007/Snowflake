using System;
using System.Collections.Generic;

namespace Snowflake.Parsing
{
	public class ReturnNode : ExpressionNode
	{
		ExpressionNode expression;

		public ExpressionNode Expression
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

			if (this.Expression != null)
			{
				foreach (T node in this.Expression.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
