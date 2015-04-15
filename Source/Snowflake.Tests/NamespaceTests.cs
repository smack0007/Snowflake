using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                42,
                (engine) =>
                {
                    engine.SetGlobalStaticObject("Scripting.StaticClass", typeof(StaticClass));
                },
                "return Scripting.StaticClass.Get42();");
        }

        [Fact]
        public void Namespace_Cannot_Override_Existing_Variable()
        {
            ScriptEngine engine = new ScriptEngine();
            engine.SetGlobalVariable("Scripting.Foo", 42);
            Assert.Throws<ScriptExecutionException>(() => engine.SetGlobalStaticObject("Scripting.Foo.StaticClass", typeof(StaticClass)));
        }
    }
}
