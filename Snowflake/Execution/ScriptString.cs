using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptString : ScriptObject<string>
	{
		public override string TypeName
		{
			get { return "string"; }
		}

		public ScriptString(string value)
			: base(value)
		{
		}

		public override ScriptObject Add(ScriptObject other)
		{
			return new ScriptString(this.Value + other.ToString());
		}
	}
}
