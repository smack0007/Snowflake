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
				context.DeclareVariable("foo", new ScriptFunction(new Func<dynamic>(() => { 
					context.PushStackFrame("foo");
					bool isError1 = false;
					try {
						Invoke(context, context["bar"]);
						return null;
					} catch(Exception) {
						isError1 = true;
						throw;
					} finally {
						if (!isError1) {
							context.PopStackFrame();
						}
					}
				})));
				context.DeclareVariable("bar", new ScriptFunction(new Func<dynamic>(() => { 
					context.PushStackFrame("bar");
					bool isError2 = false;
					try {
						context["Console"].WriteLine("bar");
						return null;
					} catch(Exception) {
						isError2 = true;
						throw;
					} finally {
						if (!isError2) {
							context.PopStackFrame();
						}
					}
				})));
				Invoke(context, context["foo"]);
				context.DeclareVariable("timeSpan", Construct(context, new ScriptType("System.TimeSpan"), new dynamic[] { 1, 2, 3 }));
				context["Console"].WriteLine(context["timeSpan"]);
				context.DeclareVariable("tuple1", Construct(context, new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int") }), new dynamic[] { 21 }));
				context["Console"].WriteLine(context["tuple1"]);
				context.DeclareVariable("tuple2", Construct(context, new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int"), new ScriptType("string") }), new dynamic[] { 42, "foo" }));
				context["Console"].WriteLine(context["tuple2"]);
				context.DeclareVariable("tuple3", Construct(context, new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int"), new ScriptType("string") }), new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int"), new ScriptType("string") }) }), new dynamic[] { Construct(context, new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int"), new ScriptType("string") }), new dynamic[] { 42, "foo" }), Construct(context, new ScriptType("System.Tuple", new ScriptType[] { new ScriptType("int"), new ScriptType("string") }), new dynamic[] { 21, "bar" }) }));
				context["Console"].WriteLine(context["tuple3"]);
				context.DeclareVariable("person1", Construct(context, new ScriptType("Namespace.Person")));
				context["person1"].FirstName = "Bob";
				context["person1"].LastName = "Freeman";
				context.DeclareVariable("person2", Construct(context, new ScriptType("Namespace.Person")));
				context["person2"].FirstName = "Joe";
				context["person2"].LastName = "Montana";
				context["person1"].Friends.Add(context["person2"]);
				context["Console"].WriteLine(context["person1"].Friends[0].FirstName);
				context["Console"].WriteLine(context["person1"]);
				context.DeclareVariable("buildMultiplier", new ScriptFunction(new Func<dynamic, dynamic>((v10) => { 
					context.PushStackFrame("buildMultiplier");
					context.DeclareVariable("x", v10);
					bool isError3 = false;
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v11) => { 
							context.PushStackFrame("<anonymous>");
							context.DeclareVariable("y", v11);
							context.DeclareVariable("x", v10);
							bool isError4 = false;
							try {
								return (context["x"] * context["y"]);
							} catch(Exception) {
								isError4 = true;
								throw;
							} finally {
								if (!isError4) {
									context.PopStackFrame();
								}
							}
						}), null);
					} catch(Exception) {
						isError3 = true;
						throw;
					} finally {
						if (!isError3) {
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
				foreach (dynamic v15 in context["values"]) {
					context.SetVariable("value", v15);
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
