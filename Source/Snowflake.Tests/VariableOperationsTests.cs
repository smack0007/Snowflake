using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Tests
{
	public class VariableOperationsTests : LanguageTestFixture
	{
		[Fact]
		public void Add_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(12, @"
var x = 4;
var y = 8;
return x + y;");
		}

		[Fact]
		public void Add_Int_And_Int_Variable()
		{
			AssertScriptReturnValue(8, @"
var x = 4;
return 4 + x;");
		}

		[Fact]
		public void Subtract_Int_Variable_And_Int_Variable()
		{
			AssertScriptReturnValue(4, @"
var x = 8;
var y = 4;
return x - y;");
		}
	}
}
