using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptNamespace : DynamicObject
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
    }
}
