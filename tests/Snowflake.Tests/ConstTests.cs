using Snowflake.Execution;
using Xunit;

namespace Snowflake.Tests
{
    public class ConstTests : LanguageTestFixture
    {
        [Fact]
        public void Const_Variable_Cannot_Be_Modified()
        {
            AssertScriptIsException<ScriptExecutionException>(@"
const x = 5;
x = 10;");
        }

        [Fact]
        public void Const_Variable_Can_Be_Returned()
        {
            AssertScriptReturnValue(5, @"
const x = 5;
return x;");
        }

        [Fact]
        public void Const_Variable_And_Normal_Variable_Cannot_Have_Same_Name()
        {
            AssertScriptIsException<ScriptExecutionException>(@"
const x = 5;
var x = 10;");
        }

        [Fact]
        public void Variable_Inside_Function_Can_Have_Same_Name_As_Const()
        {
            AssertScriptReturnValue(10, @"
const x = 5;
const foo = func() {
    var x = 10;
    return x;
};
return foo();");
        }

        [Fact]
        public void Const_Variable_Value_Can_Be_Expression()
        {
            AssertScriptReturnValue(10, @"
var x = 5;
const y = x * 2;
return y;");
        }
    }
}
