using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.CodeGeneration;

namespace Snowflake.Tests
{
	public class VariableDeclarationTests : LanguageTestFixture
	{
		[Fact]
		public void Variable_Declared_Twice_Is_Error()
		{
			AssertScriptIsException<CodeGenerationException>("var x = 42; var x = 12;");
		}

		[Fact]
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

		[Fact]
		public void Function_Arg_With_Same_Name_As_Global()
		{
			AssertScriptReturnValue(12, @"
var x = 42;
var doIt = func(x) {
	return x;
};
return doIt(12);");
		}

		[Fact]
		public void Variable_Declared_With_Initial_Value_Set_To_Other_Variable()
		{
			AssertScriptReturnValue(42, @"
var x = 42;
var y = x;
x = 21;
return y;");
		}

		[Fact]
		public void Undeclarded_Variable_Is_Undefined()
		{
			AssertScriptReturnValue(true, @"return x == undef;");
		}

		[Fact]
		public void Variable_Declared_Inside_Function_Is_Undefined_Outside_Function()
		{
			AssertScriptReturnValue(true, @"
var doIt = func() {
	var x = 5;
	return x;
};

return x == undef;");
		}

		[Fact]
		public void Variable_Declared_As_Function_Arg_Is_Undefined_Outside_Function()
		{
			AssertScriptReturnValue(true, @"
var doIt = func(x) {
	return x;
};

return x == undef;");
		}

		[Fact]
		public void Variable_Declared_With_Same_Name_Inside_If_Block_Is_Ok()
		{
			AssertScriptReturnValue(true, @"
var x = 42;
var y = 0;

if (y <= 0) {
	var x = ""abc"";
	y += 1;
}

return true;");
		}

		[Fact]
		public void Variable_Declared_With_Same_Name_Inside_While_Block_Is_Ok()
		{
			AssertScriptReturnValue(true, @"
var x = 42;
var y = 0;

while (y <= 0) {
	var x = ""abc"";
	y += 1;
}

return true;");
		}

		[Fact]
		public void Variable_Declared_With_Same_Name_Inside_For_Block_Is_Ok()
		{
			AssertScriptReturnValue(true, @"
var x = 42;
var y = 0;

for (var i = 0; i < 1; i += 1) {
	var x = ""abc"";
	y += 1;
}

return true;");
		}

		[Fact]
		public void Variable_Declared_With_Same_Name_Inside_ForEach_Block_Is_Ok()
		{
			AssertScriptReturnValue(true, @"
var x = 42;
var y = 0;

foreach (var i in [ 1, 2, 3 ]) {
	var x = ""abc"";
	y += 1;
}

return true;");
		}
	}
}
