using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
    public class TypeNameNode : SyntaxNode
    {
        public string Name
        {
            get;
            set;
        }

        public SyntaxNodeCollection<TypeNameNode> GenericArgs
        {
            get;
            private set;
        }

        public TypeNameNode()
        {
            this.GenericArgs = new SyntaxNodeCollection<TypeNameNode>(this);
        }

        public override IEnumerable<T> Find<T>()
        {
            foreach (T node in base.Find<T>())
            {
                yield return node;
            }

            foreach (T node in this.GenericArgs.Find<T>())
            {
                yield return node;
            }
        }
    }
}
