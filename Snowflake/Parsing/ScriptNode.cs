using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptNode : SyntaxNode
	{
        public string ID
        {
            get;
            private set;
        }

		public SyntaxNodeCollection<StatementNode> Statements
		{
			get;
			private set;
		}

		public ScriptNode(string id)
		{
            if (id == null)
                throw new ArgumentNullException("id");

            this.ID = id;
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
