using Xunit;

namespace Snowflake.Tests
{
    public class ReturnValueTests : LanguageTestFixture
	{		
		[Fact]
		public void Script_Return_Value_Is_Returned()
		{
			AssertScriptReturnValue(42, "return 42;");
		}

		[Fact]
		public void Script_Return_Value_Is_Correct_When_Returning_Function_Call()
		{
			AssertScriptReturnValue(42, @"
var x = func() {
	return 42;
};

return x();");
		}

		[Fact]
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
	}
}
