using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionCallNode : ExpressionNode
	{
		public string FunctionName
		{
			get;
			set;
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
