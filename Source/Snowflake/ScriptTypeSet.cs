using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptTypeSet
    {
        Dictionary<int, Type> types;

        public Type this[int index]
        {
            get { return this.types[index]; }
            set { this.types[index] = value; }
        }

        public ScriptTypeSet()
        {
            this.types = new Dictionary<int, Type>();
        }

        public ScriptTypeSet(Type type)
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
}
