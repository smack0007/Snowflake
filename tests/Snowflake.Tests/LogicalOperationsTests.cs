using Xunit;

namespace Snowflake.Tests
{
    public class LogicalOperationsTests : LanguageTestFixture
	{
		[Fact]
		public void True_Equal_To_True()
		{
			AssertScriptReturnValue(true, "return true == true;");
		}

		[Fact]
		public void True_Not_Equal_To_False()
		{
			AssertScriptReturnValue(true, "return true != false;");
		}

		[Fact]
		public void True_And_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true && true;");
		}

		[Fact]
		public void True_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return true && false;");
		}

		[Fact]
		public void False_And_True_Is_False()
		{
			AssertScriptReturnValue(false, "return false && true;");
		}

		[Fact]
		public void False_And_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false && false;");
		}

		[Fact]
		public void True_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return true || true;");
		}

		[Fact]
		public void True_Or_False_Is_True()
		{
			AssertScriptReturnValue(true, "return true || false;");
		}

		[Fact]
		public void False_Or_True_Is_True()
		{
			AssertScriptReturnValue(true, "return false || true;");
		}

		[Fact]
		public void False_Or_False_Is_False()
		{
			AssertScriptReturnValue(false, "return false || false;");
		}

		[Fact]
		public void Variable_True_And_Variable_True_Is_True()
		{
			AssertScriptReturnValue(true, "var x = true; var y = true; return x && y;");
		}

		[Fact]
		public void Not_True_Equal_To_False()
		{
			AssertScriptReturnValue(false, "return !true;");
		}

        [Fact]
        public void And_Shortcircuts()
        {
            AssertScriptReturnValue(1, @"
var result = 0;

const foo = func() {
    result = 1;
    return false;
};

const bar = func() {
    result = 2;
    return true;
};

const test = foo() && bar();
return result;
");
        }

        [Fact]
        public void Or_Shortcircuts()
        {
            AssertScriptReturnValue(1, @"
var result = 0;

const foo = func() {
    result = 1;
    return true;
};

const bar = func() {
    result = 2;
    return false;
};

const test = foo() || bar();
return result;
");
        }
	}
}
