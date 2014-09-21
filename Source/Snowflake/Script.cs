using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Snowflake
{
    public abstract class Script
	{		
		public Script()
            : base()
        {
        }

		public abstract dynamic Execute(ScriptExecutionContext context);

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
