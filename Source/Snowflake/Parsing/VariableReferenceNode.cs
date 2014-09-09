using System;

namespace Snowflake.Parsing
{
	public class VariableReferenceNode : ExpressionNode
	{
		public string VariableName
		{
			get;
			set;
		}

		public VariableReferenceNode()
			: base()
		{
		}
	}
}
