using System;

namespace Snowflake.Parsing
{
	public class BooleanValueNode : ExpressionNode
	{
		public bool Value
		{
			get;
			set;
		}

		public BooleanValueNode()
		{
		}
	}
}
