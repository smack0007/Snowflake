using System;
using System.Collections.Generic;
using System.Dynamic;
using Snowflake.Execution;

namespace Snowflake
{
    public class ScriptNamespace : DynamicObject, IEnumerable<KeyValuePair<string, ScriptVariable>>
    {
        string name;
        Dictionary<string, ScriptVariable> variables;

        public ScriptNamespace Parent
        {
            get;
            private set;
        }
        
        public string Name
        {
            get
            {
                if (this.Parent != null && this.Parent.Parent != null)
                {
                    return this.Parent.Name + "." + this.name;
                }
                else
                {
                    return this.name;
                }
            }
        }
       
        public dynamic this[string index]
        {
            get { return this.variables[index]; }
        }

        public IEnumerable<string> Keys
        {
            get { return this.variables.Keys; }
        }

        public IEnumerable<ScriptVariable> Values
        {
            get { return this.variables.Values; }
        }

        public ScriptNamespace(string name)
            : this(name, null)
        {
        }

        public ScriptNamespace(string name, ScriptNamespace parent)
        {
            this.name = name;
            this.Parent = parent;
            this.variables = new Dictionary<string, ScriptVariable>();
        }
        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            ScriptVariable variable;
            result = null;

            if (this.variables.TryGetValue(binder.Name, out variable))
            {
                result = variable.Value;
                return true;
            }

            return false;
        }

        public ScriptNamespace DeclareSubNamespace(string namespaceName)
        {
            if (namespaceName == null)
                throw new ArgumentNullException("namespaceName");

            if (this.variables.ContainsKey(namespaceName))
                throw new ScriptExecutionException(string.Format("Cannot create sub namespace \"{0}\" because the name is already in use in the namespace \"{1}\".", namespaceName, this.Name));

            ScriptNamespace subNamespace = new ScriptNamespace(namespaceName, this);
            this.variables[namespaceName] = new ScriptVariable(subNamespace, true);

            return subNamespace;
        }

        public bool ContainsVariable(string name)
        {
            return this.variables.ContainsKey(name);
        }

        public bool TryGetVariable(string name, out ScriptVariable variable)
        {
            return this.variables.TryGetValue(name, out variable);
        }

        public void SetVariable(string name, ScriptVariable variable)
        {
            this.variables[name] = variable;
        }

        public IEnumerator<KeyValuePair<string, ScriptVariable>> GetEnumerator()
        {
            return this.variables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
