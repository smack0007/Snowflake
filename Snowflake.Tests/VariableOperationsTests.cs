using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	[TestFixture]
	public class VariableOperationsTests : LanguageTestFixture
	{
		[Test]
		public void Add_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(12, @"
var x = 4;
var y = 8;
return x + y;");
		}

		[Test]
		public void Add_Int_And_Int_Variable()
		{
			AssertScriptReturnValue(8, @"
var x = 4;
return 4 + x;");
		}

		[Test]
		public void Subtract_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(4, @"
var x = 8;
var y = 4;
return x - y;");
		}
	}
}
