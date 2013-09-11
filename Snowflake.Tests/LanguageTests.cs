using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class LanguageTests
	{
#region Return Values

		private void AssertScriptReturnValue<T>(T expectedValue, string script)
		{
			ScriptEngine engine = new ScriptEngine();
			Assert.AreEqual(expectedValue, engine.Execute(script));
		}

		[Test]
		public void Script_Return_Value_Is_Correct()
		{
			AssertScriptReturnValue(42, "return 42;");
		}

		[Test]
		public void Script_Return_Value_Is_Correct_When_Returning_Function_Call()
		{
			AssertScriptReturnValue(42, @"var x = func() { return 42; }; return x();");
		}

		[Test]
		public void Return_Value_Is_Correct_When_Call_Stack_Is_Multiple_Levels_Deep()
		{
			AssertScriptReturnValue(42, @"var x = func() { return 21; }; var y = func() { return x() + x(); }; return y();");
		}

#endregion
	}
}
