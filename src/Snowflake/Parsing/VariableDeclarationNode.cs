using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class VariableDeclarationNode : StatementNode
	{
		ExpressionNode valueExpression;

		public string VariableName
		{
			get;
			set;
		}

		public ExpressionNode ValueExpression
		{
			get { return this.valueExpression; }
			set { this.valueExpression = SetParent(this.valueExpression, value); }
		}

		public VariableDeclarationNode()
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
