using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class VariableNode : ExpressionNode
	{
		public string VariableName
		{
			get;
			set;
		}

		public VariableNode()
			: base()
		{
		}
	}
}
