using Xunit;

namespace Snowflake.Tests
{
    public class StringOperationsTests : LanguageTestFixture
	{
		[Fact]
		public void Add_String_And_String()
		{
			AssertScriptReturnValue("Hello World!", "return \"Hello\" + \" World!\";");
		}

		[Fact]
		public void Add_String_And_Character()
		{
			AssertScriptReturnValue("HelloC", "return \"Hello\" + 'C';");
		}

		[Fact]
		public void Add_String_And_Bool()
		{
			string trueString = true.ToString();
			AssertScriptReturnValue("Hello" + trueString, "return \"Hello\" + true;");
		}

		[Fact]
		public void Add_String_And_Int()
		{
			AssertScriptReturnValue("Hello42", "return \"Hello\" + 42;");
		}

		[Fact]
		public void Add_String_And_Float()
		{
			string floatString = (1.1f).ToString();
			AssertScriptReturnValue("Hello" + floatString, "return \"Hello\" + 1.1;");
		}
	}
}
