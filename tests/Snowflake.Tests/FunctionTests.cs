using Snowflake.Execution;
using Xunit;

namespace Snowflake.Tests
{
    public class FunctionTests : LanguageTestFixture
	{
        [Fact]
		public void Function_Equality_Implemented()
		{
			AssertScriptReturnValue(true, @"
const foo = func() {
	return 42;
};

const bar = foo;

return bar == foo;
");
		}

        [Fact]
		public void Function_Inequality_Implemented()
		{
			AssertScriptReturnValue(true, @"
const foo = func() {
	return 42;
};

const bar = func() {
    return 21;
};

return bar != foo;
");
		}

        [Fact]
		public void Functions_Declared_With_Same_Body_Are_Not_Equal()
		{
			AssertScriptReturnValue(false, @"
const foo = func() {
	return 42;
};

const bar = func() {
    return 42;
};

return bar == foo;
");
		}
	}
}
