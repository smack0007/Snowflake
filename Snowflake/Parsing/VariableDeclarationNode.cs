using System;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class VariableDeclarationNode : StatementNode
	{
		public string VariableName
		{
			get;
			set;
		}

		public ExpressionNode ValueExpression
		{
			get;
			set;
		}

		public VariableDeclarationNode()
			: base()
		{
		}
	}
}
