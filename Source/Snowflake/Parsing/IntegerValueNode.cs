using System;

namespace Snowflake.Parsing
{
	public class IntegerValueNode : ExpressionNode
	{
		public int Value
		{
			get;
			set;
		}

		public IntegerValueNode()
		{
		}
	}
}
