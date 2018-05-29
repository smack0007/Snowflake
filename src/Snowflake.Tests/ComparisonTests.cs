using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.CodeGeneration;
using Snowflake.Parsing;
using Xunit;

namespace Snowflake.Tests
{
	public class ComparisonTests : LanguageTestFixture
	{
		[Fact]
		public void Two_Is_Greater_Than_One()
		{
			AssertScriptReturnValue(true, @"return 2 > 1;");
		}

		[Fact]
		public void Two_Is_Greater_Than_Or_Equal_To_One()
		{
			AssertScriptReturnValue(true, @"return 2 >= 1;");
		}

		[Fact]
		public void Two_Is_Not_Less_Than_One()
		{
			AssertScriptReturnValue(false, @"return 2 < 1;");
		}

		[Fact]
		public void Two_Is_Not_Less_Than_Or_Equal_To_One()
		{
			AssertScriptReturnValue(false, @"return 2 <= 1;");
		}

		[Fact]
		public void Comparing_Two_Strings_Is_Error()
		{
			AssertScriptIsException<CodeCompilationException>(@"return ""Hello"" > ""World"";");
		}

        [Fact]
        public void Consecutive_Compares_Is_Syntax_Error()
        {
            AssertScriptIsException<SyntaxException>(@"return 1 < 2 < 3;");
        }
	}
}
