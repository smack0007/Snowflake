using Xunit;
using Snowflake.Execution;

namespace Snowflake.Tests
{
    public class VariableDeclarationTests : LanguageTestFixture
	{
		[Fact]
		public void Variable_Declared_Twice_Is_Error()
		{
			AssertScriptIsException<ScriptExecutionException>("var x = 42; var x = 12;");
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
		public void Undeclarded_Variable_Is_Error()
		{
			AssertScriptIsException<ScriptExecutionException>(@"return x;");
		}

		[Fact]
		public void Variable_Declared_Inside_Function_Is_Not_Defined_Outside_Function()
		{
            AssertScriptIsException<ScriptExecutionException>(@"
var doIt = func() {
	var x = 5;
	return x;
};

return x;");
		}

		[Fact]
		public void Variable_Declared_As_Function_Arg_Is_Not_Defined_Outside_Function()
		{
            AssertScriptIsException<ScriptExecutionException>(@"
var doIt = func(x) {
	return x;
};

return x;");
		}
	}
}
