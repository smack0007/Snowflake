using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Parsing
{
	public class AssignmentNode : StatementNode
	{
		ExpressionNode valueExpression;

		public string VariableName
		{
			get;
			set;
		}

		public AssignmentOperation Operation
		{
			get;
			set;
		}

		public ExpressionNode ValueExpression
		{
			get { return this.valueExpression; }
			set { this.valueExpression = SetParent(this.valueExpression, value); }
		}

		public AssignmentNode()
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
