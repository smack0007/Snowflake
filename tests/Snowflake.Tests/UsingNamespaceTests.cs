using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
    public class UsingNamespaceTests : LanguageTestFixture
    {
        [Fact]
        public void Using_Namespace_Can_Resolve_Function_Call()
        {
            AssertScriptReturnValue(
                (engine) =>
                {
                    engine.SetGlobalFunction("MyNamespace.get42", () => 42);
                },
                42,
                @"
using MyNamespace;
return get42();");
        }

        [Fact]
        public void Using_Namespace_Can_Resolve_Constructor_Call()
        {
            AssertScriptReturnValue<Person>(
                (engine) =>
                {
                    engine.RegisterType("MyNamespace.Person", typeof(Person));
                },
                (x) =>
                {
                    Assert.Equal("Bob", x.FirstName);
                    Assert.Equal("Freeman", x.LastName);
                },
                @"
using MyNamespace;
return new Person(""Bob"", ""Freeman"");");
        }
    }
}
