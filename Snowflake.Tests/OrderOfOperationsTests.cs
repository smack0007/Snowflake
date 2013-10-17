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
	}
}
