using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexing;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class Parser : IScriptParser
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

		private void ThrowUnableToParseException(string parseType, IList<Lexeme> lexemes, int pos)
		{
			throw new SyntaxException("Unable to parse lexeme type \"" + lexemes[pos].Type + "\" as \"" + parseType + "\" at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column + ".");
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
				node.Functions.Add(this.Func(lexemes, ref pos));
			}

			return node;
		}

		private FunctionNode Func(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Function, pos);

			FunctionNode node = new FunctionNode();

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			node.Name = lexemes[pos].Value;

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			if (lexemes[pos].Type == LexemeType.Variable)
			{
				pos++;
				this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
				node.Args.Add(lexemes[pos].Value);

				pos++;
				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					this.EnsureLexemeType(lexemes, LexemeType.Variable, pos);

					pos++;
					this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
					node.Args.Add(lexemes[pos].Value);

					pos++;
				}
			}
			
			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.StatementBlock = this.StatementBlock(lexemes, ref pos);
			
			return node;
		}

		private StatementBlockNode StatementBlock(IList<Lexeme> lexemes, ref int pos)
		{			
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			StatementBlockNode node = new StatementBlockNode();

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
				case LexemeType.Variable:
					node = this.Variable(lexemes, ref pos);
					break;

				case LexemeType.Null:
					node = new NullValueNode();
					pos++;
					break;

				case LexemeType.String:
					node = new StringValueNode() { Value = lexemes[pos].Value };
					pos++;
					break;

				case LexemeType.Char:
					node = new CharValueNode() { Value = lexemes[pos].Value[0] };
					pos++;
					break;

				case LexemeType.Integer:
					node = new IntegerValueNode() { Value = int.Parse(lexemes[pos].Value) };
					pos++;
					break;

				case LexemeType.Float:
					node = new FloatValueNode() { Value = float.Parse(lexemes[pos].Value) }; // TODO: Specify always use '.' for decimal place.
					pos++;
					break;
			}

			if (node == null)
				this.ThrowUnableToParseException("Expression", lexemes, pos);
						
			switch (lexemes[pos].Type)
			{
				case LexemeType.Gets:
					pos++;
					node = new OperationNode()
					{
						Type = OperationType.Gets,
						LHS = node,
						RHS = this.Expression(lexemes, ref pos)
					};
					break;

				case LexemeType.Plus:
					pos++;
					node = new OperationNode()
					{
						Type = OperationType.Add,
						LHS = node,
						RHS = this.Expression(lexemes, ref pos)
					};
					break;
			}

			return node;
		}

		private VariableNode Variable(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Variable, pos);
			
			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			VariableNode node = new VariableNode() { VariableName = lexemes[pos].Value };

			pos++;
			return node;
		}
	}
}
