using System;

namespace Snowflake.Parsing
{
	public class FloatValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(float); }
        }

		public float Value
		{
			get;
			set;
		}

		public FloatValueNode()
			: base()
		{
		}

        public override object GetValue()
        {
            return this.Value;
        }
    }
}
