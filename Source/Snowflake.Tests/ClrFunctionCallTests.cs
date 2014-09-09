using NUnit.Framework;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class ClrFunctionCallTests : LanguageTestFixture
	{
		[Test]
		public void Too_Few_Arguments_To_Clr_Method_Is_Error()
		{
			int value = 0;

			ScriptEngine engine = new ScriptEngine();
			engine.SetGlobalFunction<int>("DoIt", (x) => { value += x; });

			Assert.Throws<ScriptExecutionException>(() =>
			{
				engine.Execute(@"DoIt();");
			});
		}

		[Test]
		public void Too_Many_Arguments_To_Clr_Method_Is_Error()
		{
			int value = 0;

			ScriptEngine engine = new ScriptEngine();
			engine.SetGlobalFunction<int>("DoIt", (x) => { value += x; });

			Assert.Throws<ScriptExecutionException>(() =>
			{
				engine.Execute(@"DoIt(12, 12);");
			});
		}

		[Test]
		public void Invalid_Arguments_To_Clr_Method_Is_Error()
		{
			int value = 0;

			ScriptEngine engine = new ScriptEngine();
			engine.SetGlobalFunction<int>("DoIt", (x) => { value += x; });

			Assert.Throws<ScriptExecutionException>(() =>
			{
				engine.Execute(@"DoIt(""Hello World!"");");
			});
		}
	}
}
