using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tests.Types;
using Xunit;

namespace Snowflake.Tests
{
    public class TypeTests : LanguageTestFixture
    {
        [Fact]
        public void Type_Can_Be_Stored_InVariable()
        {
            AssertScriptReturnValue<Person>(
                (engine) =>
                {
                    engine.RegisterType("Person", typeof(Person));
                },
                (x) =>
                {
                    Assert.Equal("Bob", x.FirstName);
                    Assert.Equal("Freeman", x.LastName);
                },
                @"
var personType = Person;
return new personType(""Bob"", ""Freeman"");");
        }
    }
}
