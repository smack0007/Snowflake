using System;
using System.Collections.Generic;
using Snowflake.Lexing;
using System.Globalization;
using System.Text;

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

		private static bool IsAssignmentOperator(LexemeType type)
		{
			if (type == LexemeType.Gets || type == LexemeType.PlusGets || type == LexemeType.MinusGets || type == LexemeType.MultiplyGets || type == LexemeType.DivideGets)
				return true;

			return false;
		}

		private static bool IsPostfixOperator(LexemeType type)
		{
			if (type == LexemeType.Increment || type == LexemeType.Decrement)
				return true;

			return false;
		}

		private static bool IsUnaryOperator(LexemeType type)
		{
			if (type == LexemeType.Minus || type == LexemeType.Not || type == LexemeType.Increment || type == LexemeType.Decrement)
				return true;

			return false;
		}

		private static void SkipWhileEndStatement(IList<Lexeme> lexemes, ref int pos)
		{
			while (lexemes[pos].Type == LexemeType.EndStatement)
				pos++;
        }

        private static int LookAheadUntilEndStatement(IList<Lexeme> lexemes, int pos, LexemeType searchFor)
        {
            for (int i = pos; i < lexemes.Count; i++)
            {
                if (lexemes[i].Type == searchFor)
                    return pos;

                if (lexemes[i].Type == LexemeType.EndStatement)
                    return -1;
            }

            return -1;
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

            switch (lexemes[pos].Type)
            {
                case LexemeType.Using:
                    {
                        node = this.ParseUsingDeclaration(lexemes, ref pos);

                        this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
                        pos++;
                    }
                    break;

                case LexemeType.Const:
				    {
					    node = this.ParseConstDeclaration(lexemes, ref pos);

					    this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
					    pos++;
				    }
				    break;

			    case LexemeType.Var:
				    {
					    node = this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: true);

					    this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
					    pos++;
				    }
				    break;

			    case LexemeType.Func:
				    {
					    node = this.ParseNamedFunctionDeclaration(lexemes, ref pos);
				    }
				    break;

			    case LexemeType.If:
				    {
					    node = this.ParseIf(lexemes, ref pos);
				    }
				    break;
                case LexemeType.While:
				    {
					    node = this.ParseWhile(lexemes, ref pos);
				    }
				    break;

                case LexemeType.For:
				    {
					    node = this.ParseFor(lexemes, ref pos);
				    }
				    break;
                case LexemeType.ForEach:
				    {
					    node = this.ParseForEach(lexemes, ref pos);
				    }
				    break;

                case LexemeType.Return:
				    {
					    node = this.ParseReturn(lexemes, ref pos);
				    }
				    break;

                case LexemeType.Yield:
                    {
                        node = this.ParseYield(lexemes, ref pos);
                    }
                    break;

                default:
				    {
					    node = this.ParseExpression(lexemes, ref pos);

					    this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
					    pos++;
				    }
                    break;
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

        private UsingDeclarationNode ParseUsingDeclaration(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.Using, pos);
            pos++;

            UsingDeclarationNode node = Construct<UsingDeclarationNode>(lexemes, pos);

            node.NamespaceName = this.ParseNamespaceName(lexemes, ref pos);
                       
            return node;
        }

        private ConstDeclarationNode ParseConstDeclaration(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.Const, pos);
            pos++;

            ConstDeclarationNode node = Construct<ConstDeclarationNode>(lexemes, pos);

            this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
            node.ConstName = lexemes[pos].Value;

            pos++;
            this.EnsureLexemeType(lexemes, LexemeType.Gets, pos);

            pos++;
            node.ValueExpression = this.ParseExpression(lexemes, ref pos);
                        
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

		private FunctionNode ParseNamedFunctionDeclaration(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FunctionNode node = Construct<FunctionNode>(lexemes, pos);

			pos++;

            if (lexemes[pos].Type == LexemeType.Multiply)
            {
                node.IsGenerator = true;
                pos++;
            }

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

			SkipWhileEndStatement(lexemes, ref pos);

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
				node.InitializeSyntax = this.ParseVariableDeclaration(lexemes, ref pos, varKeyword: true);
			}
			else
			{
				node.InitializeSyntax = this.ParseExpression(lexemes, ref pos);
			}

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;
			node.EvaluateExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);

			pos++;
			node.IncrementExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);

			pos++;
			node.BodyStatementBlock = this.ParseStatementBlock(lexemes, ref pos);

			return node;
		}

		private ForEachNode ParseForEach(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.ForEach, pos);

			ForEachNode node = Construct<ForEachNode>(lexemes, pos);

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.OpenParen, pos);

			pos++;
			int variableDeclarationPos = pos;
			node.VariableDeclaration = this.ParseVariableDeclaration(lexemes, ref pos, true);

			if (node.VariableDeclaration.ValueExpression != null)
			{
				throw new SyntaxException(string.Format("ForEach variable declaration may not have a value expression at line {0} column {1}.", lexemes[variableDeclarationPos].Line, lexemes[variableDeclarationPos].Column));
			}

			this.EnsureLexemeType(lexemes, LexemeType.In, pos);

			pos++;
			node.SourceExpression = this.ParseExpression(lexemes, ref pos);

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
			node.ValueExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
			pos++;

			return node;
		}

        private YieldNode ParseYield(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.Yield, pos);

            YieldNode node = Construct<YieldNode>(lexemes, pos);

            pos++;
            node.ValueExpression = this.ParseExpression(lexemes, ref pos);

            this.EnsureLexemeType(lexemes, LexemeType.EndStatement, pos);
            pos++;

            return node;
        }

        private ExpressionNode ParseExpression(IList<Lexeme> lexemes, ref int pos)
		{
			return this.ParseAssignmentExpression(lexemes, ref pos);
		}

		private ExpressionNode ParseAssignmentExpression(IList<Lexeme> lexemes, ref int pos)
		{
			ExpressionNode node = this.ParseConditionalOrExpression(lexemes, ref pos);

			if ((node is VariableReferenceNode || node is MemberAccessNode) &&
				IsAssignmentOperator(lexemes[pos].Type))
			{ 
				AssignmentOpeartionNode assignNode = Construct<AssignmentOpeartionNode>(lexemes, pos);
				assignNode.TargetExpression = node;

				switch (lexemes[pos].Type)
				{
					case LexemeType.Gets:
						assignNode.Type = AssignmentOperationType.Gets;
						break;

					case LexemeType.PlusGets:
						assignNode.Type = AssignmentOperationType.AddGets;
						break;

					case LexemeType.MinusGets:
						assignNode.Type = AssignmentOperationType.SubtractGets;
						break;

					case LexemeType.MultiplyGets:
						assignNode.Type = AssignmentOperationType.MultiplyGets;
						break;

					case LexemeType.DivideGets:
						assignNode.Type = AssignmentOperationType.DivideGets;
						break;

					default:
						throw new SyntaxException(string.Format("An assignment operation was expected at line {0} column {1}.", lexemes[pos].Line, lexemes[pos].Column));
				}

				pos++;
				assignNode.ValueExpression = this.ParseExpression(lexemes, ref pos);

				node = assignNode;
			}

			return node;
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

			if (lexemes[pos].Type == LexemeType.GreaterThan || lexemes[pos].Type == LexemeType.GreaterThanOrEqualTo ||
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

                if (opType == OperationType.LessThan && LookAheadUntilEndStatement(lexemes, pos, LexemeType.GreaterThan) > -1)
                {
                    GenericTypeAccessNode gtaNode = new GenericTypeAccessNode();
                    gtaNode.SourceExpression = node;
                    this.ParseGenericArgs(gtaNode.GenericArgs, lexemes, ref pos);                    

                    node = gtaNode;
                }
                else
                {
                    pos++;

                    OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                    opNode.Type = opType;
                    opNode.LeftHand = node;
                    opNode.RightHand = this.ParseAdditiveExpression(lexemes, ref pos);

                    node = opNode;
                }
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
			ExpressionNode node = this.ParseUnaryExpression(lexemes, ref pos);

			if (lexemes[pos].Type == LexemeType.Multiply || lexemes[pos].Type == LexemeType.Divide)
			{
				OperationType opType = lexemes[pos].Type == LexemeType.Multiply ? OperationType.Multiply : OperationType.Divide;
				pos++;

                OperationNode opNode = Construct<OperationNode>(lexemes, pos);
                opNode.Type = opType;
                opNode.LeftHand = node;
				opNode.RightHand = this.ParseUnaryExpression(lexemes, ref pos);

				node = opNode;
			}
			
			return node;
		}

		private ExpressionNode ParseUnaryExpression(IList<Lexeme> lexemes, ref int pos)
		{
			if (IsUnaryOperator(lexemes[pos].Type))
			{
				UnaryOperationNode node = Construct<UnaryOperationNode>(lexemes, pos);

				switch (lexemes[pos].Type)
				{
					case LexemeType.Minus:
						node.Type = UnaryOperationType.Negate;
						break;

					case LexemeType.Not:
						node.Type = UnaryOperationType.LogicalNegate;
						break;

					case LexemeType.Increment:
						node.Type = UnaryOperationType.Increment;
						break;

					case LexemeType.Decrement:
						node.Type = UnaryOperationType.Decrement;
						break;

					default:
						ThrowUnableToParseException("UnaryExpression", lexemes, pos);
						break;
				}

				pos++;
				node.ValueExpression = this.ParseExpression(lexemes, ref pos);

				return node;
			}
			else
			{
				return this.ParsePrimaryExpression(lexemes, ref pos);
			}
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
			else
			{
				switch (lexemes[pos].Type)
				{
					case LexemeType.Func:
						node = this.ParseAnonymousFunction(lexemes, ref pos);
						break;

                    case LexemeType.New:
                        node = this.ParseConstructorCall(lexemes, ref pos);
                        break;

					case LexemeType.OpenBracket:
						node = this.ParseList(lexemes, ref pos);
						break;

					case LexemeType.OpenPipeBracket:
						node = this.ParseArray(lexemes, ref pos);
						break;

					case LexemeType.OpenBrace:
						node = this.ParseDictionary(lexemes, ref pos);
						break;

					case LexemeType.Identifier:
						node = this.ParseVariableReference(lexemes, ref pos);
						break;

					case LexemeType.Null:
					case LexemeType.True:
                    case LexemeType.False:
					case LexemeType.Char:
					case LexemeType.Integer:
					case LexemeType.Float:
					case LexemeType.Double:
                    case LexemeType.String:
                        node = this.ParseValue(lexemes, ref pos);
						break;
				}
			}

			if (node == null)
				this.ThrowUnableToParseException("PrimaryExpression", lexemes, pos);

            if (pos < lexemes.Count - 1 && lexemes[pos + 1].Type == LexemeType.LessThan)
            {
                Console.WriteLine("");

            }
            else
            {
                while (lexemes[pos].Type == LexemeType.OpenParen ||
                       lexemes[pos].Type == LexemeType.Dot ||
                       lexemes[pos].Type == LexemeType.OpenBracket ||
                       IsPostfixOperator(lexemes[pos].Type))
                {
                    if (lexemes[pos].Type == LexemeType.OpenParen)
                    {
                        node = this.ParseFunctionCall(node, lexemes, ref pos);
                    }
                    else if (lexemes[pos].Type == LexemeType.Dot)
                    {
                        node = this.ParseMemberAccess(node, lexemes, ref pos);
                    }
                    else if (lexemes[pos].Type == LexemeType.OpenBracket)
                    {
                        node = this.ParseElementAccess(node, lexemes, ref pos);
                    }
                    else if (IsPostfixOperator(lexemes[pos].Type))
                    {
                        node = this.ParsePostfixOperation(node, lexemes, ref pos);
                    }
                }
            }

			return node;
		}
               
		private FunctionNode ParseAnonymousFunction(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Func, pos);

			FunctionNode node = Construct<FunctionNode>(lexemes, pos);

            pos++;

            if (lexemes[pos].Type == LexemeType.Multiply)
            {
                node.IsGenerator = true;
                pos++;
            }
            
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

		private ListNode ParseList(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenBracket, pos);

			ListNode node = Construct<ListNode>(lexemes, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseBracket && lexemes[pos].Type != LexemeType.EOF)
			{
				node.ValueExpressions.Add(this.ParseExpression(lexemes, ref pos));

				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					node.ValueExpressions.Add(this.ParseExpression(lexemes, ref pos));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseBracket, pos);
			pos++;

			return node;
		}

		private ArrayNode ParseArray(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenPipeBracket, pos);

			ArrayNode node = Construct<ArrayNode>(lexemes, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.ClosePipeBracket && lexemes[pos].Type != LexemeType.EOF)
			{
				node.ValueExpressions.Add(this.ParseExpression(lexemes, ref pos));

				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					node.ValueExpressions.Add(this.ParseExpression(lexemes, ref pos));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.ClosePipeBracket, pos);
			pos++;

			return node;
		}

		private DictionaryNode ParseDictionary(IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenBrace, pos);

			DictionaryNode node = Construct<DictionaryNode>(lexemes, pos);

			pos++;
			while (lexemes[pos].Type != LexemeType.CloseBrace && lexemes[pos].Type != LexemeType.EOF)
			{
				node.Pairs.Add(this.ParseDictionaryPair(lexemes, ref pos));

				while (lexemes[pos].Type == LexemeType.Comma)
				{
					pos++;
					node.Pairs.Add(this.ParseDictionaryPair(lexemes, ref pos));
				}
			}

			this.EnsureLexemeType(lexemes, LexemeType.CloseBrace, pos);
			pos++;

			return node;
		}

		private DictionaryPairNode ParseDictionaryPair(IList<Lexeme> lexemes, ref int pos)
		{
			DictionaryPairNode node = Construct<DictionaryPairNode>(lexemes, pos);

			if (lexemes[pos].Type == LexemeType.Identifier)
			{
				node.KeyExpression = new StringValueNode() { Value = lexemes[pos].Value };
				pos++;
			}
			else
			{
				node.KeyExpression = this.ParseExpression(lexemes, ref pos);
			}

			this.EnsureLexemeType(lexemes, LexemeType.Colon, pos);
            pos++;

			node.ValueExpression = this.ParseExpression(lexemes, ref pos);

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

        private ConstructorCallNode ParseConstructorCall(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.New, pos);

            ConstructorCallNode node = Construct<ConstructorCallNode>(lexemes, pos);

            pos++;
            node.TypeName = this.ParseTypeName(lexemes, ref pos);

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

        private string ParseNamespaceName(IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

            StringBuilder name = new StringBuilder(lexemes[pos].Value.Length);

            while (lexemes[pos].Type == LexemeType.Identifier)
            {
                name.Append(lexemes[pos].Value);

                pos++;
                if (lexemes[pos].Type == LexemeType.Dot)
                {
                    name.Append(".");

                    pos++;
                    this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
                }
            }

            return name.ToString();
        }

        private TypeNameNode ParseTypeName(IList<Lexeme> lexemes, ref int pos)
        {
            TypeNameNode node = new TypeNameNode();

            if (lexemes[pos].Type == LexemeType.OpenParen)
            {
                pos++;

                node.TypeExpression = this.ParsePrimaryExpression(lexemes, ref pos);

                this.EnsureLexemeType(lexemes, LexemeType.CloseParen, pos);
                pos++;
            }
            else
            {
                this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);

                node.TypeExpression = this.ParseVariableReference(lexemes, ref pos);

                while (lexemes[pos].Type == LexemeType.Dot)
                    node.TypeExpression = this.ParseMemberAccess(node.TypeExpression, lexemes, ref pos);
            }

            if (lexemes[pos].Type == LexemeType.LessThan)
                this.ParseGenericArgs(node.GenericArgs, lexemes, ref pos);

            return node;
        }

        private void ParseGenericArgs(SyntaxNodeCollection<TypeNameNode> args, IList<Lexeme> lexemes, ref int pos)
        {
            this.EnsureLexemeType(lexemes, LexemeType.LessThan, pos);
            pos++;

            args.Add(this.ParseTypeName(lexemes, ref pos));

            while (lexemes[pos].Type == LexemeType.Comma)
            {
                pos++;
                args.Add(this.ParseTypeName(lexemes, ref pos));
            }

            this.EnsureLexemeType(lexemes, LexemeType.GreaterThan, pos);
            pos++;
        }

		private MemberAccessNode ParseMemberAccess(ExpressionNode sourceExpression, IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.Dot, pos);

			MemberAccessNode node = Construct<MemberAccessNode>(lexemes, pos);
			node.SourceExpression = sourceExpression;

			pos++;
			this.EnsureLexemeType(lexemes, LexemeType.Identifier, pos);
			node.MemberName = lexemes[pos].Value;

			pos++;
			return node;
		}

		private ElementAccessNode ParseElementAccess(ExpressionNode sourceExpression, IList<Lexeme> lexemes, ref int pos)
		{
			this.EnsureLexemeType(lexemes, LexemeType.OpenBracket, pos);

			ElementAccessNode node = Construct<ElementAccessNode>(lexemes, pos);
			node.SourceExpression = sourceExpression;

			pos++;
			node.ElementExpression = this.ParseExpression(lexemes, ref pos);

			this.EnsureLexemeType(lexemes, LexemeType.CloseBracket, pos);
			pos++;

			return node;
		}

		private PostfixOperationNode ParsePostfixOperation(ExpressionNode sourceExpression, IList<Lexeme> lexemes, ref int pos)
		{
			if (sourceExpression is PostfixOperationNode)
				throw new SyntaxException(string.Format("Postfix opeartion may not be applied to postfix operation at line {0} column {1}.", lexemes[pos].Line, lexemes[pos].Column));

			PostfixOperationNode node = Construct<PostfixOperationNode>(lexemes, pos);
			node.SourceExpression = sourceExpression;

			if (lexemes[pos].Type == LexemeType.Increment)
			{
				node.Type = PostfixOperationType.Increment;
			}
			else if (lexemes[pos].Type == LexemeType.Decrement)
			{
				node.Type = PostfixOperationType.Decrement;
			}
			else
			{
				ThrowUnableToParseException("PostfixOperation", lexemes, pos);
			}

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

        private ValueNode ParseValue(IList<Lexeme> lexemes, ref int pos)
        {
            ValueNode node = null;

            switch (lexemes[pos].Type)
            {
                case LexemeType.Null:
                    node = Construct<NullValueNode>(lexemes, pos);
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

				case LexemeType.Double:
					var doubleNode = Construct<DoubleValueNode>(lexemes, pos);
					doubleNode.Value = double.Parse(lexemes[pos].Value, CultureInfo.InvariantCulture);
					node = doubleNode;
					pos++;
					break;
            }

            if (node == null)
                this.ThrowUnableToParseException("Value", lexemes, pos);

            return node;
        }
	}
}
