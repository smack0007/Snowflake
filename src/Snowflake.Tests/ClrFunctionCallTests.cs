using System;
using Xunit;

namespace Snowflake.Tests
{
	public class ClrFunctionCallTests : LanguageTestFixture
	{
		[Fact]
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

		[Fact]
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

		[Fact]
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
