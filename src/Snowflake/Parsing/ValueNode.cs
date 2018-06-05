using System;

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
