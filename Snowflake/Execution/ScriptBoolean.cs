using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptBoolean : ScriptObject<bool>
	{
		public static readonly ScriptBoolean True = new ScriptBoolean(true);

		public static readonly ScriptBoolean False = new ScriptBoolean(false);

		public override string TypeName
		{
			get { return "bool"; }
		}

		public ScriptBoolean(bool value)
			: base(value)
		{
		}

		public override string ToString()
		{
			return this.Value ? "true" : "false";
		}

		public override ScriptObject LogicalAnd(ScriptObject other)
		{
			if (!(other is ScriptBoolean))
			{
				this.ThrowOperationNotSupportedBetweenTypesException("LogicalAnd", other);
			}

			if (this.Value && ((ScriptBoolean)other).Value)
			{
				return ScriptBoolean.True;
			}
			else
			{
				return ScriptBoolean.False;
			}
		}

		public override ScriptObject LogicalOr(ScriptObject other)
		{
			if (!(other is ScriptBoolean))
			{
				this.ThrowOperationNotSupportedBetweenTypesException("LogicalOr", other);
			}

			if (this.Value || ((ScriptBoolean)other).Value)
			{
				return ScriptBoolean.True;
			}
			else
			{
				return ScriptBoolean.False;
			}
		}
	}
}
