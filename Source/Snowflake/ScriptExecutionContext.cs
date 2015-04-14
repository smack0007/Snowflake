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

            public bool ContainsKey(int key)
            {
                return this.types.ContainsKey(key);
            }

            public bool TryGetValue(int key, out Type value)
            {
                return this.types.TryGetValue(key, out value);
            }
        }

        Dictionary<string, ScriptVariable> globals;
        List<ScriptStackFrame> stack;

        Dictionary<string, TypeSet> types;
        
        public dynamic this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        public ScriptExecutionContext()
        {
            this.globals = new Dictionary<string, ScriptVariable>();
            this.stack = new List<ScriptStackFrame>();

            this.types = new Dictionary<string, TypeSet>()
            {
                { "bool", new TypeSet(typeof(bool)) },
                { "char", new TypeSet(typeof(char)) },
                { "float", new TypeSet(typeof(float)) },
                { "int", new TypeSet(typeof(int)) },
                { "string", new TypeSet(typeof(string)) }
            };
        }
                
        public void PushStackFrame(string function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

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

        public void DeclareVariable(string name, dynamic value = null, bool isConst = false)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ScriptVariable variable = new ScriptVariable(value, isConst);
            
            if (this.stack.Count > 0)
            {
                if (this.stack[this.stack.Count - 1].Variables.ContainsKey(name))
                    throw new ScriptExecutionException(string.Format("Variable \"{0}\" declared more than once in the same stack frame.", name), this.stack.ToArray());

                this.stack[this.stack.Count - 1].Variables[name] = variable;
            }
            else
            {
                if (this.globals.ContainsKey(name))
                    throw new ScriptExecutionException(string.Format("Variable \"{0}\" declared more than once in the same stack frame.", name), this.stack.ToArray());

                this.globals[name] = variable;
            }
        }

        public dynamic GetVariable(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ScriptVariable variable = null;

            for (int i = this.stack.Count - 1; i >= 0; i--)
            {
                if (this.stack[i].Variables.TryGetValue(name, out variable))
                    return variable.Value;
            }

            return this.GetGlobalVariable(name);
        }

        public dynamic SetVariable(string name, dynamic value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            for (int i = this.stack.Count - 1; i >= 0; i--)
            {
                if (this.stack[i].Variables.ContainsKey(name))
                {
                    this.stack[i].Variables[name].Value = value;
                    return value;
                }
            }

            if (this.globals.ContainsKey(name))
            {
                this.globals[name].Value = value;
                return value;
            }

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
        }
                
        public dynamic GetGlobalVariable(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ScriptVariable variable;

            if (this.globals.TryGetValue(name, out variable))
                return variable.Value;

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
        }

        public void SetGlobalVariable(string name, dynamic value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            this.globals[name] = new ScriptVariable(value, false);
        }

        public void RegisterType(string name, Type type)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            TypeSet typeSet = null;

            if (this.types.TryGetValue(name, out typeSet))
            {
                if (type.IsGenericType)
                {
                    Type[] genericArgs = type.GetGenericArguments();

                    if (!typeSet.ContainsKey(genericArgs.Length))
                    {
                        typeSet[genericArgs.Length] = type;
                    }
                    else
                    {
                        throw new ScriptExecutionException(string.Format("Type \"{0}\" is already registered with the given generic argument variation.", name));
                    }
                }
                else if (!typeSet.ContainsKey(0))
                {
                    typeSet[0] = type;
                }
                else
                {
                    throw new ScriptExecutionException(string.Format("Type \"{0}\" is already registered and is not a generic argument variation on the existing type.", name));
                }
            }
            else
            {
                this.types[name] = new TypeSet(type);
            }
        }

        public Type GetType(string name, int genericArgCount)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (genericArgCount < 0)
                throw new ArgumentOutOfRangeException("genericArgCount", "genericArgCount must be >= 0.");

            TypeSet typeSet;

            if (this.types.TryGetValue(name, out typeSet))
            {
                Type type;

                if (!typeSet.TryGetValue(genericArgCount, out type))
                {
                    throw new ScriptExecutionException(string.Format("Type \"{0}\" is registered but not with generic argument variation of {1}.", name, genericArgCount), this.stack.ToArray());
                }

                return typeSet[genericArgCount];
            }

            throw new ScriptExecutionException(string.Format("Type \"{0}\" is not registered.", name), this.stack.ToArray());
        }
    }
}
