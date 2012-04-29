using System;

namespace Snowsoft.SnowflakeScript.Parsing
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
