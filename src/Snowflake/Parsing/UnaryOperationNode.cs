﻿using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class UnaryOperationNode : ExpressionNode
	{
		ExpressionNode valueExpression;

		public UnaryOperationType Type
		{
			get;
			set;
		}

		public ExpressionNode ValueExpression
		{
			get { return this.valueExpression; }
			set { this.valueExpression = SetParent(this.valueExpression, value); }
		}

		public UnaryOperationNode()
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
