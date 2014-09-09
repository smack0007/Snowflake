using System;

namespace Snowsoft.SnowflakeScript.Parsing
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
