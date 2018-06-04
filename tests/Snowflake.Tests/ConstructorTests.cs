using Snowflake.Execution;
using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
    public class ConstructorTests : LanguageTestFixture
    {
        [Fact]
        public void Unknown_Constructor_Call_Is_Error()
        {
            AssertScriptIsException<ScriptExecutionException>("return new Person();");
        }

        [Fact]
        public void Known_Parameterless_Constructor_Can_Be_Called()
        {
            AssertScriptReturnValue<Person>(
                (engine) =>
                {
                    engine.RegisterType("Person", typeof(Person));
                },
                (x) =>
                {
                    Assert.IsType<Person>(x);
                },
                "return new Person();");
        }

        [Fact]
        public void Known_Constructor_With_Parameters_Can_Be_Called()
        {
            AssertScriptReturnValue<Person>(
                (engine) =>
                {
                    engine.RegisterType("Person", typeof(Person));
                },
                (x) =>
                {
                    Assert.IsType<Person>(x);
                    Assert.Equal("Bob", x.FirstName);
                    Assert.Equal("Freeman", x.LastName);
                },
                "return new Person(\"Bob\", \"Freeman\");");
        }
    }
}
