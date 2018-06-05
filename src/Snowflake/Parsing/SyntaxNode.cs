using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Parsing
{
    public abstract class SyntaxNode
	{
        public ScriptNode Script
        {
            get
            {
                if (this is ScriptNode)
                    return (ScriptNode)this;

                return this.FindParent<ScriptNode>();
            }
        }

		public SyntaxNode Parent
		{
			get;
			set;
		}

        public int Line
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }

		protected T SetParent<T>(T oldValue, T newValue)
			where T : SyntaxNode
		{
			if (oldValue != null)
				oldValue.Parent = null;

			if (newValue != null)
				newValue.Parent = this;

			return newValue;
		}

		public T FindParent<T>()
			where T : SyntaxNode
		{
			var parent = this.Parent;

			while (parent != null && !(parent is T))
				parent = parent.Parent;

			return parent as T;
		}

		public IEnumerable<T> FindChildren<T>()
			where T : SyntaxNode
		{
			return this.Find<T>().Where(x => x != this);
		}

		public virtual IEnumerable<T> Find<T>()
			where T : SyntaxNode
		{
			if (this is T)
			{
				return Enumerable.Repeat<T>((T)this, 1);
			}
			else
			{
				return Enumerable.Empty<T>();
			}
		}
	}
}
