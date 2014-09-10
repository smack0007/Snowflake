using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Tests
{
	public class ListTests : LanguageTestFixture
	{
		[Fact]
		public void Empty_List_Is_Correct()
		{
			AssertScriptReturnValue<List<dynamic>>(
				(x) =>
				{
					Assert.Equal(0, x.Count);
				},
				"return {};");
		}

		[Fact]
		public void List_With_One_Element_Is_Correct()
		{
			AssertScriptReturnValue<List<dynamic>>(
				(x) =>
				{
					Assert.Equal(1, x.Count);
					Assert.Equal(1, x[0]);
				},
				"return { 1 };");
		}

		[Fact]
		public void List_With_Two_Elements_Is_Correct()
		{
			AssertScriptReturnValue<List<dynamic>>(
				(x) =>
				{
					Assert.Equal(2, x.Count);
					Assert.Equal(1, x[0]);
					Assert.Equal(2, x[1]);
				},
				"return { 1, 2 };");
		}
	}
}
