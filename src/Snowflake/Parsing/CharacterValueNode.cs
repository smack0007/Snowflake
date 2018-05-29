using System;

namespace Snowflake.Parsing
{
	public class CharacterValueNode : ValueNode
	{
        public override Type ValueType
        {
            get { return typeof(char); }
        }

		public char Value
		{
			get;
			set;
		}

		public CharacterValueNode()
		{
		}
               
        public override object GetValue()
        {
            return this.Value;
        }
    }
}
