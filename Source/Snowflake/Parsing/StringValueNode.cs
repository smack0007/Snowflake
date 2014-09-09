using System;

namespace Snowflake.Parsing
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
