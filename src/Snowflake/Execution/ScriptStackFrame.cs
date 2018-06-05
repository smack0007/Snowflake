using System;
using System.Collections.Generic;

namespace Snowflake.Execution
{
    [Serializable]
    public class ScriptStackFrame
	{
		public string FunctionName
		{
			get;
			private set;
		}

        public Dictionary<string, ScriptVariable> Variables
        {
            get;
            private set;
        }

        public List<ScriptNamespace> UsingNamespaces
        {
            get;
            private set;
        }
                
		public ScriptStackFrame(string function)
		{
			this.FunctionName = function;
            this.Variables = new Dictionary<string, ScriptVariable>();
            this.UsingNamespaces = new List<ScriptNamespace>();
		}
	}
}
