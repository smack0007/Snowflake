using System;

namespace Snowflake.Parsing
{
	public class StringValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(string); }
        }

		public string Value
		{
			get;
			set;
		}

		public StringValueNode()
			: base()
		{
		}

        public override object GetValue()
        {
            return this.Value;
        }
    }
}
