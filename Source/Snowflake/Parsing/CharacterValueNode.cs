using System;

namespace Snowflake.Parsing
{
	public class CharacterValueNode : ExpressionNode
	{
		public char Value
		{
			get;
			set;
		}

		public CharacterValueNode()
		{
		}
	}
}
