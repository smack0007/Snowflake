using System;

namespace Snowsoft.SnowflakeScript.Parsing
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
