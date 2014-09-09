using System;
using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowflake.Tests.Lexing
{
	[TestFixture]
	public class ScriptLexerTests
	{
		private void ParseScript(string script)
		{
			ScriptLexer parser = new ScriptLexer();
			parser.Lex(script);
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void More_Than_One_Char_No_Escape_Code_Is_Error()
		{
			this.ParseScript("'aa'");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void More_Than_One_Char_With_Escape_Code_Is_Error()
		{
			this.ParseScript("'\\aa'");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void Unclosed_Char_Is_Error()
		{
			this.ParseScript("\'a");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void Unclosed_String_Is_Error()
		{
			this.ParseScript("\"aa");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void Float_Ending_With_Decimal_Is_Error()
		{
			this.ParseScript("123.");
		}
	}
}
