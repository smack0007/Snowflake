using Xunit;

namespace Snowflake.Tests
{
    public class IntegerOperationTests : LanguageTestFixture
	{
		[Fact]
		public void Add_2_Integers()
		{
			AssertScriptReturnValue(42, "return 21 + 21;");
		}

		[Fact]
		public void Subtract_2_Integers()
		{
			AssertScriptReturnValue(42, "return 63 - 21;");
		}

		[Fact]
		public void Multiply_2_Integers()
		{
			AssertScriptReturnValue(42, "return 21 * 2;");
		}

		[Fact]
		public void Divide_2_Integers()
		{
			AssertScriptReturnValue(42, "return 84 / 2;");
		}
	}
}
