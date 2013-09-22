using System;
using System.Collections.Generic;
using Snowsoft.SnowflakeScript.Lexing;
using System.Globalization;

namespace Snowsoft.SnowflakeScript.Parsing
{
	public class ScriptParser
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
				
		public ScriptNode Parse(IList<Lexeme> lexemes)
		{
			if (lexemes == null)
				throw new ArgumentNullException("lexemes");

			ScriptNode node = new ScriptNode();

			int pos = 0;
			while (lexemes[pos].Type != LexemeType.EOF)
			{
				node.Statements.Add(this.ParseStatement(lexemes, ref pos));
			}

			return node;
		}

		private StatementNode ParseStatement(IList<Lexeme> lexemes, ref int pos)
		{
			StatementNode node = null;

			if (lexemes[pos].Type == LexemeType.Var)
			{
				node = this.ParseVariableDeclaration(lexemes, ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Return)
			{
				node = this.ParseReturn(lexemes, ref pos);
			}
			else
			{
				node = this.ParseExpression(lexemes, ref pos);
			}

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;

			return node;
		}

		private StatementBlockNode ParseStatementBlock(IList<Lexeme> lexemes, ref int pos)
		{			
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			StatementBlockNode node = new StatementBlockNode();

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseBrace &&
				   lexemes[pos].Type != LexemeType.EOF)
			{
				node.Statements.Add(this.ParseStatement(lexemes, ref pos));
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseBrace, pos);

			pos++;
			return node;
		}

		private VariableDeclarationNode ParseVariableDeclaration(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Var, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			VariableDeclarationNode node = new VariableDeclarationNode() { VariableName = lexemes[pos].Value };
			pos++;

			if (pos < lexemes.Count && lexemes[pos].Type == LexemeType.Gets)
			{
				pos++;
				node.ValueExpression = this.ParseExpression(lexemes, ref pos);
			}

			return node;
		}
		
		private ReturnNode ParseReturn(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Return, pos);

			ReturnNode node = new ReturnNode();

			pos++;
			node.Expression = this.ParseExpression(lexemes, ref pos);

			return node;
		}

		private ExpressionNode ParseExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = null;

			switch (lexemes[pos].Type)
			{
				case LexemeType.Func:
					node = this.ParseFunction(lexemes, ref pos);
					break;

				case LexemeType.Identifier:
					if (pos + 1 < lexemes.Count && lexemes[pos + 1].Type == LexemeType.OpenParen)
					{
						node = this.ParseFunctionCall(lexemes, ref pos);
					}
					else
					{
						node = this.ParseVariableReference(lexemes, ref pos);
					}
					break;

				case LexemeType.Null:
					node = new NullValueNode();
					pos++;
					break;

				case LexemeType.True:
					node = new BooleanValueNode() { Value = true };
					pos++;
					break;

				case LexemeType.False:
					node = new BooleanValueNode() { Value = false };
					pos++;
					break;

				case LexemeType.String:
					node = new StringValueNode() { Value = lexemes[pos].Value };
					pos++;
					break;

				case LexemeType.Char:
					node = new CharacterValueNode() { Value = lexemes[pos].Value[0] };
					pos++;
					break;

				case LexemeType.Integer:
					node = new IntegerValueNode() { Value = int.Parse(lexemes[pos].Value) };
					pos++;
					break;

				case LexemeType.Float:
					node = new FloatValueNode() { Value = float.Parse(lexemes[pos].Value, CultureInfo.InvariantCulture) };
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
						RHS = this.ParseExpression(lexemes, ref pos)
					};
					break;

				case LexemeType.Plus:
					pos++;
					node = new OperationNode()
					{
						Type = OperationType.Add,
						LHS = node,
						RHS = this.ParseExpression(lexemes, ref pos)
					};
					break;

				case LexemeType.Minus:
					pos++;
					node = new OperationNode()
					{
						Type = OperationType.Subtract,
						LHS = node,
						RHS = this.ParseExpression(lexemes, ref pos)
					};
					break;
			}

			return node;
		}

		private FunctionNode ParseFunction(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FunctionNode node = new FunctionNode();

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
				node.Args.Add(lexemes[pos].Value);

				pos++;
				while (lexemes[pos].Type == LexemeType.Comma)
				{					
					pos++;
					this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
					node.Args.Add(lexemes[pos].Value);

					pos++;
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.StatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			return node;
		}

		private FunctionCallNode ParseFunctionCall(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			FunctionCallNode node = new FunctionCallNode() { FunctionName = lexemes[pos].Value };

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				node.Args.Add(this.ParseExpression(lexemes, ref pos));
								
				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					node.Args.Add(this.ParseExpression(lexemes, ref pos));
				}
			}
						
			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			return node;
		}

		private VariableReferenceNode ParseVariableReference(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			VariableReferenceNode node = new VariableReferenceNode() { VariableName = lexemes[pos].Value };

			pos++;
			return node;
		}
	}
}
