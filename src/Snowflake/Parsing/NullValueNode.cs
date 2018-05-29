using System;

namespace Snowflake.Parsing
{
	public class NullValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(object); }
        }

		public NullValueNode()
			: base()
		{
		}

        public override object GetValue()
        {
            return null;
        }
    }
}
