using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Snowsoft.SnowflakeScript
{
	public abstract class Script
	{		
		public string Id
		{
			get;
			internal set;
		}

		protected internal ScriptEngine Engine
		{
			get;
			set;
		}

		public Script()
            : base()
        {
        }

		protected internal abstract object Execute();

		protected dynamic Invoke(dynamic func, params dynamic[] args)
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

		protected dynamic GetGlobalVariable(string name)
		{
			return this.Engine.GetGlobalVariableIntern(name);
		}
	}
}
