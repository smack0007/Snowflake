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

		#region CLR Function Calls

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
		
		#endregion

		#region Function Calls

		[Test]
		public void Function_Call_With_One_Arg()
		{
			AssertScriptReturnValue(42, @"
var doubleIt = func(var x) {
	return x + x;
};

return doubleIt(21);");
		}

		[Test]
		public void Function_Call_With_Two_Args()
		{
			AssertScriptReturnValue(42, @"
var add = func(var x, var y) {
	return x + y;
};

return add(21, 21);");
		}

		[Test]
		public void Too_Few_Arguments_To_Script_Function_Is_Error()
		{
			AssertScriptIsExecutionException(@"
var doubleIt = func(var x) {
	return x + x;
};

return doubleIt();");
		}

		[Test]
		public void Too_Many_Arguments_To_Script_Function_Is_Error()
		{
			AssertScriptIsExecutionException(@"
var doubleIt = func(var x) {
	return x + x;
};

return doubleIt(21, 21);");
		}

        [Test]
        public void Function_With_No_Return_Statement_Returns_Undefined()
        {
            AssertScriptReturnValue(true, @"
var doIt = func() { };

return doIt() == undef;");
        }

		#endregion

		#region If

		[Test]
		public void If_Expression_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
if (true) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Expression_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
if (false) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Else_Block_Is_Executed_When_Expression_Evaluates_To_False()
		{
			AssertScriptReturnValue(21, @"
var result = 0;

if (false) {
	result = 42;
} else {
	result = 21;
}

return result;
");
		}

		[Test]
		public void If_Else_Block_Is_Not_Executed_When_Expression_Evaluates_To_False()
		{
			AssertScriptReturnValue(42, @"
var result = 0;

if (true) {
	result = 42;
} else {
	result = 21;
}

return result;
");
		}

		[Test]
		public void If_Expression_With_Variable_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
var test = true;

if (test) {
	return 42;
}

return 0;
");
		}

		[Test]
		public void If_Expression_With_Variable_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
var test = false;

if (test) {
	return 42;
}

return 0;
");
		}

		#endregion

		#region Logical Operations

		[Test]
		public void True_Equal_To_True()
		{
			AssertScriptReturnValue(true, "return true == true;");
		}

		[Test]
		public void True_Not_Equal_To_False()
		{
			AssertScriptReturnValue(true, "return true != false;");
		}

		[Test]
		public void True_And_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true && true;");
		}

		[Test]
		public void True_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return true && false;");
		}

		[Test]
		public void False_And_True_Is_False()
		{
			AssertScriptReturnValue(false, "return false && true;");
		}

		[Test]
		public void False_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false && false;");
		}

		[Test]
		public void True_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true || true;");
		}

		[Test]
		public void True_Or_False_Is_True()
		{
			AssertScriptReturnValue(true, "return true || false;");
		}

		[Test]
		public void False_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return false || true;");
		}

		[Test]
		public void False_Or_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false || false;");
		}

		[Test]
		public void Variable_True_And_Variable_True_Is_True()
		{
			AssertScriptReturnValue(true, "var x = true; var y = true; return x && y;");
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
        public void Function_Arg_With_Same_Name_As_Global()
        {
            AssertScriptReturnValue(12, @"
var x = 42;
var doIt = func(var x) {
	return x;
};
return doIt(12);");
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

		[Test]
		public void Undeclarded_Variable_Is_Undefined()
		{
			AssertScriptReturnValue(true, @"return x == undef;");
		}

		[Test]
		public void Variable_Declared_Inside_Function_Is_Undefined_Outside_Function()
		{
			AssertScriptReturnValue(true, @"
var doIt = func() {
	var x = 5;
	return x;
};

return x == undef;");
		}

		[Test]
		public void Variable_Declared_As_Function_Arg_Is_Undefined_Outside_Function()
		{
			AssertScriptReturnValue(true, @"
var doIt = func(var x) {
	return x;
};

return x == undef;");
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
