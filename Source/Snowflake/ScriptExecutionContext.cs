using System;
using System.Collections.Generic;

namespace Snowflake
{
    public class ScriptExecutionContext : IScriptExecutionContext
    {
        Dictionary<string, dynamic> globals;
        Dictionary<string, Type> types;
        List<ScriptStackFrame> stack;

        public dynamic this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, dynamic>();
            this.types = new Dictionary<string, Type>();
            this.stack = new List<ScriptStackFrame>();
        }
                
        public void PushStackFrame(string function)
        {
            this.stack.Add(new ScriptStackFrame(function));
        }

        public void PopStackFrame()
        {
            this.stack.RemoveAt(this.stack.Count - 1);
        }

        public ScriptStackFrame[] GetStackFrames()
        {
            return this.stack.ToArray();
        }

        public void DeclareVariable(string name, dynamic value = null)
        {
            if (this.stack.Count > 0)
            {
                if (this.stack[this.stack.Count - 1].Variables.ContainsKey(name))
                    throw new ScriptExecutionException(string.Format("Variable \"{0}\" declared more than once in the same stack frame.", name), this.stack.ToArray());

                this.stack[this.stack.Count - 1].Variables[name] = value;
            }
            else
            {
                if (this.globals.ContainsKey(name))
                    throw new ScriptExecutionException(string.Format("Variable \"{0}\" declared more than once in the same stack frame.", name), this.stack.ToArray());

                this.globals[name] = value;
            }
        }

        public dynamic GetVariable(string name)
        {
            dynamic result = null;

            for (int i = this.stack.Count - 1; i >= 0; i--)
            {
                if (this.stack[i].Variables.TryGetValue(name, out result))
                    return result;
            }

            return this.GetGlobalVariable(name);
        }

        public dynamic SetVariable(string name, dynamic value)
        {
            for (int i = this.stack.Count - 1; i >= 0; i--)
            {
                if (this.stack[i].Variables.ContainsKey(name))
                {
                    this.stack[i].Variables[name] = value;
                    return value;
                }
            }

            if (this.globals.ContainsKey(name))
            {
                this.globals[name] = value;
                return value;
            }

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
        }
                
        public dynamic GetGlobalVariable(string name)
        {
            dynamic result;

            if (this.globals.TryGetValue(name, out result))
                return result;

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
            Type result;

            if (this.types.TryGetValue(name, out result))
                return result;

            throw new ScriptExecutionException(string.Format("Type \"{0}\" is not registered.", name), this.stack.ToArray());
        }
    }
}
