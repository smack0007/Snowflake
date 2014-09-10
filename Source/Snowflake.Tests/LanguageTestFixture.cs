using Xunit;
using Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	public abstract class LanguageTestFixture
	{
		public void AssertScriptReturnValue<T>(T expectedValue, string script)
		{
			AssertScriptReturnValue<T>((x) => Assert.Equal(expectedValue, x), script);
		}

		public void AssertScriptReturnValue<T>(Action<T> assertAction, string script)
		{
			ScriptEngine engine = new ScriptEngine();
			Console.WriteLine(engine.GenerateCode(script));
			var result = (T)engine.Execute(script);
			assertAction(result);
		}

		public void AssertScriptIsException<T>(string script)
			where T : Exception
		{
			ScriptEngine engine = new ScriptEngine();
			Assert.Throws<T>(() =>
			{
				Console.WriteLine(engine.GenerateCode(script));
				engine.Execute(script);
			});
		}
	}
}
