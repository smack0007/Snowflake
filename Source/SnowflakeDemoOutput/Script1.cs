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
				context.UsingNamespace("System");
				context["Console"].WriteLine("Hello World!");
				context.DeclareVariable("MaxMultiplier", isConst: true, value: 10);
				context.DeclareVariable("scriptListType", value: Invoke(context, context["import"], "Snowflake.ScriptList"));
				context.DeclareVariable("scriptList", value: Construct(context, ScriptType.FromValue(context["scriptListType"]), new dynamic[] { new ScriptList { "a", "b", "c" } }));
				context["Console"].WriteLine(context["scriptList"]);
				context.DeclareVariable("stringBuilderType", value: Invoke(context, context["import"], "System.Text.StringBuilder"));
				context.DeclareVariable("sb", value: Construct(context, ScriptType.FromValue(context["stringBuilderType"])));
				context["sb"].Append("Hello");
				context["sb"].Append(" World!");
				context["Console"].WriteLine(context["sb"]);
				context.DeclareVariable("foo", isConst: true, value: new ScriptFunction(new Func<dynamic>(() => { 
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
				context.DeclareVariable("bar", isConst: true, value: new ScriptFunction(new Func<dynamic>(() => { 
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
				context.DeclareVariable("timeSpanType", value: context["System"].TimeSpan);
				context.DeclareVariable("timeSpan", value: Construct(context, ScriptType.FromValue(context["timeSpanType"]), new dynamic[] { 1, 2, 3 }));
				context["System"].Console.WriteLine(context["timeSpan"]);
				context.DeclareVariable("tuple1", value: Construct(context, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"])), new dynamic[] { 21 }));
				context["Console"].WriteLine(context["tuple1"]);
				context.DeclareVariable("tuple2", value: Construct(context, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"]), ScriptType.FromValue(context["string"])), new dynamic[] { 42, "foo" }));
				context["Console"].WriteLine(context["tuple2"]);
				context.DeclareVariable("tuple3", value: Construct(context, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"]), ScriptType.FromValue(context["string"])), ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"]), ScriptType.FromValue(context["string"]))), new dynamic[] { Construct(context, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"]), ScriptType.FromValue(context["string"])), new dynamic[] { 42, "foo" }), Construct(context, ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"]), ScriptType.FromValue(context["string"])), new dynamic[] { 21, "bar" }) }));
				context["Console"].WriteLine(context["tuple3"]);
				context.DeclareVariable("tupleType", value: ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["int"])));
				context.DeclareVariable("tuple4", value: Construct(context, ScriptType.FromValue(context["tupleType"]), new dynamic[] { 42 }));
				context["System"].Console.WriteLine(context["tuple4"]);
				context.DeclareVariable("tuple5", value: Construct(context, ScriptType.FromValue(Invoke(context, new ScriptFunction(new Func<dynamic>(() => { 
					context.PushStackFrame("<anonymous>");
					context.DeclareVariable("System", context.GetGlobalVariable("System"));
					context.DeclareVariable("string", context.GetGlobalVariable("string"));
					context.DeclareVariable("int", context.GetGlobalVariable("int"));
					bool isError3 = false;
					try {
						return ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["string"]), ScriptType.FromValue(context["int"]));
					} catch(Exception) {
						isError3 = true;
						throw;
					} finally {
						if (!isError3) {
							context.PopStackFrame();
						}
					}
				})))), new dynamic[] { "Hello", 42 }));
				context["System"].Console.WriteLine(context["tuple5"]);
				context.DeclareVariable("person1", value: Construct(context, ScriptType.FromValue(context["Namespace"].Person)));
				context["person1"].FirstName = "Bob";
				context["person1"].LastName = "Freeman";
				context.DeclareVariable("person2", value: Construct(context, ScriptType.FromValue(context["Namespace"].Person)));
				context["person2"].FirstName = "Joe";
				context["person2"].LastName = "Montana";
				context["person1"].Friends.Add(context["person2"]);
				context["Console"].WriteLine(context["person1"].Friends[0].FirstName);
				context["Console"].WriteLine(context["person1"]);
				context.DeclareVariable("buildMultiplier", isConst: true, value: new ScriptFunction(new Func<dynamic, dynamic>((v19) => { 
					context.PushStackFrame("buildMultiplier");
					context.DeclareVariable("x", v19);
					bool isError4 = false;
					try {
						return new ScriptFunction(new Func<dynamic, dynamic>((v20) => { 
							context.PushStackFrame("<anonymous>");
							context.DeclareVariable("y", v20);
							context.DeclareVariable("x", v19);
							bool isError5 = false;
							try {
								return (context["x"] * context["y"]);
							} catch(Exception) {
								isError5 = true;
								throw;
							} finally {
								if (!isError5) {
									context.PopStackFrame();
								}
							}
						}), null);
					} catch(Exception) {
						isError4 = true;
						throw;
					} finally {
						if (!isError4) {
							context.PopStackFrame();
						}
					}
				}), null));
				context.DeclareVariable("x5", value: Invoke(context, context["buildMultiplier"], 5));
				for (context.DeclareVariable("i", value: 0); (context["i"] < context["MaxMultiplier"]); context["i"] += 1) {
					context["Console"].WriteLine(((("5 * " + context["i"]) + " = ") + Invoke(context, context["x5"], context["i"])));
				}
				context.DeclareVariable("values", value: new ScriptList { 5, 4, 3, 2, 1 });
				context.DeclareVariable("value");
				foreach (dynamic v24 in context["values"]) {
					context.SetVariable("value", v24);
					context["Console"].WriteLine(((("5 * " + context["value"]) + " = ") + Invoke(context, context["x5"], context["value"])));
				}
				Invoke(context, context["export"], "Script.Export.Number", 42);
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
	}
}
