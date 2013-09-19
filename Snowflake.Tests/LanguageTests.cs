using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using Snowsoft.SnowflakeScript.Execution;
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

		private void AssertScriptIsExecutionException(string script)
		{
			ScriptEngine engine = new ScriptEngine();
			Assert.Throws<ScriptExecutionException>(() =>
			{
				engine.Execute(script);
			});
		}

		#region Function Calls

		[Test]
		public void Function_Call_With_One_Arg()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(x) {
	return x + x;
};

return doubleIt(21);");
		}

		[Test]
		public void Function_Call_With_Two_Args()
		{
			AssertScriptReturnValue(42, @"
var add = func(x, y) {
	return x + y;
};

return add(21, 21);");
		}
		
		#endregion

		#region Return Values

		[Test]
		public void Script_Return_Value_Is_Null_When_No_Return_Value()
		{
			AssertScriptReturnValue<object>(null, "42 + 3;");
		}

		[Test]
		public void Script_Return_Value_Is_Returned()
		{
			AssertScriptReturnValue(42, "return 42;");
		}

		[Test]
		public void Script_Return_Value_Is_Correct_When_Returning_Function_Call()
		{
			AssertScriptReturnValue(42, @"
var x = func() {
	return 42;
};

return x();");
		}

		[Test]
		public void Return_Value_Is_Correct_When_Call_Stack_Is_Multiple_Levels_Deep()
		{
			AssertScriptReturnValue(42, @"
var x = func() {
	return 21;
};

var y = func() {
	return x() + x();
};

return y();");
		}

		#endregion

		#region String Operations

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
				
		#region Variable Declarations

		[Test]
		public void Variable_Declared_Twice_Is_Error()
		{
			AssertScriptIsExecutionException("var x = 42; var x = 12;");
		}

		[Test]
		public void Variable_Declared_Inside_Function_With_Same_Name_As_Global()
		{
			AssertScriptReturnValue(12, @"
var x = 42;
var doIt = func() {
	var x = 12;
	return x;
};
return doIt();");
		}

		[Test]
		public void Variable_Declared_With_Initial_Value_Set_To_Other_Variable()
		{
			AssertScriptReturnValue(42, @"
var x = 42;
var y = x;
x = 21;
return y;");
		}

		#endregion

		#region Varaible Operations

		[Test]
		public void Add_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(12, @"
var x = 4;
var y = 8;
return x + y;");
		}

		[Test]
		public void Add_Int_And_Int_Variable()
		{
			AssertScriptReturnValue(8, @"
var x = 4;
return 4 + x;");
		}

		[Test]
		public void Subtract_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(4, @"
var x = 8;
var y = 4;
return x - y;");
		}
		
		#endregion
	}
}
