using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
    public abstract class ValueNode : ExpressionNode
    {
        public abstract Type ValueType { get; }

        public ValueNode()
            : base()
        {
        }

        public abstract object GetValue();
    }
}
