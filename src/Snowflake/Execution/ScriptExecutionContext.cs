using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Execution
{
    public class ScriptExecutionContext : IScriptExecutionContext
    {           
        ScriptNamespace globals;
        List<ScriptStackFrame> stack;
        
        public dynamic this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        private ScriptStackFrame CurrentStackFrame
        {
            get { return this.stack[this.stack.Count - 1]; }
        }

        public ScriptExecutionContext()
        {
            this.globals = new ScriptNamespace("<global>");
            this.stack = new List<ScriptStackFrame>();

            this.SetGlobalVariable("bool", new ScriptType(typeof(bool)));
            this.SetGlobalVariable("char", new ScriptType(typeof(char)));
            this.SetGlobalVariable("float", new ScriptType(typeof(float)));
            this.SetGlobalVariable("int", new ScriptType(typeof(int)));
            this.SetGlobalVariable("string", new ScriptType(typeof(string)));
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

        public void UsingNamespace(string name)
        {
            ScriptNamespace pointer = this.GetNamespace(name);

            if (pointer == null)
                throw new ScriptExecutionException(string.Format("Namespace \"{0}\" is not defined.", name));

            this.CurrentStackFrame.UsingNamespaces.Add(pointer);
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
            {
                return variable.Value;
            }
            else
            {
                for (int i = this.stack.Count - 1; i >= 0; i--)
                {
                    foreach (ScriptNamespace pointer in this.stack[i].UsingNamespaces)
                    {
                        if (pointer.TryGetVariable(name, out variable))
                            return variable.Value;
                    }
                }
            }

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

        private ScriptNamespace GetNamespace(string name)
        {
            string[] nameParts = name.Split('.');
            return this.GetNamespace(nameParts, nameParts.Length);
        }

        private ScriptNamespace GetNamespace(string[] nameParts, int length)
        {
            ScriptNamespace pointer = this.globals;
            ScriptVariable variable;
                        
            for (int i = 0; i < length; i++)
            {
                if (pointer.TryGetVariable(nameParts[i], out variable))
                {
                    if (variable.Value is ScriptNamespace)
                    {
                        pointer = variable.Value;
                    }
                    else
                    {
                        pointer = null;
                        break;
                    }
                }
                else
                {
                    pointer = null;
                    break;
                }
            }

            return pointer;
        }

        public dynamic GetGlobalVariable(string name)
        {
            dynamic value;
            if (this.TryGetGlobalVariable(name, out value))
            {
                return value;
            }

            throw new ScriptExecutionException(string.Format("Global variable \"{0}\" is not defined.", name));
        }

        public bool TryGetGlobalVariable(string name, out dynamic value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            value = null;
            bool result = false;

            ScriptVariable variable;

            if (name.IndexOf('.') > 0)
            {
                string[] nameParts = name.Split('.');

                ScriptNamespace pointer = this.GetNamespace(nameParts, nameParts.Length - 1);

                if (pointer != null && pointer.TryGetVariable(nameParts[nameParts.Length - 1], out variable))
                {
                    value = variable.Value;
                    result = true;
                }
            }
            else
            {
                if (this.globals.TryGetVariable(name, out variable))
                {
                    value = variable.Value;
                    result = true;
                }
            }

            return result;
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

            dynamic value;
            if (this.TryGetGlobalVariable(name, out value))
            {
                if (value is ScriptTypeSet)
                {
                    typeSet = (ScriptTypeSet)value;   
                }
                else if (value is ScriptType)
                {
                    typeSet = new ScriptTypeSet((ScriptType)value);
                    this.SetGlobalVariable(name, typeSet, true);
                }
                else
                {
                    throw new ScriptExecutionException(string.Format("\"{0}\" is already set and is not a ScriptTypeSet or a ScriptType.", name));
                }

				typeSet.AddType(new ScriptType(type));
            }
            else
            {
                this.SetGlobalVariable(name, new ScriptType(type), true);
            }
        }
    }
}
