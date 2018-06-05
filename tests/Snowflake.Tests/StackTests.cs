using Snowflake.Execution;
using Xunit;

namespace Snowflake.Tests
{
    public class StackTests : LanguageTestFixture
    {
        [Fact]
        public void Depth_Of_One_Stack_Trace()
        {
            AssertScriptIsException<ScriptExecutionException>(@"
foo();
",
            (x) =>
            {
                var ex = (ScriptExecutionException)x;
                Assert.Single(ex.ScriptStack);
                Assert.Equal("<script>", ex.ScriptStack[0].Name);
            });
        }

        [Fact]
        public void Depth_Of_Two_Stack_Trace()
        {
            AssertScriptIsException<ScriptExecutionException>(@"
func isException() {
    foo();
};

isException();
",
            (x) =>
            {
                var ex = (ScriptExecutionException)x;
                Assert.Equal(2, ex.ScriptStack.Length);
                Assert.Equal("<script>", ex.ScriptStack[0].Name);
                Assert.Equal("isException", ex.ScriptStack[1].Name);
            });
        }
    }
}
