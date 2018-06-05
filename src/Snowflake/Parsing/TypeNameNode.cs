using System.Collections.Generic;

namespace Snowflake.Parsing
{
    public class TypeNameNode : SyntaxNode
    {
        ExpressionNode typeExpression;

        public ExpressionNode TypeExpression
        {
            get { return this.typeExpression; }
            set { this.typeExpression = SetParent(this.typeExpression, value); }
        }

        public bool IsGeneric
        {
            get { return this.GenericArgs != null && this.GenericArgs.Count > 0; }
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

            if (this.TypeExpression != null)
            {
                foreach (T node in this.TypeExpression.Find<T>())
                {
                    yield return node;
                }
            }

            foreach (T node in this.GenericArgs.Find<T>())
            {
                yield return node;
            }
        }
    }
}
