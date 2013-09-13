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
		private void AssertScriptReturnValue<T>(T expectedValue, string script)
		{
			ScriptEngine engine = new ScriptEngine();
			Assert.AreEqual(expectedValue, engine.Execute(script));
		}

		#region String Adds

		[Test]
		public void Add_String_And_String()
		{
			AssertScriptReturnValue("Hello World!", "return \"Hello\" + \" World!\";");
		}

		[Test]
		public void Add_String_And_Character()
		{
			AssertScriptReturnValue("HelloC", "return \"Hello\" + 'C';");
		}

		[Test]
		public void Add_String_And_Bool()
		{
			AssertScriptReturnValue("Hellotrue", "return \"Hello\" + true;");
		}

		[Test]
		public void Add_String_And_Int()
		{
			AssertScriptReturnValue("Hello42", "return \"Hello\" + 42;");
		}

		[Test]
		public void Add_String_And_Float()
		{
			AssertScriptReturnValue("Hello1.1", "return \"Hello\" + 1.1;");
		}

		#endregion

		#region Return Values

		[Test]
		public void Script_Return_Value_Is_Null_When_No_Return_Value()
		{
			AssertScriptReturnValue<object>(null, "42 + 3;");
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
