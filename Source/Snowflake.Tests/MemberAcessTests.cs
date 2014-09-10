using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Tests
{
	public class MemberAccessTests : LanguageTestFixture
	{
		[Fact]
		public void List_Count_Property_Can_Be_Accessed()
		{
			AssertScriptReturnValue(0, "return {}.Count;");
		}
	}
}
