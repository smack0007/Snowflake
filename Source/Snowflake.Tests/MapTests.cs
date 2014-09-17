using System;
using System.Collections.Generic;
using Xunit;

namespace Snowflake.Tests
{
	public class MapTests : LanguageTestFixture
	{
		[Fact]
		public void Empty_Map_Is_Correct()
		{
			AssertScriptReturnValue<Dictionary<dynamic, dynamic>>(
				(x) =>
				{
					Assert.Equal(0, x.Count);
				},
				"return {};");
		}

		[Fact]
		public void Map_With_One_Element_Is_Correct()
		{
			AssertScriptReturnValue<Dictionary<dynamic, dynamic>>(
				(x) =>
				{
					Assert.Equal(1, x.Count);
					Assert.Equal(1, x["foo"]);
				},
				"return { foo: 1 };");
		}
                
        [Fact]
        public void Map_With_Two_Elements_Is_Correct()
        {
            AssertScriptReturnValue<Dictionary<dynamic, dynamic>>(
                (x) =>
                {
                    Assert.Equal(2, x.Count);
                    Assert.Equal(1, x["foo"]);
                    Assert.Equal(2, x["bar"]);
                },
                "return { foo: 1, bar: 2 };");
        }

        [Fact]
        public void Map_With_String_Keys_Is_Correct()
        {
            AssertScriptReturnValue<Dictionary<dynamic, dynamic>>(
                (x) =>
                {
                    Assert.Equal(2, x.Count);
                    Assert.Equal(1, x["foo"]);
                    Assert.Equal(2, x["bar"]);
                },
                "return { \"foo\": 1, \"bar\": 2 };");
        }
	}
}
