using Snowflake.Execution;
using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
    public class NamespaceTests : LanguageTestFixture
	{
		[Fact]
		public void Static_Class_Can_Be_Made_Available_Via_Namespace()
		{
            AssertScriptReturnValue(
                (engine) =>
                {
                    engine.RegisterType("Scripting.StaticClass", typeof(StaticClass));
                },
                42,
                "return Scripting.StaticClass.Get42();");
        }

        [Fact]
        public void Namespace_Cannot_Override_Existing_Variable()
        {
            ScriptEngine engine = new ScriptEngine();
            engine.SetGlobalVariable("Scripting.Foo", 42);
            Assert.Throws<ScriptExecutionException>(() => engine.RegisterType("Scripting.Foo.StaticClass", typeof(StaticClass)));
        }
    }
}
