using System;
using System.Collections.Generic;

namespace Snowflake.Lexing
{
	public class ScriptLexer
	{
		private string ConvertEscapeCodes(string input)
		{
			return input.Replace("\\n", "\n")
						.Replace("\\t", "\t");
		}

		public IList<Lexeme> Lex(string text)
		{
			List<Lexeme> lexemes = new List<Lexeme>();

			text += '\n'; // Ensure new line at EOF

			bool output = false; // If true, write the lexeme.
			string value = ""; // The current value of the lexeme.
			LexemeType type = LexemeType.Unknown;
			bool isFloat = false; // For parsing numbers

			int line = 1, column = 1; // Current position of the lexeme being parsed.
			int curLine = 1, curColumn = 1; // Current position of parsing.

			for (int i = 0; i < text.Length - 1; i++, curColumn++)
			{
				if (text[i] == '\n')
				{
					curLine++;
					curColumn = 1;
				}

				if (type == LexemeType.Unknown) // We're looking for a lexeme
				{
					if (char.IsWhiteSpace(text[i])) // Skip over white space
					{
						continue;
					}
					else if (text[i] == '\'') // Start of a string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.Char;
					}
					else if (text[i] == '"') // Start of a magic string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.String;
					}
					else if (text[i] == ';') // End Statement
					{
						lexemes.Add(new Lexeme(LexemeType.EndStatement, null, curLine, curColumn));
					}
					else if (text[i] == '.') // Period
					{
						lexemes.Add(new Lexeme(LexemeType.Dot, null, curLine, curColumn));
					}
					else if (text[i] == ',') // Period
					{
						lexemes.Add(new Lexeme(LexemeType.Comma, null, curLine, curColumn));
					}
					else if (text[i] == '(') // OpenParen
					{
						lexemes.Add(new Lexeme(LexemeType.OpenParen, null, curLine, curColumn));
					}
					else if (text[i] == ')') // CloseParen
					{
						lexemes.Add(new Lexeme(LexemeType.CloseParen, null, curLine, curColumn));
					}
					else if (text[i] == '[') // OpenBracket or OpenPipeBracket
					{
						if (i + 1 < text.Length && text[i + 1] == '|')
						{
							lexemes.Add(new Lexeme(LexemeType.OpenPipeBracket, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.OpenBracket, null, curLine, curColumn));
						}
					}
					else if (text[i] == ']') // CloseBracket
					{
						lexemes.Add(new Lexeme(LexemeType.CloseBracket, null, curLine, curColumn));
					}
					else if (text[i] == '{') // OpenBrace
					{
						lexemes.Add(new Lexeme(LexemeType.OpenBrace, null, curLine, curColumn));
					}
					else if (text[i] == '}') // CloseBrace
					{
						lexemes.Add(new Lexeme(LexemeType.CloseBrace, null, curLine, curColumn));
					}
					else if (text[i] == ':')
					{
						lexemes.Add(new Lexeme(LexemeType.Colon, null, curLine, curColumn));
					}
					else if (text[i] == '=') // Gets or EqualTo or MapsTo
					{
						if (i + 1 < text.Length && text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.EqualTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.Gets, null, curLine, curColumn));
						}
					}
					else if (text[i] == '!') // Not or NotEqualTo
					{
						if (i + 1 < text.Length && text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.NotEqualTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.Not, null, curLine, curColumn));
						}
					}
					else if (text[i] == '>')
					{
						if (i + 1 < text.Length && text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.GreaterThanOrEqualTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.GreaterThan, null, curLine, curColumn));
						}
					}
					else if (text[i] == '<')
					{
						if (i + 1 < text.Length && text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.LessThanOrEqualTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.LessThan, null, curLine, curColumn));
						}
					}
					else if (text[i] == '+') // Plus or PlusGets
					{
						if (text[i + 1] == '+')
						{
							lexemes.Add(new Lexeme(LexemeType.Increment, null, curLine, curColumn));
							i++;
						}
						else if (i + 1 < text.Length && text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.PlusGets, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.Plus, null, curLine, curColumn));
						}
					}
					else if (text[i] == '-') // Minus or MinusGets
					{
						if (i + 1 < text.Length && !char.IsWhiteSpace(text[i + 1]))
						{
							if (text[i + 1] == '-')
							{
								lexemes.Add(new Lexeme(LexemeType.Decrement, null, curLine, curColumn));
								i++;
							}
							else if (text[i + 1] == '=')
							{
								lexemes.Add(new Lexeme(LexemeType.MinusGets, null, curLine, curColumn));
								i++;
							}
							else if (char.IsDigit(text[i + 1]))
							{
								type = LexemeType.Numeric;
								value += '-';
								value += text[i + 1];
								i++;
							}
							else
							{
								lexemes.Add(new Lexeme(LexemeType.Minus, null, curLine, curColumn));
							}
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.Minus, null, curLine, curColumn));
						}
					}
					else if (text[i] == '*') // Multiply or MultiplyGets
					{
						if (i + 1 < text.Length && text[i + 1] != '=')
						{
							lexemes.Add(new Lexeme(LexemeType.Multiply, null, curLine, curColumn));
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.MultiplyGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '/') // Multiply or MultiplyGets
					{
						if (i + 1 < text.Length && text[i + 1] != '=')
						{
							lexemes.Add(new Lexeme(LexemeType.Divide, null, curLine, curColumn));
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.DivideGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '&' && i + 1 < text.Length && text[i + 1] == '&') // And
					{
						lexemes.Add(new Lexeme(LexemeType.ConditionalAnd, null, curLine, curColumn));
						i++;
					}
					else if (text[i] == '|') // ConditionalOr or ClosePipeBracket
					{
						if (i + 1 < text.Length)
						{
							if (text[i + 1] == '|')
							{
								lexemes.Add(new Lexeme(LexemeType.ConditionalOr, null, curLine, curColumn));
								i++;
							}
							else if (text[i + 1] == ']')
							{
								lexemes.Add(new Lexeme(LexemeType.ClosePipeBracket, null, curLine, curColumn));
								i++;
							}
						}
					}
					else if (char.IsLetter(text[i]) || text[i] == '_') // Identifier
					{
						type = LexemeType.Identifier;
						value += text[i];
					}
					else if (char.IsNumber(text[i])) // Numeric
					{
						type = LexemeType.Numeric;
						value += text[i];
					}
				}
				else if (type == LexemeType.Char) // We're inside a char
				{
					if (text[i] != '\'')
					{
						value += text[i];
					}
					else if (text[i - 1] == '\\')
					{
						value += text[i];
					}
					else
					{
						output = true;
					}
				}
				else if (type == LexemeType.String) // We're inside a string
				{
					if (text[i] != '"')
					{
						value += text[i];
					}
					else if (text[i - 1] == '\\')
					{
						value += text[i];
					}
					else
					{
						output = true;
					}
				}
				else if (type == LexemeType.Identifier) // We're inside an identifier
				{
					if (Char.IsLetter(text[i]) || Char.IsNumber(text[i]) || text[i] == '_')
					{
						value += text[i];
					}
					else
					{
						output = true;
						i--;
					}
				}
				else if (type == LexemeType.Numeric) // We're inside a numeric
				{
					if (Char.IsNumber(text[i]))
					{
						value += text[i];
					}
					else if (text[i] == '.')
					{
						isFloat = true;
						value += text[i];
					}
					else
					{
						output = true;
						i--;
					}
				}

				if (output) // We've reached the end of a long lexeme
				{
					if (type == LexemeType.Identifier)
					{
						switch (value)
						{
							case "var":
								lexemes.Add(new Lexeme(LexemeType.Var, null, curLine, curColumn));
								break;

                            case "new":
                                lexemes.Add(new Lexeme(LexemeType.New, null, curLine, curColumn));
                                break;

							case "func":
								lexemes.Add(new Lexeme(LexemeType.Func, null, line, column));
								break;

							case "return":
								lexemes.Add(new Lexeme(LexemeType.Return, null, line, column));
								break;

							case "if":
								lexemes.Add(new Lexeme(LexemeType.If, null, line, column));
								break;

							case "else":
								lexemes.Add(new Lexeme(LexemeType.Else, null, line, column));
								break;

							case "while":
								lexemes.Add(new Lexeme(LexemeType.While, null, line, column));
								break;

							case "for":
								lexemes.Add(new Lexeme(LexemeType.For, null, line, column));
								break;

							case "foreach":
								lexemes.Add(new Lexeme(LexemeType.ForEach, null, line, column));
								break;

							case "in":
								lexemes.Add(new Lexeme(LexemeType.In, null, line, column));
								break;
                                                            
							case "null":
								lexemes.Add(new Lexeme(LexemeType.Null, null, line, column));
								break;

							case "true":
								lexemes.Add(new Lexeme(LexemeType.True, null, line, column));
								break;

							case "false":
								lexemes.Add(new Lexeme(LexemeType.False, null, line, column));
								break;

							default:
								lexemes.Add(new Lexeme(type, value, line, column));
								break;
						}
					}
					else if (type == LexemeType.Numeric)
					{
						if (isFloat)
						{
							type = LexemeType.Float;
						}
						else
						{
							type = LexemeType.Integer;
						}

						lexemes.Add(new Lexeme(type, value, line, column));
					}
					else if (type == LexemeType.Char)
					{
						if (value.Length > 1)
						{
							if (value.Length > 2 || value[0] != '\\')
							{
								throw new LexerException("Invalid char: " + value + ".");
							}
						}

						lexemes.Add(new Lexeme(type, this.ConvertEscapeCodes(value.Replace("\\'", "'")), line, column));
					}
					else if (type == LexemeType.String)
					{
						lexemes.Add(new Lexeme(type, this.ConvertEscapeCodes(value.Replace("\\\"", "\"")), line, column));
					}

					output = false;
					type = LexemeType.Unknown;
					isFloat = false;
					value = "";
				}
			}

			if (type != LexemeType.Unknown)
				throw new LexerException("Unexpected end of file.");

			// Add an EOF to mark the end of the script.
			lexemes.Add(new Lexeme(LexemeType.EOF, null, line, column));

			return lexemes;
		}
	}
}
