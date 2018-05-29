using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Tests
{
	public class FloatingPointTests : LanguageTestFixture
	{
		[Fact]
		public void Float_Type_Is_Correctly_Assigned()
		{
			AssertScriptReturnValue<float>(
				(x) =>
				{
					Assert.IsType<float>(x);
				},
				"return 12.5f;");
		}

		[Fact]
		public void Double_Type_Is_Correctly_Used()
		{
			AssertScriptReturnValue<double>(
				(x) =>
				{
					Assert.IsType<double>(x);
				},
				"return 12.5;");
		}
	}
}
