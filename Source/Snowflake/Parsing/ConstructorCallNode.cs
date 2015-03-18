using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
    public class ConstructorCallNode : ExpressionNode
    {
        public string ConstructorName
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

            foreach (T node in this.Args.Find<T>())
            {
                yield return node;
            }
        }
    }
}
