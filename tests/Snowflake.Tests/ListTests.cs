using Snowflake.Execution;
using Xunit;

namespace Snowflake.Tests
{
    public class ListTests : LanguageTestFixture
	{
		[Fact]
		public void Empty_List_Is_Correct()
		{
			AssertScriptReturnValue<ScriptList>(
				(x) =>
				{
					Assert.Empty(x);
				},
				"return [];");
		}

		[Fact]
		public void List_With_One_Element_Is_Correct()
		{
			AssertScriptReturnValue<ScriptList>(
				(x) =>
				{
					Assert.Single(x);
					Assert.Equal(1, x[0]);
				},
				"return [ 1 ];");
		}

		[Fact]
		public void List_With_Two_Elements_Is_Correct()
		{
			AssertScriptReturnValue<ScriptList>(
				(x) =>
				{
					Assert.Equal(2, x.Count);
					Assert.Equal(1, x[0]);
					Assert.Equal(2, x[1]);
				},
				"return [ 1, 2 ];");
		}

		[Fact]
		public void List_With_List_Elements_Is_Correct()
		{
			AssertScriptReturnValue<ScriptList>(
				(x) =>
				{
					Assert.Equal(2, x.Count);
					Assert.Equal(2, x.ElementAt<ScriptList>(0).Count);
					Assert.Equal(1, x.ElementAt<ScriptList>(0)[0]);
					Assert.Equal(2, x.ElementAt<ScriptList>(0)[1]);
					Assert.Equal(2, x.ElementAt<ScriptList>(1).Count);
					Assert.Equal(3, x.ElementAt<ScriptList>(1)[0]);
					Assert.Equal(4, x.ElementAt<ScriptList>(1)[1]);
				},
				"return [ [1, 2], [3, 4] ];");
		}

		[Fact]
		public void List_With_Array_Elements_Is_Correct()
		{
			AssertScriptReturnValue<ScriptList>(
				(x) =>
				{
					Assert.Equal(2, x.Count);
					Assert.Equal(2, ((object[])x[0]).Length);
					Assert.Equal(1, ((object[])x[0])[0]);
					Assert.Equal(2, ((object[])x[0])[1]);
					Assert.Equal(2, ((object[])x[1]).Length);
					Assert.Equal(3, ((object[])x[1])[0]);
					Assert.Equal(4, ((object[])x[1])[1]);
				},
				"return [ [| 1, 2 |], [| 3, 4 |] ];");
		}

		[Fact]
		public void List_Element_Access_Is_Correct()
		{
			AssertScriptReturnValue(
				1,
				@"
var foo = [ 3, 1, 2 ];
return foo[1];");
		}

		[Fact]
		public void Func_Can_Be_List_Element()
		{
			AssertScriptReturnValue(
				42,
				@"
var foo = [ func() { return 0; }, func() { return 42; } ];
return foo[1]();");
		}
	}
}
