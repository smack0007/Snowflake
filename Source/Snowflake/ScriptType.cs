using System;

namespace Snowflake
{
    public class ScriptType
    {
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

        public ScriptType(string name, ScriptType[] genericArgs)
        {
            this.Name = name;
            this.GenericArgs = genericArgs;
        }
    }
}
