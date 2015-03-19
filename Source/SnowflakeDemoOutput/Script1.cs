using System;
using System.Collections.Generic;

namespace Snowflake.Generated
{
	public class Script1 : Script
	{
		public override dynamic Execute(ScriptExecutionContext context)
		{
			try
			{
				dynamic v1 = Construct(context, "Person");
				Invoke(context, context.GetGlobalVariable("print"), v1.GetType());
				v1.FirstName = "Bob";
				v1.LastName = "Freeman";
				dynamic v2 = new ScriptFunction(new Func<dynamic, dynamic>((v3) => { 
					context.PushStackFrame("buildMultiplier");
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v4) => { 
							context.PushStackFrame("<anonymous>");
							try {
								return (v3 * v4);
							} finally {
								context.PopStackFrame();
							}
						 }), null);
					} finally {
						context.PopStackFrame();
					}
				 }), null);
				dynamic v5 = Invoke(context, v2, 5);
				for (dynamic v6 = 0; (v6 < 10); v6 += 1) {
					Invoke(context, context.GetGlobalVariable("print"), ((("5 * " + v6) + " = ") + Invoke(context, v5, v6)));
				}
				dynamic v7 = new ScriptList { 1, 2, 3, 4, 5 };
				Invoke(context, context.GetGlobalVariable("print"), v7.Count);
				context.GetGlobalVariable("export").number = 42;
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
	}
}
