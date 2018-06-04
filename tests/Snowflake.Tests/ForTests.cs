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
	}
}
