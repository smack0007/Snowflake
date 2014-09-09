using System;

namespace Snowflake
{
	public sealed class ScriptFunction
	{
		Delegate func;
		dynamic[] defaults;

		public ScriptFunction(Delegate func, params dynamic[] defaults)
		{
			this.func = func;
			this.defaults = defaults;
		}

		public dynamic Invoke(params dynamic[] args)
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

			return ((Delegate)this.func).DynamicInvoke(args);
		}
	}
}
