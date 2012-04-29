using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class CharValueNode : ExpressionNode
	{
		public char Value
		{
			get;
			set;
		}

		public CharValueNode()
		{
		}
	}
}
