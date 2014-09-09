using System;

namespace Snowflake
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
