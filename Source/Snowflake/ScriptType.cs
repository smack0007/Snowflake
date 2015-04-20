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
        
        public ScriptType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.Type = type;
        }
        
        public static ScriptType FromValue(dynamic value, params ScriptType[] genericArgs)
        {
            Type type = null;

            if (value is ScriptTypeSet)
            {
                ScriptTypeSet typeSet = (ScriptTypeSet)value;
                                
                if (genericArgs != null && genericArgs.Length > 0)
                {
                    typeSet.TryGetValue(genericArgs.Length, out type);
                    type = type.MakeGenericType(genericArgs.Select(x => x.Type).ToArray());
                }
                else
                {
                    typeSet.TryGetValue(0, out type);
                }
            }
            else if (value is ScriptType)
            {
                type = ((ScriptType)value).Type;
            }
            
            if (type == null)
                throw new ScriptExecutionException("Unable to create ScriptType from value.");

            return new ScriptType(type);
        }
    }
}
