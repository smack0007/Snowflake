using System;
using System.Collections.Generic;

namespace Snowflake
{
    public class ScriptExecutionContext : MarshalByRefObject, IScriptExecutionContext
    {
        Dictionary<string, dynamic> globals;
        Dictionary<string, Type> types;
        Stack<ScriptStackFrame> stack;

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, dynamic>();
            this.types = new Dictionary<string, Type>();
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

        public void RegisterType(string name, Type type)
        {
            if (this.types.ContainsKey(name))
                throw new InvalidOperationException(string.Format("A type is already registered under the name \"{0}\".", name));

            this.types[name] = type;
        }

        public Type GetType(string name)
        {
            if (this.types.ContainsKey(name))
                return this.types[name];

            throw new ScriptExecutionException(string.Format("Type \"{0}\" is not registered.", name), this.stack.ToArray());
        }
    }
}
