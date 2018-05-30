using System;
using System.Collections.Generic;

namespace Snowflake.Parsing
{
	public class AssignmentOpeartionNode : ExpressionNode
	{
		ExpressionNode targetExpression;
		
		ExpressionNode valueExpression;

		public ExpressionNode TargetExpression
		{
			get { return this.targetExpression; }
			set { this.targetExpression = SetParent(this.targetExpression, value); }
		}

		public AssignmentOperationType Type
		{
			get;
			set;
		}

		public ExpressionNode ValueExpression
		{
			get { return this.valueExpression; }
			set { this.valueExpression = SetParent(this.valueExpression, value); }
		}

		public AssignmentOpeartionNode()
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
