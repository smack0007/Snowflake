using Snowflake.Execution;
using Xunit;

namespace Snowflake.Tests
{
    public class ForTests : LanguageTestFixture
	{
		[Fact]
		public void For_With_False_Eval_Expr_Body_Never_Executed()
		{
			AssertScriptReturnValue(0, @"
var y = 0;

for (var x = 0; false; x += 1) {
	y += 1;
}

return y;
");
		}

        [Fact]
		public void Variables_Declared_In_Initializer_Are_Scoped()
		{
			AssertScriptIsException<ScriptExecutionException>(@"
var y = 0;

for (var x = 42; false; x += 1) {
	y += 1;
}

return x;
");
		}

		[Fact]
		public void For_Exits_Correctly()
		{
			AssertScriptReturnValue(10, @"
var y = 0;

for (var x = 0; x != 10; x += 1) {
	y += 1;
}

return y;
");
		}

        [Fact]
		public void For_Can_Have_Prefix_Increment()
		{
			AssertScriptReturnValue(10, @"
var y = 0;

for (var x = 0; x != 10; ++x) {
	y += 1;
}

return y;
");
		}

        [Fact]
		public void For_Can_Have_Postfix_Increment()
		{
			AssertScriptReturnValue(10, @"
var y = 0;

for (var x = 0; x != 10; x++) {
	y += 1;
}

return y;
");
		}
	}
}
