using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class OrderOfOperationsTests : LanguageTestFixture
	{
		[Test]
		public void Without_Parens_Add_And_Divide_Operations_Are_Done_In_Correct_Order()
		{
			AssertScriptReturnValue(4, "return 2 + 4 / 2;");
		}

		[Test]
		public void Parens_Operation_Done_First()
		{
			AssertScriptReturnValue(3, "return (2 + 4) / 2;");
		}
		
		[Test]
		public void Add_Integers()
		{
			AssertScriptReturnValue(6, "return 1 + 2 + 3;");
		}

		[Test]
		public void Add_And_Subtract_Integers()
		{
			AssertScriptReturnValue(1, "return 2 - 4 + 3;");
		}

		[Test]
		public void Add_Strings()
		{
			AssertScriptReturnValue("Hello World again!", "return \"Hello\" + \" World \" + \"again!\";");
		}

		[Test]
		public void Negate_Operator_With_Integer()
		{
			AssertScriptReturnValue(0, "return 1 + -1;");
		}

		[Test]
		public void Negate_Operator_With_Expression_In_Parens()
		{
			AssertScriptReturnValue(-42, "return -(21 + 21);");
		}
	}
}
