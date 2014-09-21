using System;

namespace Snowflake
{
    [Serializable]
    public class ScriptStackFrame
	{
		public string Function
		{
			get;
			private set;
		}

        public ScriptStackFrame()
        {
        }

		public ScriptStackFrame(string function)
		{
			this.Function = function;
		}
	}
}
