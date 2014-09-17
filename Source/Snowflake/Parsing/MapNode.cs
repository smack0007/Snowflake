using System;
using System.Collections.Generic;

namespace Snowflake.Parsing
{
	public class MapNode : ExpressionNode, IEnumerable<MapPairNode>
	{
		public SyntaxNodeCollection<MapPairNode> Pairs
		{
			get;
			private set;
		}

		public MapNode()
			: base()
		{
			this.Pairs = new SyntaxNodeCollection<MapPairNode>(this);
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

		public IEnumerator<MapPairNode> GetEnumerator()
		{
			return this.Pairs.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Pairs.GetEnumerator();
		}
	}
}
