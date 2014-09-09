using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class OperationNode : ExpressionNode
	{
		ExpressionNode lhs;
		ExpressionNode rhs;
		
		public OperationType Type
		{
			get;
			set;
		}

		public ExpressionNode LHS
		{
			get { return this.lhs; }
			set { this.lhs = SetParent(this.lhs, value); }
		}

		public ExpressionNode RHS
		{
			get { return this.rhs; }
			set { this.rhs = SetParent(this.rhs, value); }
		}

		public OperationNode()
			: base()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.LHS != null)
			{
				foreach (T node in this.LHS.Find<T>())
				{
					yield return node;
				}
			}

			if (this.RHS != null)
			{
				foreach (T node in this.RHS.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
