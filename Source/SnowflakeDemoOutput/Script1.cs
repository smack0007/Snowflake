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
				dynamic v2 = Construct(context, "Person");
				v2.FirstName = "Joe";
				v2.LastName = "Montana";
				v1.Friends.Add(v2);
				Invoke(context, context.GetGlobalVariable("print"), v1.Friends[0].FirstName);
				dynamic v3 = new ScriptFunction(new Func<dynamic, dynamic>((v4) => { 
					context.PushStackFrame("buildMultiplier");
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v5) => { 
							context.PushStackFrame("<anonymous>");
							try {
								return (v4 * v5);
							} finally {
								context.PopStackFrame();
							}
						 }), null);
					} finally {
						context.PopStackFrame();
					}
				 }), null);
				dynamic v6 = Invoke(context, v3, 5);
				for (dynamic v7 = 0; (v7 < 10); v7 += 1) {
					Invoke(context, context.GetGlobalVariable("print"), ((("5 * " + v7) + " = ") + Invoke(context, v6, v7)));
				}
				dynamic v8 = new ScriptList { 1, 2, 3, 4, 5 };
				Invoke(context, context.GetGlobalVariable("print"), v8.Count);
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
