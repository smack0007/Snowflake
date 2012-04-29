using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class StringValueNode : ExpressionNode
	{
		public string Value
		{
			get;
			set;
		}

		public StringValueNode()
			: base()
		{
		}
	}
}
