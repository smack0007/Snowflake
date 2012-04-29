using System;

namespace Snowsoft.SnowflakeScript.Parsing
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
