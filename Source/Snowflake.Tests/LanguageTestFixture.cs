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
			ScriptEngine engine = new ScriptEngine();
			Console.WriteLine(engine.GenerateCode(script));
			Assert.Equal(expectedValue, engine.Execute(script));
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
