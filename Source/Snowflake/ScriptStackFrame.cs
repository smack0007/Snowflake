using System;
using System.Collections.Generic;

namespace Snowflake
{
    [Serializable]
    public class ScriptStackFrame
	{
		public string FunctionName
		{
			get;
			private set;
		}

        public Dictionary<string, dynamic> Variables
        {
            get;
            private set;
        }
                
		public ScriptStackFrame(string function)
		{
			this.FunctionName = function;
            this.Variables = new Dictionary<string, dynamic>();
		}
	}
}
