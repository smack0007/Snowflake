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
				dynamic v1 = new ScriptFunction(new Func<dynamic, dynamic>((v2) => { 
					context.PushStackFrame("buildMultiplier");
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v3) => { 
							context.PushStackFrame("<anonymous>");
							try {
								return (v2 * v3);
							} finally {
								context.PopStackFrame();
							}
						 }), null);
					} finally {
						context.PopStackFrame();
					}
				 }), null);
				dynamic v4 = Invoke(context, v1, 5);
				for (dynamic v5 = 0; (v5 < 10); v5 += 1) {
					Invoke(context, context.GetGlobalVariable("print"), ((("5 * " + v5) + " = ") + Invoke(context, v4, v5)));
				}
				dynamic v6 = new ScriptList { 1, 2, 3, 4, 5 };
				Invoke(context, context.GetGlobalVariable("print"), v6.Count);
				dynamic v7 = Construct(context, "Person");
				v7.FirstName = "Bob";
				v7.LastName = "Freeman";
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
