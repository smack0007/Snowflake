using System;
using System.Collections.Generic;
using System.Dynamic;
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

        protected static dynamic Construct(ScriptExecutionContext context, string typeName, params dynamic[] args)
        {
            Type type = context.GetType(typeName);

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
