using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Lexing;
using Xunit;

namespace Snowflake.Tests.Lexing
{
    public class CommentsTests : LexerTestFixture
    {
        [Fact]
        public void Single_Line_Comment_At_Start()
        {
            AssertLexemes(@"
// Starts with a comment
x = 10;",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }

        [Fact]
        public void Single_Line_Comment_At_End()
        {
            AssertLexemes(@"
x = 10;
// Ends with a comment",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }

        [Fact]
        public void Single_Line_Comment_After_Statement()
        {
            AssertLexemes(@"
x = 10; // After statement",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }

        [Fact]
        public void Single_Line_Comment_Before_Statement()
        {
            AssertLexemes(@"
// Before statement x = 10;",
                LexemeType.EOF);
        }

        [Fact]
        public void Multi_Line_Comment()
        {
            AssertLexemes(@"
/* This
is
a
multi line
comment. */
x = 10;",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }

        [Fact]
        public void Multi_Line_Comment_On_Single_Line()
        {
            AssertLexemes(@"
/* Starts with a comment */
x = 10;",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }

        [Fact]
        public void Multi_Line_Comment_With_No_Content()
        {
            AssertLexemes(@"
/**/x = 10;/**/",
                LexemeType.Identifier,
                LexemeType.Gets,
                LexemeType.Integer,
                LexemeType.EndStatement,
                LexemeType.EOF);
        }
    }
}
