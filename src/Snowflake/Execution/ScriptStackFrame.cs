using System;
using System.Collections.Generic;

namespace Snowflake.Execution
{
    [Serializable]
    public class ScriptStackFrame
	{
		public string Name { get; }

        public Dictionary<string, ScriptVariable> CapturedVariables { get; }

        public Dictionary<string, ScriptVariable> Variables { get; } = new Dictionary<string, ScriptVariable>();

        public List<ScriptNamespace> UsingNamespaces { get; } = new List<ScriptNamespace>();

        internal ScriptStackFrame(string name, Dictionary<string, ScriptVariable> capturedVariables)
		{
			this.Name = name;
            this.CapturedVariables = capturedVariables;
		}
	}
}
