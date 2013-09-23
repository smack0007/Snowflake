using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptCharacter : ScriptObject<char>
	{
		public override string TypeName
		{
			get { return "char"; }
		}

		public ScriptCharacter(char value)
			: base(value)
		{
		}
	}
}
