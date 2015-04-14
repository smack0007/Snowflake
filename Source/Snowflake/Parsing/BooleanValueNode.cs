using System;

namespace Snowflake.Parsing
{
	public class BooleanValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(bool); }
        }

		public bool Value
		{
			get;
			set;
		}

		public BooleanValueNode()
		{
		}

        public override object GetValue()
        {
            return this.Value;
        }
    }
}
