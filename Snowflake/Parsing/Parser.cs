using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class Parser : IParser
	{
		private void EnsureLexemeType(IList<Lexeme> lexemes, LexemeType expected, int pos)
		{
			if (lexemes[pos].Type != expected)
				throw new SyntaxException(expected + " was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column + ".");
		}

		private void EnsureNotLexemeType(IList<Lexeme> lexemes, LexemeType unexpected, int pos)
		{
			if (lexemes[pos].Type == unexpected)
				throw new SyntaxException(unexpected + " was not expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column + ".");
		}

		/// <summary>
		/// Moves "pos" from OpenBrace to the matching CloseBrace.
		/// </summary>
		/// <param name="pos"></param>
		private void MoveToMatchingBrace(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			int level = 1;
			while (level > 0 && lexemes[pos].Type != LexemeType.EOF)
			{
				pos++;
				if (lexemes[pos].Type == LexemeType.OpenBrace)
				{
					level++;
				}
				else if (lexemes[pos].Type == LexemeType.CloseBrace)
				{
					level--;
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseBrace, pos);
		}

		public ScriptNode Parse(IList<Lexeme> lexemes)
		{
			if (lexemes == null)
				throw new ArgumentNullException("lexemes");

			ScriptNode node = new ScriptNode();

			int pos = 0;
			while (lexemes[pos].Type != LexemeType.EOF)
			{
				node.Funcs.Add(this.Func(lexemes, ref pos));
			}

			return node;
		}

		private FuncNode Func(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FuncNode node = new FuncNode();

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			node.Name = lexemes[pos].Val;

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			if (lexemes[pos].Type == LexemeType.Variable)
			{
				pos++;
				this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
				node.Args.Add(lexemes[pos].Val);

				pos++;
				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					this.EnsureLexemeType(lexemes, LexemeType.Variable, pos);

					pos++;
					this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
					node.Args.Add(lexemes[pos].Val);

					pos++;
				}
			}
			
			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseBrace &&
				   lexemes[pos].Type != LexemeType.EOF)
			{
				node.Statements.Add(this.Statement(lexemes, ref pos));
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseBrace, pos);

			pos++;
			return node;
		}

		private StatementNode Statement(IList<Lexeme> lexemes, ref int pos)
		{
			StatementNode node = null;

			if (lexemes[pos].Type == LexemeType.Echo)
			{
				node = this.Echo(lexemes, ref pos);
			}
			else
			{
				node = this.Expression(lexemes, ref pos);
			}

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;

			return node;
		}

		private EchoNode Echo(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Echo, pos);

			EchoNode node = new EchoNode();

			pos++;
			node.Expression = this.Expression(lexemes, ref pos);

			return node;
		}

		private ExpressionNode Expression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = null;

			switch (lexemes[pos].Type)
			{
				case LexemeType.String:
					node = new StringValueNode() { Value = lexemes[pos].Val };
					break;
			}

			pos++;

			return node;
		}
	}
}
