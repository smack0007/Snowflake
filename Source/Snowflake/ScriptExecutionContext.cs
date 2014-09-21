using System;
using System.Collections.Generic;

namespace Snowflake
{
    public class ScriptExecutionContext : MarshalByRefObject, IScriptExecutionContext
    {
        Dictionary<string, dynamic> globals;
        Stack<ScriptStackFrame> stack;

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, dynamic>();
            this.stack = new Stack<ScriptStackFrame>();
        }
                
        public void PushStackFrame(string function)
        {
            this.stack.Push(new ScriptStackFrame(function));
        }

        public void PopStackFrame()
        {
            this.stack.Pop();
        }

        public ScriptStackFrame[] GetStackFrames()
        {
            return this.stack.ToArray();
        }

        public dynamic GetGlobalVariable(string name)
        {
            if (this.globals.ContainsKey(name))
                return this.globals[name];

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
        }

        public void SetGlobalVariable(string name, dynamic value)
        {
            this.globals[name] = value;
        }
    }
}
