using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptBoolean : ScriptObject<bool>
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
	}
}
