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
				throw new SyntaxException(string.Format("{0} was expected at line {1} column {2}.", expected, lexemes[pos].Line, lexemes[pos].Column));
		}

		private void EnsureNotLexemeType(IList<Lexeme> lexemes, LexemeType unexpected, int pos)
		{
			if (lexemes[pos].Type == unexpected)
				throw new SyntaxException(string.Format("{0} was not expected at line {1} column {2}.", unexpected, lexemes[pos].Line, lexemes[pos].Column));
		}

		private void ThrowUnableToParseException(string parseType, IList<Lexeme> lexemes, int pos)
		{
			throw new SyntaxException(string.Format("Unable to parse lexeme type \"{0}\" as \"{1}\" at line {2} column {3}.", lexemes[pos].Type, parseType, lexemes[pos].Line, lexemes[pos].Column));
		}

		private bool IsAssignmentOperator(LexemeType type)
		{
			if (type == LexemeType.Gets || type == LexemeType.PlusGets || type == LexemeType.MinusGets || type == LexemeType.MultiplyGets || type == LexemeType.DivideGets)
				return true;

			return false;
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

				this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
				pos++;
			}
			else if (lexemes[pos].Type == LexemeType.Identifier)
			{
				if (this.IsAssignmentOperator(lexemes[pos + 1].Type))
				{
					node = this.ParseAssignment(lexemes, ref pos);
				}
				else
				{
					node = this.ParseFunctionCall(lexemes, ref pos);
				}

				this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
				pos++;
			}
			else if (lexemes[pos].Type == LexemeType.If)
			{
				node = this.ParseIf(lexemes, ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Return)
			{
				node = this.ParseReturn(lexemes, ref pos);
			}
			else
			{
				this.ThrowUnableToParseException("Statement", lexemes, pos);
			}
			
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

		private AssignmentNode ParseAssignment(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			AssignmentNode node = new AssignmentNode() { VariableName = lexemes[pos].Value };

			pos++;
			switch (lexemes[pos].Type)
			{
				case LexemeType.Gets:
					node.Operation = AssignmentOperation.Gets;
					break;

				case LexemeType.PlusGets:
					node.Operation = AssignmentOperation.AddGets;
					break;

				case LexemeType.MinusGets:
					node.Operation = AssignmentOperation.SubtractGets;
					break;

				case LexemeType.MultiplyGets:
					node.Operation = AssignmentOperation.MultiplyGets;
					break;

				case LexemeType.DivideGets:
					node.Operation = AssignmentOperation.DivideGets;
					break;

				default:
					throw new SyntaxException(string.Format("An assignment operation was expected at line {0} column {1}.", lexemes[pos].Line, lexemes[pos].Column));
			}

			pos++;
			node.ValueExpression = this.ParseExpression(lexemes, ref pos);

			return node;
		}

		private IfNode ParseIf(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.If, pos);

			IfNode node = new IfNode();

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			node.EvaluateExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			if (lexemes[pos].Type == LexemeType.Else)
			{
				pos++;
				node.ElseStatementBlock = this.ParseStatementBlock(lexemes, ref pos);
			}

			return node;
		}

		private ReturnNode ParseReturn(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Return, pos);

			ReturnNode node = new ReturnNode();

			pos++;
			node.Expression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
			pos++;

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

                case LexemeType.Undef:
                    node = new UndefinedValueNode();
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
						
			OperationType? operationType = this.ParseOperationType(lexemes, ref pos);

			if (operationType != null)
			{
				node = new OperationNode()
				{
					Type = operationType.Value,
					LHS = node,
					RHS = this.ParseExpression(lexemes, ref pos)
				};
			}

			return node;
		}

		private OperationType? ParseOperationType(IList<Lexeme> lexemes, ref int pos)
		{
			OperationType? operationType = null;

			switch (lexemes[pos].Type)
			{
                case LexemeType.EqualTo:
                    operationType = OperationType.Equals;
                    break;

				case LexemeType.NotEqualTo:
					operationType = OperationType.NotEquals;
					break;

				case LexemeType.Gets:
					operationType = OperationType.Gets;
					break;

				case LexemeType.Plus:
					operationType = OperationType.Add;
					break;

				case LexemeType.Minus:
					operationType = OperationType.Subtract;
					break;

				case LexemeType.LogicalAnd:
					operationType = OperationType.LogicalAnd;
					break;

				case LexemeType.LogicalOr:
					operationType = OperationType.LogicalOr;
					break;
			}

			if (operationType != null)
				pos++;

			return operationType;
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
				node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos));
												
				while (lexemes[pos].Type == LexemeType.Comma)
				{
                    pos++;
					node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

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
