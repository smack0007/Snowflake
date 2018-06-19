using Xunit;

namespace Snowflake.Tests
{
    public class ElementAccessTests : LanguageTestFixture
	{
		[Fact]
		public void ElementAccess_Can_Be_Chained()
		{
			AssertScriptReturnValue(42, @"
var foo = [ [ 1, 2, 3 ], [ 4, 5, 42 ] ];
return foo[1][2];");
		}

		[Fact]
		public void ElementAccess_Can_Contain_Expression()
		{
			AssertScriptReturnValue(42, @"
var foo = [ [ 1, 2, 3 ], [ 4, 5, 42 ] ];
var bar = 1;
return foo[bar][bar + 1];");
		}

		[Fact]
		public void ElementAccess_Can_Contain_Function_Call()
		{
			AssertScriptReturnValue(42, @"
var foo = [ [ 1, 2, 3 ], [ 4, 5, 42 ] ];
var bar = func() { return 1; };
return foo[bar()][bar() + 1];");
		}
	}
}
