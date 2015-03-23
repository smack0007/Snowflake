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
				context.DeclareVariable("foo", new ScriptFunction(new Func<dynamic, dynamic>((v2) => { 
					context.PushStackFrame("foo");
					context.DeclareVariable("x", v2);
					bool isError1 = false;
					try {
						return ((context["x"] * 4) + Invoke(context, context["bar"], context["x"]));
					} catch(Exception) {
						isError1 = true;
						throw;
					} finally {
						if (!isError1) {
							context.PopStackFrame();
						}
					}
				}), null));
				context.DeclareVariable("bar", new ScriptFunction(new Func<dynamic, dynamic>((v4) => { 
					context.PushStackFrame("bar");
					context.DeclareVariable("x", v4);
					bool isError2 = false;
					try {
						return (context["x"] * 2);
					} catch(Exception) {
						isError2 = true;
						throw;
					} finally {
						if (!isError2) {
							context.PopStackFrame();
						}
					}
				}), null));
				return Invoke(context, context["foo"], 7);
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
	}
}
