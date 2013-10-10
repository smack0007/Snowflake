using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class VariableDeclarationTests : LanguageTestFixture
	{
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
	}
}
