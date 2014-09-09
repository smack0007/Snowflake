using System;

namespace Snowsoft.SnowflakeScript.Parsing
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
