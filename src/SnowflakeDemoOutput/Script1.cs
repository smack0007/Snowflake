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
				context.DeclareVariable("float1", value: 12.5);
				context.DeclareVariable("float2", value: 12.5f);
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
				context.DeclareVariable("foo", isConst: true, value: new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic>(Function1), null, null));
				context.DeclareVariable("bar", isConst: true, value: new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic>(Function2), null, null));
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
				context.DeclareVariable("tuple5", value: Construct(context, ScriptType.FromValue(Invoke(context, new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic>(Function3), null, new dynamic[] { context["System"], context["string"], context["int"] }))), new dynamic[] { "Hello", 42 }));
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
				context.DeclareVariable("buildMultiplier", isConst: true, value: new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic, dynamic>(Function4), new dynamic[] { null }, null));
				context.DeclareVariable("x5", value: Invoke(context, context["buildMultiplier"], 5));
				for (context.DeclareVariable("i", value: 0); (context["i"] < context["MaxMultiplier"]); context["i"] += 1) {
					context["Console"].WriteLine(((("5 * " + context["i"]) + " = ") + Invoke(context, context["x5"], context["i"])));
				}
				context.DeclareVariable("values", value: new ScriptList { 5, 4, 3, 2, 1 });
				context.DeclareVariable("value");
				foreach (dynamic v26 in context["values"]) {
					context.SetVariable("value", v26);
					context["Console"].WriteLine(((("5 * " + context["value"]) + " = ") + Invoke(context, context["x5"], context["value"])));
				}
				context.DeclareVariable("valueGen", value: new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic>(Function6), null, null));
				context.DeclareVariable("value2");
				foreach (dynamic v28 in Invoke(context, context["valueGen"])) {
					context.SetVariable("value2", v28);
					context["Console"].WriteLine(((("5 * " + context["value2"]) + " = ") + Invoke(context, context["x5"], context["value2"])));
				}
				Invoke(context, context["export"], "Script.Export.Number", 42);
				return null;
			}
			catch (Exception ex)
			{
				throw new ScriptExecutionException(ex.Message, context.GetStackFrames(), ex);
			}
		}
		
		private static dynamic Function1(ScriptExecutionContext context, dynamic[] captures)
		{
			context.PushStackFrame("foo");
			bool isError = false;
			try {
				Invoke(context, context["bar"]);
				return null;
			} catch(Exception) {
				isError = true;
				throw;
			} finally {
				if (!isError) {
					context.PopStackFrame();
				}
			}
		}
		
		private static dynamic Function2(ScriptExecutionContext context, dynamic[] captures)
		{
			context.PushStackFrame("bar");
			bool isError = false;
			try {
				context["Console"].WriteLine("bar");
				return null;
			} catch(Exception) {
				isError = true;
				throw;
			} finally {
				if (!isError) {
					context.PopStackFrame();
				}
			}
		}
		
		private static dynamic Function3(ScriptExecutionContext context, dynamic[] captures)
		{
			context.PushStackFrame("<anonymous>");
			context.DeclareVariable("System", captures[0]);
			context.DeclareVariable("string", captures[1]);
			context.DeclareVariable("int", captures[2]);
			bool isError = false;
			try {
				return ScriptType.FromValue(context["System"].Tuple, ScriptType.FromValue(context["string"]), ScriptType.FromValue(context["int"]));
			} catch(Exception) {
				isError = true;
				throw;
			} finally {
				if (!isError) {
					context.PopStackFrame();
				}
			}
		}
		
		private static dynamic Function4(ScriptExecutionContext context, dynamic[] captures, dynamic v21)
		{
			context.PushStackFrame("buildMultiplier");
			context.DeclareVariable("x", v21);
			bool isError = false;
			try {
				return new ScriptFunction(new Func<ScriptExecutionContext, dynamic[], dynamic, dynamic>(Function5), new dynamic[] { null }, new dynamic[] { context["x"] });
			} catch(Exception) {
				isError = true;
				throw;
			} finally {
				if (!isError) {
					context.PopStackFrame();
				}
			}
		}
		
		private static dynamic Function5(ScriptExecutionContext context, dynamic[] captures, dynamic v22)
		{
			context.PushStackFrame("<anonymous>");
			context.DeclareVariable("y", v22);
			context.DeclareVariable("x", captures[0]);
			bool isError = false;
			try {
				return (context["x"] * context["y"]);
			} catch(Exception) {
				isError = true;
				throw;
			} finally {
				if (!isError) {
					context.PopStackFrame();
				}
			}
		}
		
		private static IEnumerable<dynamic> Function6(ScriptExecutionContext context, dynamic[] captures)
		{
			yield return 1;
			yield return 2;
			yield return 3;
			yield return 4;
			yield return 5;
		}
	}
}
