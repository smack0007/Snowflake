using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Parsing
{
    public class WhileNode : StatementNode
    {
        ExpressionNode evaluateExpression;
		StatementBlockNode bodyStatementBlock;

		public ExpressionNode EvaluateExpression
		{
			get { return this.evaluateExpression; }
			set { this.evaluateExpression = SetParent(this.evaluateExpression, value); }
		}

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}
        
		public WhileNode()
		{
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			if (this.EvaluateExpression != null)
			{
				foreach (T node in this.EvaluateExpression.Find<T>())
				{
					yield return node;
				}
			}

			if (this.BodyStatementBlock != null)
			{
				foreach (T node in this.BodyStatementBlock.Find<T>())
				{
					yield return node;
				}
			}
		}
    }
}
