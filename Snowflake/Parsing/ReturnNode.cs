using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ReturnNode : ExpressionNode
	{
		public ExpressionNode Expression
		{
			get;
			set;
		}

		public ReturnNode()
			: base()
		{
		}
	}
}
