using System;
using System.Collections.Generic;

namespace Snowflake
{
    public class ScriptExecutionContext : MarshalByRefObject, IScriptExecutionContext
    {
        Dictionary<string, dynamic> globals;

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, dynamic>();
        }

        public dynamic GetGlobalVariable(string name)
        {
            if (this.globals.ContainsKey(name))
                return this.globals[name];

            return ScriptUndefined.Value;
        }

        public void SetGlobalVariable(string name, dynamic value)
        {
            this.globals[name] = value;
        }
    }
}
