using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Lexing;
using Xunit;

namespace Snowflake.Tests.Lexing
{
    public abstract class LexerTestFixture
    {
        protected void AssertLexemes(string script, params LexemeType[] expectedLexemes)
        {
            ScriptLexer lexer = new ScriptLexer();
            var actualLexemes = lexer.Lex(script);

            foreach (var lexeme in actualLexemes)
                Console.WriteLine("{0}", lexeme.Type);

            Assert.Equal(expectedLexemes.Length, actualLexemes.Count);

            for (int i = 0; i < actualLexemes.Count; i++)
            {
                Assert.Equal(expectedLexemes[i], actualLexemes[i].Type);
            }
        }
    }
}
