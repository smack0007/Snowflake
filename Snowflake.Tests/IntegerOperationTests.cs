using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class IntegerOperationTests : LanguageTestFixture
	{
		[Test]
		public void Add_2_Integers()
		{
			AssertScriptReturnValue(42, "return 21 + 21;");
		}

		[Test]
		public void Subtract_2_Integers()
		{
			AssertScriptReturnValue(42, "return 63 - 21;");
		}

		[Test]
		public void Multiply_2_Integers()
		{
			AssertScriptReturnValue(42, "return 21 * 2;");
		}

		[Test]
		public void Divide_2_Integers()
		{
			AssertScriptReturnValue(42, "return 84 / 2;");
		}
	}
}
