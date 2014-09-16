using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	public class OrderOfOperationsTests : LanguageTestFixture
	{
		[Fact]
		public void Without_Parens_Add_And_Divide_Operations_Are_Done_In_Correct_Order()
		{
			AssertScriptReturnValue(4, "return 2 + 4 / 2;");
		}

		[Fact]
		public void Parens_Operation_Done_First()
		{
			AssertScriptReturnValue(3, "return (2 + 4) / 2;");
		}
		
		[Fact]
		public void Add_Integers()
		{
			AssertScriptReturnValue(6, "return 1 + 2 + 3;");
		}

		[Fact]
		public void Add_And_Subtract_Integers()
		{
			AssertScriptReturnValue(1, "return 2 - 4 + 3;");
		}

		[Fact]
		public void Add_Strings()
		{
			AssertScriptReturnValue("Hello World again!", "return \"Hello\" + \" World \" + \"again!\";");
		}

		[Fact]
		public void Negate_Operator_With_Integer()
		{
			AssertScriptReturnValue(0, "return 1 + -1;");
		}

		[Fact]
		public void Negate_Operator_With_Expression_In_Parens()
		{
			AssertScriptReturnValue(-42, "return -(21 + 21);");
		}

		[Fact]
		public void Negate_Operator_And_Subtraction_Operation()
		{
			AssertScriptReturnValue(42, "return 21 - -21;");
		}

		[Fact]
		public void Prefix_Increment_Works_As_Expected()
		{
			AssertScriptReturnValue(4, @"
var x = 1;
var y = ++x;
return x + y;");
		}

		[Fact]
		public void Prefix_Decrement_Works_As_Expected()
		{
			AssertScriptReturnValue(2, @"
var x = 2;
var y = --x;
return x + y;");
		}
	}
}
