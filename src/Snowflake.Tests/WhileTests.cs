using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
