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
				context.DeclareVariable("person1", Construct(context, "Person"));
				context["person1"].FirstName = "Bob";
				context["person1"].LastName = "Freeman";
				context.DeclareVariable("person2", Construct(context, "Person"));
				context["person2"].FirstName = "Joe";
				context["person2"].LastName = "Montana";
				context["person1"].Friends.Add(context["person2"]);
				context["Console"].WriteLine(context["person1"].Friends[0].FirstName);
				context["Console"].WriteLine(context["person1"]);
				context.DeclareVariable("buildMultiplier", new ScriptFunction(new Func<dynamic, dynamic>((v4) => { 
					context.PushStackFrame("buildMultiplier");
					context.DeclareVariable("x", v4);
					bool isError1 = false;
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v5) => { 
							context.PushStackFrame("<anonymous>");
							context.DeclareVariable("y", v5);
							context.DeclareVariable("x", v4);
							bool isError2 = false;
							try {
								return (context["x"] * context["y"]);
							} catch(Exception) {
								isError2 = true;
								throw;
							} finally {
								if (!isError2) {
									context.PopStackFrame();
								}
							}
						 }), null);
					} catch(Exception) {
						isError1 = true;
						throw;
					} finally {
						if (!isError1) {
							context.PopStackFrame();
						}
					}
				 }), null));
				context.DeclareVariable("x5", Invoke(context, context["buildMultiplier"], 5));
				for (context.DeclareVariable("i", 0); (context["i"] < 10); context["i"] += 1) {
					context["Console"].WriteLine(((("5 * " + context["i"]) + " = ") + Invoke(context, context["x5"], context["i"])));
				}
				context.DeclareVariable("values", new ScriptList { 5, 4, 3, 2, 1 });
				context.DeclareVariable("value");
				foreach (dynamic v9 in context["values"]) {
					context.SetVariable("value", v9);
					context["Console"].WriteLine(((("5 * " + context["value"]) + " = ") + Invoke(context, context["x5"], context["value"])));
				}
				context["export"].number = 42;
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
	}
}
