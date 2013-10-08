using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptNull : ScriptObject
	{
		public static readonly ScriptNull Value = new ScriptNull();

		public override string TypeName
		{
			get { return "null"; }
		}

		private ScriptNull()
		{
		}

		public override object Unbox()
		{
			return null;
		}

        public override ScriptBoolean EqualTo(ScriptObject other)
        {
			return ReferenceEquals(other, this) ? ScriptBoolean.True : ScriptBoolean.False;
        }
	}
}
