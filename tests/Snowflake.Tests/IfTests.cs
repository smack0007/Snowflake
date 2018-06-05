using Xunit;

namespace Snowflake.Tests
{
    public class IfTests : LanguageTestFixture
	{
		[Fact]
		public void If_Expression_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
if (true) {
	return 42;
}

return 0;
");
		}

		[Fact]
		public void If_Expression_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
if (false) {
	return 42;
}

return 0;
");
		}

		[Fact]
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

		[Fact]
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

		[Fact]
		public void If_Expression_With_Variable_Which_Evaluates_To_True_Is_Executed()
		{
			AssertScriptReturnValue(42, @"
var test = 42;

if (test == 42) {
	return 42;
}

return 0;
");
		}

		[Fact]
		public void If_Expression_With_Variable_Which_Evaluates_To_False_Is_Not_Executed()
		{
			AssertScriptReturnValue(0, @"
var test = 21;

if (test == 42) {
	return 42;
}

return 0;
");
		}

        [Fact]
		public void If_Can_Shadow_Variables()
		{
			AssertScriptReturnValue(42, @"
var test = 21;

if (true) {
    var test = 42;
	return test;
}

return test;
");
		}

        [Fact]
		public void If_Can_Return()
		{
			AssertScriptReturnValue(42, @"
if (true) {
	return 42;
}

return 21;
");
		}
	}
}
