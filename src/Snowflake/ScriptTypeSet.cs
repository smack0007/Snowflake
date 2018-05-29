using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake
{
    public class ScriptTypeSet : DynamicObject
    {
        private readonly Dictionary<int, ScriptType> types;
        
        public ScriptType this[int index]
        {
            get { return this.types[index]; }
        }

		public int Count
		{
			get { return this.types.Keys.Count; }
		}

        public ScriptTypeSet()
        {
            this.types = new Dictionary<int, ScriptType>();
        }

        public ScriptTypeSet(ScriptType type)
            : this()
        {
			this.AddType(type);
        }

		public void AddType(ScriptType type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (!type.IsGenericType || !type.IsGenericTypeDefinition)
				throw new ScriptExecutionException("ScriptTypeSet may only contain generic types which are also generic type definitions.");

			if (!this.types.ContainsKey(type.GenericArgumentCount))
			{
				this.types[type.GenericArgumentCount] = type;
			}
			else
			{
				throw new ScriptExecutionException(string.Format("Unable to add type \"{0}\" to ScriptTypeSet.", type.Type.Name));
			}
						
			this.types[type.GenericArgumentCount] = type;
		}

        public bool ContainsKey(int key)
        {
            return this.types.ContainsKey(key);
        }

        public bool TryGetType(int key, out ScriptType value)
        {
            return this.types.TryGetValue(key, out value);
        }
               
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = null;

            ScriptType type;
            if (this.types.TryGetValue(0, out type))
                return type.TryInvokeMember(binder, args, out result);

            return base.TryInvokeMember(binder, args, out result);
        }
    }
}
