using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class FunctionNode : ExpressionNode
	{
		StatementBlockNode bodyStatementBlock;

		public List<string> Args
		{
			get;
			private set;
		}

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}

		public FunctionNode()
			: base()
		{
			this.Args = new List<string>();
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
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
