using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using Snowsoft.SnowflakeScript.Execution;
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
			Assert.AreEqual(expectedValue, engine.Execute(script));
		}

		public void AssertScriptIsExecutionException(string script)
		{
			ScriptEngine engine = new ScriptEngine();
			Assert.Throws<ScriptExecutionException>(() =>
			{
				engine.Execute(script);
			});
		}
	}
}
