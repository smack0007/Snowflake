using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class FunctionCallNode : ExpressionNode
	{
        ExpressionNode functionExpression;

        public ExpressionNode FunctionExpression
        {
            get { return this.functionExpression; }
            set { this.functionExpression = SetParent(this.functionExpression, value); }
        }

		public SyntaxNodeCollection<ExpressionNode> Args
		{
			get;
			private set;
		}

		public FunctionCallNode()
			: base()
		{
			this.Args = new SyntaxNodeCollection<ExpressionNode>(this);
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			foreach (T node in this.Args.Find<T>())
			{
				yield return node;
			}
		}
	}
}
