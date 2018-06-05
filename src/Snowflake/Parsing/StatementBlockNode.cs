using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class StatementBlockNode : SyntaxNode, IEnumerable<StatementNode>
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

		public IEnumerator<StatementNode> GetEnumerator()
		{
			return this.Statements.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Statements.GetEnumerator();
		}
	}
}
