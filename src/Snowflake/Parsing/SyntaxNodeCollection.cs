using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Parsing
{
	public class SyntaxNodeCollection<T> : IEnumerable<T>
		where T : SyntaxNode
	{
		SyntaxNode parent;
		List<T> items;

		public int Count
		{
			get { return this.items.Count; }
		}

		public T this[int index]
		{
			get { return this.items[index]; }
		}

		public SyntaxNodeCollection(SyntaxNode parent)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			this.parent = parent;
			this.items = new List<T>();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(T item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			item.Parent = this.parent;
			this.items.Add(item);
		}

		public IEnumerable<T2> Find<T2>()
			where T2 : SyntaxNode
		{
			foreach (var item in this.items)
			{
				foreach (T2 node in item.Find<T2>())
				{
					yield return node;
				}
			}
		}
	}
}
