using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public class ScriptLexer : ILexer
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
			ScriptLexemeType type = ScriptLexemeType.Unknown;
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

				if (type == ScriptLexemeType.Unknown) // We're looking for a lexeme
				{
					if (char.IsWhiteSpace(text[i])) // Skip over white space
					{
						continue;
					}
					else if (text[i] == '\'') // Start of a string
					{
						line = curLine;
						column = curColumn;

						type = ScriptLexemeType.Char;
					}
					else if (text[i] == '"') // Start of a magic string
					{
						line = curLine;
						column = curColumn;

						type = ScriptLexemeType.String;
					}
					else if (text[i] == ';') // End Statement
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.EndStatement, null, curLine, curColumn));
					}
					else if (text[i] == '$') // Variable
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.Variable, null, curLine, curColumn));
					}
					else if (text[i] == '.') // Period
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.Period, null, curLine, curColumn));
					}
					else if (text[i] == ',') // Period
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.Comma, null, curLine, curColumn));
					}
					else if (text[i] == '(') // OpenParen
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.OpenParen, null, curLine, curColumn));
					}
					else if (text[i] == ')') // CloseParen
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.CloseParen, null, curLine, curColumn));
					}
					else if (text[i] == '[') // OpenBracket
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.OpenBracket, null, curLine, curColumn));
					}
					else if (text[i] == ']') // CloseBracket
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.CloseBracket, null, curLine, curColumn));
					}
					else if (text[i] == '{') // OpenBrace
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.OpenBrace, null, curLine, curColumn));
					}
					else if (text[i] == '}') // CloseBrace
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.CloseBrace, null, curLine, curColumn));
					}
					else if (text[i] == '=') // Gets or EqualTo or MapsTo
					{
						if (text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.EqualTo, null, curLine, curColumn));
							i++;
						}
						else if (text[i + 1] == '>')
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.MapsTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.Gets, null, curLine, curColumn));
						}
					}
					else if (text[i] == '!') // Not or NotEqualTo
					{
						if (text[i + 1] != '=')
							lexemes.Add(new Lexeme(ScriptLexemeType.Not, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.NotEqualTo, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '+') // Plus or PlusGets
					{
						if (text[i + 1] != '=')
							lexemes.Add(new Lexeme(ScriptLexemeType.Plus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.PlusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '-') // Minus or MinusGets
					{
						if (text[i + 1] != '=')
							lexemes.Add(new Lexeme(ScriptLexemeType.Minus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.MinusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '*') // Multiply or MultiplyGets
					{
						if (text[i + 1] != '=')
							lexemes.Add(new Lexeme(ScriptLexemeType.Multiply, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.MultiplyGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '/') // Multiply or MultiplyGets
					{
						if (text[i + 1] != '=')
							lexemes.Add(new Lexeme(ScriptLexemeType.Divide, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(ScriptLexemeType.DivideGets, null, curLine, curColumn));
							i++;
						}
					}
					else if (text[i] == '&' && text[i + 1] == '&') // And
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.LogicalAnd, null, curLine, curColumn));
						i++;
					}
					else if (text[i] == '|' && text[i + 1] == '|') // Or
					{
						lexemes.Add(new Lexeme(ScriptLexemeType.LogicalOr, null, curLine, curColumn));
						i++;
					}
					else if (char.IsLetter(text[i]) || text[i] == '_') // Identifier
					{
						type = ScriptLexemeType.Identifier;
						value += text[i];
					}
					else if (char.IsNumber(text[i])) // Numeric
					{
						type = ScriptLexemeType.Numeric;
						value += text[i];
					}
				}
				else if (type == ScriptLexemeType.Char) // We're inside a char
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
				else if (type == ScriptLexemeType.String) // We're inside a string
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
				else if (type == ScriptLexemeType.Identifier) // We're inside an identifier
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
				else if (type == ScriptLexemeType.Numeric) // We're inside a numeric
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
					if (type == ScriptLexemeType.Identifier)
					{
						switch (value)
						{
							case "func":
								lexemes.Add(new Lexeme(ScriptLexemeType.Func, null, line, column));
								break;

							case "return":
								lexemes.Add(new Lexeme(ScriptLexemeType.Return, null, line, column));
								break;

							case "if":
								lexemes.Add(new Lexeme(ScriptLexemeType.If, null, line, column));
								break;

							case "else":
								lexemes.Add(new Lexeme(ScriptLexemeType.Else, null, line, column));
								break;

							case "while":
								lexemes.Add(new Lexeme(ScriptLexemeType.While, null, line, column));
								break;

							case "for":
								lexemes.Add(new Lexeme(ScriptLexemeType.For, null, line, column));
								break;

							case "foreach":
								lexemes.Add(new Lexeme(ScriptLexemeType.ForEach, null, line, column));
								break;

							case "as":
								lexemes.Add(new Lexeme(ScriptLexemeType.As, null, line, column));
								break;

							case "echo":
								lexemes.Add(new Lexeme(ScriptLexemeType.Echo, null, line, column));
								break;

							case "null":
								lexemes.Add(new Lexeme(ScriptLexemeType.Null, null, line, column));
								break;

							case "true":
								lexemes.Add(new Lexeme(ScriptLexemeType.True, null, line, column));
								break;

							case "false":
								lexemes.Add(new Lexeme(ScriptLexemeType.False, null, line, column));
								break;

							case "array":
								lexemes.Add(new Lexeme(ScriptLexemeType.Array, null, line, column));
								break;

							case "list":
								lexemes.Add(new Lexeme(ScriptLexemeType.List, null, line, column));
								break;

							default:
								lexemes.Add(new Lexeme(type, value, line, column));
								break;
						}
					}
					else if (type == ScriptLexemeType.Numeric)
					{
						if (isFloat)
							type = ScriptLexemeType.Float;
						else
							type = ScriptLexemeType.Integer;

						lexemes.Add(new Lexeme(type, value, line, column));
					}
					else if (type == ScriptLexemeType.Char)
					{
						if (value.Length > 1)
						{
							if (value.Length > 2 || value[0] != '\\')
							{
								throw new ScriptLexerException("Invalid char: " + value + ".");
							}
						}

						lexemes.Add(new Lexeme(type, this.ConvertEscapeCodes(value.Replace("\\'", "'")), line, column));
					}
					else if (type == ScriptLexemeType.String)
					{
						lexemes.Add(new Lexeme(type, this.ConvertEscapeCodes(value.Replace("\\\"", "\"")), line, column));
					}

					output = false;
					type = ScriptLexemeType.Unknown;
					isFloat = false;
					value = "";
				}
			}

			if (type != ScriptLexemeType.Unknown)
				throw new ScriptLexerException("Unexpected end of file.");

			// Add an EOF to mark the end of the script.
			lexemes.Add(new Lexeme(ScriptLexemeType.EOF, null, line, column));

			return lexemes;
		}
	}
}
