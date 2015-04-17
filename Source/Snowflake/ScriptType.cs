using System;
using System.Linq;

namespace Snowflake
{
    public class ScriptType
    {
        public Type Type
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
        
        public ScriptType[] GenericArgs
        {
            get;
            private set;
        }

        public ScriptType(string name)
            : this(name, null)
        {
        }

        public ScriptType(string name, params ScriptType[] genericArgs)
        {
            this.Type = null;
            this.Name = name;
            this.GenericArgs = genericArgs;
        }

        public ScriptType(ScriptExecutionContext context, dynamic value, params ScriptType[] genericArgs)
        {
            if (value is ScriptTypeSet)
            {
                ScriptTypeSet typeSet = (ScriptTypeSet)value;

                Type type;
                typeSet.TryGetValue(genericArgs.Length, out type);

                this.Type = type.MakeGenericType(genericArgs.Select(x => GetType(context, x)).ToArray());
                this.Name = null;
            }

            this.GenericArgs = genericArgs;
        }

        // TODO: This is duplicated from the script class...
        private static Type GetType(ScriptExecutionContext context, ScriptType scriptType)
        {
            Type type = null;

            if (scriptType.GenericArgs != null && scriptType.GenericArgs.Length > 0)
            {
                type = context.GetType(scriptType.Name, scriptType.GenericArgs.Length);
                type = type.MakeGenericType(scriptType.GenericArgs.Select(x => GetType(context, x)).ToArray());
            }
            else
            {
                type = context.GetType(scriptType.Name, 0);
            }

            return type;
        }
    }
}
