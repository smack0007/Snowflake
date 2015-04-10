using System;
using System.Collections.Generic;

namespace Snowflake
{
    public class ScriptExecutionContext : IScriptExecutionContext
    {   
        class TypeSet
        {
            Dictionary<int, Type> types;

            public Type this[int index]
            {
                get { return this.types[index]; }
                set { this.types[index] = value; }
            }

            public TypeSet()
            {
                this.types = new Dictionary<int, Type>();
            }

            public TypeSet(Type type)
                : this()
            {
                if (type.IsGenericType)
                {
                    Type[] genericArgs = type.GetGenericArguments();
                    this.types[genericArgs.Length] = type;
                }
                else
                {
                    this.types[0] = type;
                }
            }

            public bool TryGetValue(int count, out Type type)
            {
                return this.types.TryGetValue(count, out type);
            }
        }

        Dictionary<string, dynamic> globals;
        List<ScriptStackFrame> stack;

        Dictionary<string, TypeSet> types;
        
        public dynamic this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, dynamic>();
            this.stack = new List<ScriptStackFrame>();

            this.types = new Dictionary<string, TypeSet>()
            {
                { "bool", new TypeSet(typeof(Boolean)) },
                { "char", new TypeSet(typeof(Char)) },
                { "float", new TypeSet(typeof(Single)) },
                { "int", new TypeSet(typeof(Int32)) },
                { "string", new TypeSet(typeof(String)) }
            };
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
            TypeSet typeSet = null;

            if (this.types.TryGetValue(name, out typeSet))
            {
                if (type.IsGenericType)
                {
                    Type[] genericArgs = type.GetGenericArguments();
                    Type existingType = null;

                    if (!typeSet.TryGetValue(genericArgs.Length, out existingType))
                    {
                        typeSet[genericArgs.Length] = type;
                    }
                    else
                    {
                        throw new ScriptExecutionException(string.Format("Type \"{0}\" is already registered with the given generic argument variation.", name));
                    }
                }
                else
                {
                    throw new ScriptExecutionException(string.Format("Type \"{0}\" is already registered and is not generic argument variation on the existing type.", name));
                }
            }
            else
            {
                this.types[name] = new TypeSet(type);
            }
        }

        public Type GetType(string name, int genericArgCount)
        {
            TypeSet result;

            if (this.types.TryGetValue(name, out result))
                return result[genericArgCount];

            throw new ScriptExecutionException(string.Format("Type \"{0}\" is not registered.", name), this.stack.ToArray());
        }
    }
}
