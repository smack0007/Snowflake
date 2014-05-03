using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
    [TestFixture]
    public class WhileTests : LanguageTestFixture
    {
		[Test]
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

		[Test]
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
