﻿using Xunit;

namespace Snowflake.Tests
{
    public class MemberAccessTests : LanguageTestFixture
	{
		[Fact]
		public void List_Count_Property_Can_Be_Accessed()
		{
			AssertScriptReturnValue(0, "return [].Count;");
		}

        [Fact]
        public void List_Clear_Method_Can_Be_Called()
        {
            AssertScriptReturnValue(0, "var x = [1, 2, 3]; x.Clear(); return x.Count;");
        }

        [Fact]
        public void List_Count_ToString_Method_Can_Be_Called()
        {
            AssertScriptReturnValue("3", "var x = [1, 2, 3]; return x.Count.ToString();");
        }
	}
}
