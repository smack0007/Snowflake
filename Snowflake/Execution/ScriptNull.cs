using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptNull : ScriptObject
	{
		public static readonly ScriptNull Value = new ScriptNull();

		public override string TypeName
		{
			get { return "null"; }
		}

		private ScriptNull()
		{
		}

		public override object GetValue()
		{
			return null;
		}
	}
}
