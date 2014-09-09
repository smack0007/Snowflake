using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptStackFrame
	{
		public string Function
		{
			get;
			private set;
		}

		public ScriptStackFrame(string function)
		{
			this.Function = function;
		}
	}
}
