using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
    public class YieldNode : ExpressionNode
    {
        ExpressionNode expression;

        public ExpressionNode ValueExpression
        {
            get { return this.expression; }
            set { this.expression = SetParent(this.expression, value); }
        }

        public YieldNode()
            : base()
        {
        }

        public override IEnumerable<T> Find<T>()
        {
            foreach (T node in base.Find<T>())
            {
                yield return node;
            }

            if (this.ValueExpression != null)
            {
                foreach (T node in this.ValueExpression.Find<T>())
                {
                    yield return node;
                }
            }
        }
    }
}
