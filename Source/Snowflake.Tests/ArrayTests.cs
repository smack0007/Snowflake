﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Snowflake.Tests
{
	public class ArrayTests : LanguageTestFixture
	{
		[Fact]
		public void Empty_Array_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(0, x.Length);
				},
				"return [||];");
		}

		[Fact]
		public void Array_With_One_Element_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(1, x.Length);
					Assert.Equal(1, x[0]);
				},
				"return [| 1 |];");
		}

		[Fact]
		public void Array_With_String_Element_Containing_Pipe_Character_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(1, x.Length);
					Assert.Equal("|", x[0]);
				},
				"return [| \"|\" |];");
		}

		[Fact]
		public void Array_With_Two_Elements_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(2, x.Length);
					Assert.Equal(1, x[0]);
					Assert.Equal(2, x[1]);
				},
				"return [| 1, 2 |];");
		}

		[Fact]
		public void Array_With_Array_Elements_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(2, x.Length);
					Assert.Equal(2, x[0].Length);
					Assert.Equal(1, x[0][0]);
					Assert.Equal(2, x[0][1]);
					Assert.Equal(2, x[1].Length);
					Assert.Equal(3, x[1][0]);
					Assert.Equal(4, x[1][1]);
				},
				"return [| [| 1, 2 |], [| 3, 4 |] |];");
		}

		[Fact]
		public void Array_With_List_Elements_Is_Correct()
		{
			AssertScriptReturnValue<dynamic[]>(
				(x) =>
				{
					Assert.Equal(2, x.Length);
					Assert.Equal(2, x[0].Count);
					Assert.Equal(1, x[0][0]);
					Assert.Equal(2, x[0][1]);
					Assert.Equal(2, x[1].Count);
					Assert.Equal(3, x[1][0]);
					Assert.Equal(4, x[1][1]);
				},
				"return [| [ 1, 2 ], [ 3, 4 ] |];");
		}

		[Fact]
		public void Array_Element_Access_Is_Correct()
		{
			AssertScriptReturnValue(
				1,
				@"
var foo = [| 3, 1, 2 |];
return foo[1];");
		}
	}
}
