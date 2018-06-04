using Snowflake.Lexing;
using Xunit;

namespace Snowflake.Tests.Lexing
{
    public class ScriptLexerTests
	{
		private void ParseScript(string script)
		{
			ScriptLexer parser = new ScriptLexer();
			parser.Lex(script);
		}

		[Fact]
		public void More_Than_One_Char_No_Escape_Code_Is_Error()
		{
			Assert.Throws<LexerException>(() =>
			{
				this.ParseScript("'aa'");
			});
		}

		[Fact]
		public void More_Than_One_Char_With_Escape_Code_Is_Error()
		{
			Assert.Throws<LexerException>(() =>
			{
				this.ParseScript("'\\aa'");
			});
		}

		[Fact]
		public void Unclosed_Char_Is_Error()
		{
			Assert.Throws<LexerException>(() =>
			{
				this.ParseScript("\'a");
			});
		}

		[Fact]
		public void Unclosed_String_Is_Error()
		{
			Assert.Throws<LexerException>(() =>
			{
				this.ParseScript("\"aa");
			});
		}

		[Fact]
		public void Float_Ending_With_Decimal_Is_Error()
		{
			Assert.Throws<LexerException>(() =>
			{
				this.ParseScript("123.");
			});
		}
	}
}
