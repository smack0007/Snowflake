using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake
{
    public class ScriptExecutionContext : IScriptExecutionContext
    {           
        ScriptNamespace globals;
        List<ScriptStackFrame> stack;

        Dictionary<string, ScriptTypeSet> types;
        
        public dynamic this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        public ScriptExecutionContext()
        {
            this.globals = new ScriptNamespace("<global>");
            this.stack = new List<ScriptStackFrame>();

            this.types = new Dictionary<string, ScriptTypeSet>()
            {
                { "bool", new ScriptTypeSet(typeof(bool)) },
                { "char", new ScriptTypeSet(typeof(char)) },
                { "float", new ScriptTypeSet(typeof(float)) },
                { "int", new ScriptTypeSet(typeof(int)) },
                { "string", new ScriptTypeSet(typeof(string)) }
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
                if (this.globals.ContainsVariable(name))
                    throw new ScriptExecutionException(string.Format("Variable \"{0}\" declared more than once in the same stack frame.", name), this.stack.ToArray());

                this.globals.SetVariable(name, variable);
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

            if (this.globals.TryGetVariable(name, out variable))
                return variable.Value;

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
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

            ScriptVariable variable;
            if (this.globals.TryGetVariable(name, out variable))
            {
                variable.Value = value;
                return value;
            }

            throw new ScriptExecutionException(string.Format("Variable \"{0}\" is not defined.", name), this.stack.ToArray());
        }
  
        public dynamic GetGlobalVariable(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ScriptVariable variable;

            if (name.IndexOf('.') > 0)
            {
                string[] nameParts = name.Split('.');

                ScriptNamespace pointer = this.globals;

                for (int i = 0; i < nameParts.Length - 1; i++)
                {
                    if (pointer.TryGetVariable(nameParts[i], out variable))
                    {
                        if (variable.Value is ScriptNamespace)
                        {
                            pointer = variable.Value;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (pointer.TryGetVariable(nameParts[nameParts.Length - 1], out variable))
                    return variable.Value;
            }
            else
            {
                if (this.globals.TryGetVariable(name, out variable))
                    return variable.Value;
            }

            throw new ScriptExecutionException("Global variable \"{0}\" is not defined.");
        }

        public void SetGlobalVariable(string name, dynamic value, bool isConst = false)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name.IndexOf('.') > 0)
            {
                string[] nameParts = name.Split('.');

                ScriptNamespace pointer = this.globals;
                                
                for (int i = 0; i < nameParts.Length - 1; i++)
                {
                    ScriptVariable variable;
                    if (pointer.TryGetVariable(nameParts[i], out variable))
                    {
                        if (variable.Value is ScriptNamespace)
                        {
                            pointer = variable.Value;
                        }
                        else
                        {
                            throw new ScriptExecutionException(string.Format("Unable to set global variable \"{0}\". \"{1}\" is already a variable.", name, string.Join(".", nameParts.Take(i + 1))));
                        }
                    }
                    else
                    {
                        pointer = pointer.DeclareSubNamespace(nameParts[i]);
                    }
                }

                pointer.SetVariable(nameParts[nameParts.Length - 1], new ScriptVariable(value, isConst));
            }
            else
            {
                this.globals.SetVariable(name, new ScriptVariable(value, isConst));
            }
        }

        public void RegisterType(string name, Type type)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (type == null)
                throw new ArgumentNullException("type");

            ScriptTypeSet typeSet = null;

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
                this.types[name] = new ScriptTypeSet(type);
            }
        }

        public Type GetType(string name, int genericArgCount)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (genericArgCount < 0)
                throw new ArgumentOutOfRangeException("genericArgCount", "genericArgCount must be >= 0.");

            ScriptTypeSet typeSet;

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
