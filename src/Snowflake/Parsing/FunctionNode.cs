﻿using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class FunctionNode : ExpressionNode
	{
		StatementBlockNode bodyStatementBlock;

        public bool IsGenerator { get; set; }

		public SyntaxNodeCollection<VariableDeclarationNode> Args { get; private set; }

		public StatementBlockNode BodyStatementBlock
		{
			get { return this.bodyStatementBlock; }
			set { this.bodyStatementBlock = SetParent(this.bodyStatementBlock, value); }
		}

		public FunctionNode()
			: base()
		{
            this.Args = new SyntaxNodeCollection<VariableDeclarationNode>(this);
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
