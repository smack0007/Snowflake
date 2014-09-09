using System;

namespace Snowflake.Parsing
{
	public class FloatValueNode : ExpressionNode
	{
		public float Value
		{
			get;
			set;
		}

		public FloatValueNode()
			: base()
		{
		}
	}
}
