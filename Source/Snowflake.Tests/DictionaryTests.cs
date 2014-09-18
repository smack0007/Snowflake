using System;
using System.Collections.Generic;
using Xunit;

namespace Snowflake.Tests
{
	public class DictionaryTests : LanguageTestFixture
	{
		[Fact]
		public void Empty_Dictionary_Is_Correct()
		{
			AssertScriptReturnValue<ScriptDictionary>(
				(x) =>
				{
					Assert.Equal(0, x.Count);
				},
				"return {};");
		}

		[Fact]
		public void Dictionary_With_One_Element_Is_Correct()
		{
			AssertScriptReturnValue<ScriptDictionary>(
				(x) =>
				{
					Assert.Equal(1, x.Count);
					Assert.Equal(1, x["foo"]);
				},
				"return { foo: 1 };");
		}
                
        [Fact]
        public void Dictionary_With_Two_Elements_Is_Correct()
        {
            AssertScriptReturnValue<ScriptDictionary>(
                (x) =>
                {
                    Assert.Equal(2, x.Count);
                    Assert.Equal(1, x["foo"]);
                    Assert.Equal(2, x["bar"]);
                },
                "return { foo: 1, bar: 2 };");
        }

        [Fact]
        public void Dictionary_With_String_Keys_Is_Correct()
        {
            AssertScriptReturnValue<ScriptDictionary>(
                (x) =>
                {
                    Assert.Equal(2, x.Count);
                    Assert.Equal(1, x["foo"]);
                    Assert.Equal(2, x["bar"]);
                },
                "return { \"foo\": 1, \"bar\": 2 };");
        }

        [Fact]
        public void Dictionary_Can_Contain_Dictionary()
        {
            AssertScriptReturnValue<ScriptDictionary>(
                (x) =>
                {
                    dynamic obj = x;
                    Assert.Equal(1, obj.Count);
                    Assert.IsType<ScriptDictionary>(obj.foo);
                    Assert.Equal(42, obj.foo.bar);
                },
                "return { foo: { bar: 42 } };");
        }

        [Fact]
        public void Dictionary_Can_Contain_Dictionary_Which_Contains_Dictionary()
        {
            AssertScriptReturnValue<ScriptDictionary>(
                (x) =>
                {
                    dynamic obj = x;
                    Assert.Equal(1, obj.Count);
                    Assert.IsType<ScriptDictionary>(obj.foo);
                    Assert.IsType<ScriptDictionary>(obj.foo.bar);
                    Assert.Equal(42, obj.foo.bar.baz);
                },
                "return { foo: { bar: { baz: 42 } } };");
        }
	}
}
