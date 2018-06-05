using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class ConstructorCallNode : ExpressionNode
    {
        public TypeNameNode TypeName
        {
            get;
            set;
        }

        public SyntaxNodeCollection<ExpressionNode> Args
        {
            get;
            private set;
        }
                
        public ConstructorCallNode()
            : base()
        {
            this.Args = new SyntaxNodeCollection<ExpressionNode>(this);
        }

        public override IEnumerable<T> Find<T>()
        {
            foreach (T node in base.Find<T>())
            {
                yield return node;
            }

            if (this.TypeName != null)
            {
                foreach (T node in this.TypeName.Find<T>())
                {
                    yield return node;
                }
            }

            foreach (T node in this.Args.Find<T>())
            {
                yield return node;
            }
        }
    }
}
