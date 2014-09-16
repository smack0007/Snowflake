using System;
using Snowflake.Parsing;
using Xunit;

namespace Snowflake.Tests
{	
	public class PostfixOperationTests : LanguageTestFixture
	{
		[Fact]
		public void Increment_Works_As_Expected()
		{
			AssertScriptReturnValue(3, @"
var x = 1;
var y = x++;
return x + y;
");
		}

		[Fact]
		public void Increment_Can_Not_Be_Chained()
		{
			AssertScriptIsException<SyntaxException>(@"
var x = 1;
x++++;
return x;
");
		}

		[Fact]
		public void Increment_Can_Not_Be_Chained_With_Parens()
		{
			AssertScriptIsException<SyntaxException>(@"
var x = 1;
(x++)++;
return x;
");
		}

		[Fact]
		public void Decrement_Works_As_Expected()
		{
			AssertScriptReturnValue(3, @"
var x = 2;
var y = x--;
return x + y;
");
		}

		[Fact]
		public void Decrement_Can_Not_Be_Chained()
		{
			AssertScriptIsException<SyntaxException>(@"
var x = 2;
x----;
return x;
");
		}

		[Fact]
		public void Decrement_Can_Not_Be_Chained_With_Parens()
		{
			AssertScriptIsException<SyntaxException>(@"
var x = 1;
(x--)--;
return x;
");
		}
	}
}
