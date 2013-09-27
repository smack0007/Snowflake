using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class StatementBlockNode : SyntaxNode
	{
		public SyntaxNodeCollection<StatementNode> Statements
		{
			get;
			private set;
		}

		public StatementBlockNode()
		{
			this.Statements = new SyntaxNodeCollection<StatementNode>(this);
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			foreach (var statement in this.Statements)
			{
				foreach (T node in statement.Find<T>())
				{
					yield return node;
				}
			}
		}
	}
}
