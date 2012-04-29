using System;
using NUnit.Framework;
using Snowsoft.SnowflakeScript;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowflake.Tests
{
	/// <summary>
	/// Contains tests which should cause ScriptParserException(s).
	/// </summary>
	[TestFixture]
	public class ScriptParserErrorTests
	{
		private void ParseScript(string script)
		{
			ScriptLexer parser = new ScriptLexer();
			parser.Lex(script);
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void Char_WithNoEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'aa'");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void Char_WithEscapeCodeMoreThanOneChar()
		{
			this.ParseScript("'\\aa'");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void UnexpectedEndOfFile_UnclosedChar()
		{
			this.ParseScript("\'a");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void UnexpectedEndOfFile_UnclosedString()
		{
			this.ParseScript("\"aa");
		}

		[Test, ExpectedException(typeof(LexerException))]
		public void UnexpectedEndOfFile_FloatEndsWithDecimal()
		{
			this.ParseScript("123.");
		}
	}
}
