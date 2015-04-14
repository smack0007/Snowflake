using System;

namespace Snowflake.Parsing
{
	public class IntegerValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(int); }
        }

		public int Value
		{
			get;
			set;
		}

		public IntegerValueNode()
		{
		}

        public override object GetValue()
        {
            return this.Value;
        }
    }
}
