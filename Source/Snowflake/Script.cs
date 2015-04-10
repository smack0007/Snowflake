using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Snowflake
{
    public abstract class Script
	{		
		public Script()
            : base()
        {
        }

		public abstract dynamic Execute(ScriptExecutionContext context);

        private static T ConstructObject<T>(object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }

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

        protected static dynamic Construct(ScriptExecutionContext context, ScriptType scriptType, params dynamic[] args)
        {
            Type type = GetType(context, scriptType);
            
            dynamic instance = typeof(Script)
                .GetMethod("ConstructObject", BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(type)
                .Invoke(null, new object[] { args });

            return instance;
        }

		protected static dynamic Invoke(ScriptExecutionContext context, dynamic func, params dynamic[] args)
		{
			if (func is ScriptFunction)
			{
				return ((ScriptFunction)func).Invoke(args);
			}
			else
			{
				return func.DynamicInvoke(args);
			}
		}
	}
}
