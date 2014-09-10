using System;
using System.Collections.Generic;
using Snowflake.Lexing;
using System.Globalization;

namespace Snowflake.Parsing
{
	public class ScriptParser
	{
        private static T Construct<T>(IList<Lexeme> lexemes, int pos)
            where T : SyntaxNode, new()
        {
            return new T() { Line = lexemes[pos].Line, Column = lexemes[pos].Column };
        }

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

		public ScriptNode Parse(string id, IList<Lexeme> lexemes)
		{
            if (id == null)
                throw new ArgumentNullException("id");

			if (lexemes == null)
				throw new ArgumentNullException("lexemes");

			ScriptNode node = new ScriptNode(id);

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
				node = this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: true);

				this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
				pos++;
			}
			else if (lexemes[pos].Type == LexemeType.Func)
			{
				node = this.ParseFunctionDeclaration(lexemes, ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Identifier && pos < lexemes.Count - 1 && this.IsAssignmentOperator(lexemes[pos + 1].Type))
			{
				node = this.ParseAssignment(lexemes, ref pos);
				
				this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
				pos++;
			}
			else if (lexemes[pos].Type == LexemeType.If)
			{
				node = this.ParseIf(lexemes, ref pos);
			}
            else if (lexemes[pos].Type == LexemeType.While)
            {
                node = this.ParseWhile(lexemes, ref pos);
            }
			else if (lexemes[pos].Type == LexemeType.For)
			{
				node = this.ParseFor(lexemes, ref pos);
			}
			else if (lexemes[pos].Type == LexemeType.Return)
			{
				node = this.ParseReturn(lexemes, ref pos);
			}
			else
			{
				node = this.ParseExpression(lexemes, ref pos);

				this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
				pos++;
			}
			
			return node;
		}

		private StatementBlockNode ParseStatementBlock(IList<Lexeme> lexemes, ref int pos)
		{			
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			StatementBlockNode node = Construct<StatementBlockNode>(lexemes, pos);

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

		private VariableDeclarationNode ParseVariableDeclaration(IList<Lexeme> lexemes, ref int pos, bool varKeyword)
		{
			if (varKeyword)
			{
				this.EnsureLexemeType(lexemes, LexemeType.Var, pos);
				pos++;
			}

            VariableDeclarationNode node = Construct<VariableDeclarationNode>(lexemes, pos);

			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
            node.VariableName = lexemes[pos].Value;

			pos++;

			if (pos < lexemes.Count && lexemes[pos].Type == LexemeType.Gets)
			{
				pos++;
				node.ValueExpression = this.ParseExpression(lexemes, ref pos);
			}

			return node;
		}

		private FunctionNode ParseFunctionDeclaration(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FunctionNode node = Construct<FunctionNode>(lexemes, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
			node.FunctionName = lexemes[pos].Value;

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: false));

				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: false));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			return node;
		}

		private AssignmentNode ParseAssignment(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

			AssignmentNode node = Construct<AssignmentNode>(lexemes, pos);
            node.VariableName = lexemes[pos].Value;

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

			IfNode node = Construct<IfNode>(lexemes, pos);

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

        private WhileNode ParseWhile(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.While, pos);

            WhileNode node = Construct<WhileNode>(lexemes, pos);

            pos++;
            this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

            pos++;
            node.EvaluateExpression = this.ParseExpression(lexemes, ref pos);

            this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

            pos++;
            node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);
            
            return node;
        }

		private ForNode ParseFor(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.For, pos);

			ForNode node = Construct<ForNode>(lexemes, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			if (lexemes[pos].Type == LexemeType.Var)
			{
				node.InitializeStatement = this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: true);
			}
			else
			{
				node.InitializeStatement = this.ParseAssignment(lexemes, ref pos);
			}

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;
			node.EvaluateExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;
			node.IncrementStatement = this.ParseAssignment(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			return node;
		}

		private ReturnNode ParseReturn(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Return, pos);

			ReturnNode node = Construct<ReturnNode>(lexemes, pos);

			pos++;
			node.Expression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
			pos++;

			return node;
		}

		private ExpressionNode ParseExpression(IList<Lexeme> lexemes, ref int pos)
		{
			return this.ParseConditionalOrExpression(lexemes, ref pos);
		}

		private ExpressionNode ParseConditionalOrExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseConditionalAndExpression(lexemes, ref pos);

			while (lexemes[pos].Type == LexemeType.ConditionalOr)
			{
				pos++;

				OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = OperationType.ConditionalOr;
                opNode.LeftHand = node;
                opNode.RightHand = this.ParseConditionalAndExpression(lexemes, ref pos);

				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParseConditionalAndExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseEqualityExpression(lexemes, ref pos);

			while (lexemes[pos].Type == LexemeType.ConditionalAnd)
			{
				pos++;

                OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = OperationType.ConditionalAnd;
                opNode.LeftHand = node;
                opNode.RightHand = this.ParseEqualityExpression(lexemes, ref pos);
                
				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParseEqualityExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseRelationalExpression(lexemes, ref pos);

			while (lexemes[pos].Type == LexemeType.EqualTo || lexemes[pos].Type == LexemeType.NotEqualTo)
			{
				OperationType opType = lexemes[pos].Type == LexemeType.EqualTo ? OperationType.Equals : OperationType.NotEquals;
				pos++;

                OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = opType;
                opNode.LeftHand = node;
				opNode.RightHand = this.ParseRelationalExpression(lexemes, ref pos);
                
				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParseRelationalExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseAdditiveExpression(lexemes, ref pos);

			while (lexemes[pos].Type == LexemeType.GreaterThan || lexemes[pos].Type == LexemeType.GreaterThanOrEqualTo ||
				   lexemes[pos].Type == LexemeType.LessThan || lexemes[pos].Type == LexemeType.LessThanOrEqualTo)
			{
				OperationType opType = default(OperationType);
				
				switch (lexemes[pos].Type)
				{
					case LexemeType.GreaterThan:
						opType = OperationType.GreaterThan;
						break;

					case LexemeType.GreaterThanOrEqualTo:
						opType = OperationType.GreaterThanOrEqualTo;
						break;

					case LexemeType.LessThan:
						opType = OperationType.LessThan;
						break;

					case LexemeType.LessThanOrEqualTo:
						opType = OperationType.LessThanOrEqualTo;
						break;
				}
				
				pos++;

				OperationNode opNode = Construct<OperationNode>(lexemes, pos);
				opNode.Type = opType;
				opNode.LeftHand = node;
				opNode.RightHand = this.ParseAdditiveExpression(lexemes, ref pos);

				node = opNode;
			}

			return node;
		}

		private ExpressionNode ParseAdditiveExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseMultiplicativeExpression(lexemes, ref pos);

			while (lexemes[pos].Type == LexemeType.Plus || lexemes[pos].Type == LexemeType.Minus)
			{
				OperationType opType = lexemes[pos].Type == LexemeType.Plus ? OperationType.Add : OperationType.Subtract;
				pos++;

                OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = opType;
                opNode.LeftHand = node;
                opNode.RightHand = this.ParseMultiplicativeExpression(lexemes, ref pos);
                
				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParseMultiplicativeExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParsePrimaryExpression(lexemes, ref pos);

			if (lexemes[pos].Type == LexemeType.Multiply || lexemes[pos].Type == LexemeType.Divide)
			{
				OperationType opType = lexemes[pos].Type == LexemeType.Multiply ? OperationType.Multiply : OperationType.Divide;
				pos++;

                OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = opType;
                opNode.LeftHand = node;
                opNode.RightHand = this.ParsePrimaryExpression(lexemes, ref pos);

				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParsePrimaryExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = null;

			if (lexemes[pos].Type == LexemeType.OpenParen)
			{
				pos++;
				node = this.ParseExpression(lexemes, ref pos);

				this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);
				pos++;
			}
			else if (lexemes[pos].Type == LexemeType.Minus)
			{
				NegateOperationNode negateOp = Construct<NegateOperationNode>(lexemes, pos);
				
				pos++;
				negateOp.ValueExpression = this.ParseExpression(lexemes, ref pos);
				
				node = negateOp;
			}
			else
			{
				switch (lexemes[pos].Type)
				{
					case LexemeType.Func:
						node = this.ParseAnonymousFunction(lexemes, ref pos);
						break;

					case LexemeType.Identifier:
						node = this.ParseVariableReference(lexemes, ref pos);
						break;

					case LexemeType.Null:
						node = Construct<NullValueNode>(lexemes, pos);
						pos++;
						break;

					case LexemeType.Undef:
                        node = Construct<UndefinedValueNode>(lexemes, pos);
						pos++;
						break;

					case LexemeType.True:
                    case LexemeType.False:
                        var boolNode = Construct<BooleanValueNode>(lexemes, pos);
                        boolNode.Value = lexemes[pos].Type == LexemeType.True;
                        node = boolNode;
						pos++;
                        break;

					case LexemeType.String:
						var stringNode = Construct<StringValueNode>(lexemes, pos);
                        stringNode.Value = lexemes[pos].Value;
                        node = stringNode;
						pos++;
						break;

					case LexemeType.Char:
                        var charNode = Construct<CharacterValueNode>(lexemes, pos);
                        charNode.Value = lexemes[pos].Value[0];
                        node = charNode;
						pos++;
						break;

					case LexemeType.Integer:
                        var intNode = Construct<IntegerValueNode>(lexemes, pos);
                        intNode.Value = int.Parse(lexemes[pos].Value);
                        node = intNode;
						pos++;
						break;

					case LexemeType.Float:
                        var floatNode = Construct<FloatValueNode>(lexemes, pos);
						floatNode.Value = float.Parse(lexemes[pos].Value, CultureInfo.InvariantCulture);
                        node = floatNode;
                        pos++;
						break;
				}
			}

			if (node == null)
				this.ThrowUnableToParseException("PrimaryExpression", lexemes, pos);

            while (lexemes[pos].Type == LexemeType.OpenParen)
            {
                node = this.ParseFunctionCall(node, lexemes, ref pos);
            }

			return node;
		}

		private FunctionNode ParseAnonymousFunction(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FunctionNode node = Construct<FunctionNode>(lexemes, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: false));
												
				while (lexemes[pos].Type == LexemeType.Comma)
				{
                    pos++;
					node.Args.Add(this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: false));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			return node;
		}

		private FunctionCallNode ParseFunctionCall(ExpressionNode functionExpression, IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			FunctionCallNode node = Construct<FunctionCallNode>(lexemes, pos);
            node.FunctionExpression = functionExpression;
            
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

			VariableReferenceNode node = Construct<VariableReferenceNode>(lexemes, pos);
            node.VariableName = lexemes[pos].Value;

			pos++;
			return node;
		}
	}
}
