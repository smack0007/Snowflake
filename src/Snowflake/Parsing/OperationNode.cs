using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Parsing
{
	public class OperationNode : ExpressionNode
	{
		ExpressionNode leftHand;
		ExpressionNode rightHand;
		
		public OperationType Type
		{
			get;
			set;
		}

		public ExpressionNode LeftHand
		{
			get { return this.leftHand; }
			set { this.leftHand = SetParent(this.leftHand, value); }
		}

		public ExpressionNode RightHand
		{
			get { return this.rightHand; }
			set { this.rightHand = SetParent(this.rightHand, value); }
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

			if (this.LeftHand != null)
			{
				foreach (T node in this.LeftHand.Find<T>())
				{
					yield return node;
				}
			}

			if (this.RightHand != null)
			{
				foreach (T node in this.RightHand.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
