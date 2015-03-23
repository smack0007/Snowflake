using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Tests
{
    public class StackTests : LanguageTestFixture
    {
        [Fact]
        public void Depth_Of_One_Stack_Trace()
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
                Assert.Equal(1, ex.ScriptStack.Length);
                Assert.Equal("isException", ex.ScriptStack[0].FunctionName);
            });
        }

        [Fact]
        public void Depth_Of_Two_Stack_Trace()
        {
            AssertScriptIsException<ScriptExecutionException>(@"
func isException() {
    isException2();
};

func isException2() {
    foo();
};

isException();
",
            (x) =>
            {
                var ex = (ScriptExecutionException)x;
                Assert.Equal(2, ex.ScriptStack.Length);
                Assert.Equal("isException", ex.ScriptStack[0].FunctionName);
                Assert.Equal("isException2", ex.ScriptStack[1].FunctionName);
            });
        }
    }
}
