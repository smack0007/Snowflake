using System;
using Snowflake.Execution;

namespace Snowflake
{
	public sealed class ScriptFunction
	{
		Delegate func;
		dynamic[] defaults;
		dynamic[] captures;

		public ScriptFunction(Delegate func, dynamic[] defaults, dynamic[] captures)
		{
			this.func = func;
			this.defaults = defaults;
			this.captures = captures;
		}

		public dynamic Invoke(ScriptExecutionContext context, params dynamic[] args)
		{
			if (this.defaults != null && args.Length < this.defaults.Length)
			{
				dynamic[] newArgs = new dynamic[this.defaults.Length];

				for (int i = 0; i < args.Length; i++)
				{
					newArgs[i] = args[i];
				}

				for (int i = args.Length; i < this.defaults.Length; i++)
				{
					newArgs[i] = this.defaults[i];
				}

				args = newArgs;
			}
						
			object[] invokeArgs = new object[args.Length + 2];
			invokeArgs[0] = context;
			invokeArgs[1] = this.captures;

			for (int i = 0; i < args.Length; i++)
				invokeArgs[i + 2] = args[i];

			return this.func.DynamicInvoke(invokeArgs);
		}
	}
}
