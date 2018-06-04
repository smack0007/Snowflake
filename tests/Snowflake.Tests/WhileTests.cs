using Xunit;

namespace Snowflake.Tests
{
    public class WhileTests : LanguageTestFixture
    {
		[Fact]
		public void While_False_Body_Is_Never_Executed()
		{
			AssertScriptReturnValue(0, @"
var x = 0;

while (false) {
	x += 1;
}

return x;
");
		}

        [Fact]
        public void While_Exits_Correctly()
        {
            AssertScriptReturnValue(10, @"
var x = 0;

while (x != 10) {
	x += 1;
}

return x;
");
        }


        [Fact]
        public void While_Can_Contain_Return()
        {
            AssertScriptReturnValue(10, @"
var x = 0;

while (x != 10) {
	return 10;
}

return x;
");
        }

        [Fact]
        public void While_Can_Shadow_Variables()
        {
            AssertScriptReturnValue(42, @"
var x = 10;

while (true) {
	const x = 42;
    return x;
}

return x;
");
        }
    }
}
