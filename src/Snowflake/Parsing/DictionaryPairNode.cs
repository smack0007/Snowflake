using System;
using System.Collections.Generic;

namespace Snowflake.Parsing
{
	public class DictionaryPairNode : SyntaxNode
	{
		ExpressionNode keyExpression;
		ExpressionNode valueExpression;

		public ExpressionNode KeyExpression
		{
			get { return this.keyExpression; }
			set { this.keyExpression = SetParent(this.keyExpression, value); }
		}

		public ExpressionNode ValueExpression
		{
			get { return this.valueExpression; }
			set { this.valueExpression = SetParent(this.valueExpression, value); }
		}

		public DictionaryPairNode()
			: base()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.KeyExpression != null)
			{
				foreach (T node in this.KeyExpression.Find<T>())
				{
					yield return node;
				}
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
