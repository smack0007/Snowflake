using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class DictionaryNode : ExpressionNode, IEnumerable<DictionaryPairNode>
	{
		public SyntaxNodeCollection<DictionaryPairNode> Pairs { get; }

		public DictionaryNode()
			: base()
		{
			this.Pairs = new SyntaxNodeCollection<DictionaryPairNode>(this);
		}

		public override IEnumerable<T> Find<T>()
		{
			foreach (T node in base.Find<T>())
			{
				yield return node;
			}

			foreach (var expression in this.Pairs)
			{
				foreach (T node in expression.Find<T>())
				{
					yield return node;
				}
			}
		}

		public IEnumerator<DictionaryPairNode> GetEnumerator()
		{
			return this.Pairs.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Pairs.GetEnumerator();
		}
	}
}
