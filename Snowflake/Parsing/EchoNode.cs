using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class EchoNode : StatementNode
	{
		public ExpressionNode Expression
		{
			get;
			set;
		}

		public EchoNode()
			: base()
		{
		}
	}
}
