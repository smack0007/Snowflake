using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class Lexeme
	{
		public LexemeType Type
		{
			get;
			set;
		}

		public string Val
		{
			get;
			set;
		}

		public int Line
		{
			get;
			set;
		}

		public int Column
		{
			get;
			set;
		}

		public Lexeme(LexemeType type, string val)
			: this(type, val, 0, 0)
		{
		}

		public Lexeme(LexemeType type, string val, int line, int column)
		{
			this.Type = type;

			if(val != null)
				this.Val = val;
			else
				this.Val = null;

			this.Line = line;
			this.Column = column;
		}

		public override string ToString()
		{
			return "{" + Type + ", " + Val + ", " + Line + ", " + Column + "}";
		}

		private static string ConvertEscapeCodes(string input)
		{
			return input.Replace("\\n", "\n")
						.Replace("\\t", "\t");
		}

		public static List<Lexeme> Parse(string text)
		{
			List<Lexeme> lexemes = new List<Lexeme>();

			text += '\n'; // Ensure new line at EOF

			bool output = false; // If true, write the lexeme.
			string value = ""; // The current value of the lexeme.
			LexemeType type = LexemeType.Unknown; 
			bool isFloat = false; // For parsing numbers

			int line = 1, column = 1; // Current position of the lexeme being parsed.
			int curLine = 1, curColumn = 1; // Current position of parsing.

			for(int i = 0; i < text.Length - 1; i++, curColumn++)
			{
				if(text[i] == '\n')
				{
					curLine++;
					curColumn = 1;
				}

				if(type == LexemeType.Unknown) // We're looking for a lexeme
				{
					if(char.IsWhiteSpace(text[i])) // Skip over white space
					{
						continue;
					}
					else if(text[i] == '\'') // Start of a string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.Char;
					}
					else if(text[i] == '"') // Start of a magic string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.String;
					}
					else if(text[i] == ';') // End Statement
					{
						lexemes.Add(new Lexeme(LexemeType.EndStatement, null, curLine, curColumn));
					}
					else if(text[i] == '$') // Variable
					{
						lexemes.Add(new Lexeme(LexemeType.Variable, null, curLine, curColumn));
					}
					else if(text[i] == '.') // Period
					{
						lexemes.Add(new Lexeme(LexemeType.Period, null, curLine, curColumn));
					}
					else if(text[i] == ',') // Period
					{
						lexemes.Add(new Lexeme(LexemeType.Comma, null, curLine, curColumn));
					}
					else if(text[i] == '(') // OpenParen
					{
						lexemes.Add(new Lexeme(LexemeType.OpenParen, null, curLine, curColumn));
					}
					else if(text[i] == ')') // CloseParen
					{
						lexemes.Add(new Lexeme(LexemeType.CloseParen, null, curLine, curColumn));
					}
					else if(text[i] == '[') // OpenBracket
					{
						lexemes.Add(new Lexeme(LexemeType.OpenBracket, null, curLine, curColumn));
					}
					else if(text[i] == ']') // CloseBracket
					{
						lexemes.Add(new Lexeme(LexemeType.CloseBracket, null, curLine, curColumn));
					}
					else if(text[i] == '{') // OpenBrace
					{
						lexemes.Add(new Lexeme(LexemeType.OpenBrace, null, curLine, curColumn));
					}
					else if(text[i] == '}') // CloseBrace
					{
						lexemes.Add(new Lexeme(LexemeType.CloseBrace, null, curLine, curColumn));
					}
					else if(text[i] == '=') // Gets or EqualTo or MapsTo
					{
						if(text[i + 1] == '=')
						{
							lexemes.Add(new Lexeme(LexemeType.EqualTo, null, curLine, curColumn));
							i++;
						}
						else if(text[i + 1] == '>')
						{
							lexemes.Add(new Lexeme(LexemeType.MapsTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new Lexeme(LexemeType.Gets, null, curLine, curColumn));
						}
					}
					else if(text[i] == '!') // Not or NotEqualTo
					{
						if(text[i + 1] != '=')
							lexemes.Add(new Lexeme(LexemeType.Not, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(LexemeType.NotEqualTo, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '+') // Plus or PlusGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new Lexeme(LexemeType.Plus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(LexemeType.PlusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '-') // Minus or MinusGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new Lexeme(LexemeType.Minus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(LexemeType.MinusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '*') // Multiply or MultiplyGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new Lexeme(LexemeType.Multiply, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(LexemeType.MultiplyGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '/') // Multiply or MultiplyGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new Lexeme(LexemeType.Divide, null, curLine, curColumn));
						else
						{
							lexemes.Add(new Lexeme(LexemeType.DivideGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '&' && text[i + 1] == '&') // And
					{
						lexemes.Add(new Lexeme(LexemeType.LogicalAnd, null, curLine, curColumn));
						i++;
					}
					else if(text[i] == '|' && text[i + 1] == '|') // Or
					{
						lexemes.Add(new Lexeme(LexemeType.LogicalOr, null, curLine, curColumn));
						i++;
					}
					else if(char.IsLetter(text[i]) || text[i] == '_') // Identifier
					{
						type = LexemeType.Identifier;
						value += text[i];
					}
					else if(char.IsNumber(text[i])) // Numeric
					{
						type = LexemeType.Numeric;
						value += text[i];
					}
				}
				else if(type == LexemeType.Char) // We're inside a char
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
				else if(type == LexemeType.String) // We're inside a string
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
				else if(type == LexemeType.Identifier) // We're inside an identifier
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
				else if(type == LexemeType.Numeric) // We're inside a numeric
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

				if(output) // We've reached the end of a long lexeme
				{
					if(type == LexemeType.Identifier)
					{
						string valueToUpper = value.ToUpper();

						switch (valueToUpper)
						{
							case "FUNC":
								lexemes.Add(new Lexeme(LexemeType.Func, null, line, column));
								break;

							case "IF":
								lexemes.Add(new Lexeme(LexemeType.If, null, line, column));
								break;

							case "ELSE":
								lexemes.Add(new Lexeme(LexemeType.Else, null, line, column));
								break;

							case "WHILE":
								lexemes.Add(new Lexeme(LexemeType.While, null, line, column));
								break;

							case "FOR":
								lexemes.Add(new Lexeme(LexemeType.For, null, line, column));
								break;

							case "FOREACH":
								lexemes.Add(new Lexeme(LexemeType.ForEach, null, line, column));
								break;

							case "AS":
								lexemes.Add(new Lexeme(LexemeType.As, null, line, column));
								break;

							case "ECHO":
								lexemes.Add(new Lexeme(LexemeType.Echo, null, line, column));
								break;

							case "NULL":
								lexemes.Add(new Lexeme(LexemeType.Null, null, line, column));
								break;

							case "TRUE":
								lexemes.Add(new Lexeme(LexemeType.True, null, line, column));
								break;

							case "FALSE":
								lexemes.Add(new Lexeme(LexemeType.False, null, line, column));
								break;

							case "ARRAY":
								lexemes.Add(new Lexeme(LexemeType.Array, null, line, column));
								break;

							case "LIST":
								lexemes.Add(new Lexeme(LexemeType.List, null, line, column));
								break;

							default:
								lexemes.Add(new Lexeme(type, value, line, column));
								break;
						}						
					}
					else if(type == LexemeType.Numeric)
					{
						if(isFloat)
							type = LexemeType.Float;
						else
							type = LexemeType.Integer;

						lexemes.Add(new Lexeme(type, value));
					}
					else if(type == LexemeType.Char)
					{
						if (value.Length > 1)
						{
							if (value.Length > 2 || value[0] != '\\')
							{
								throw new ScriptException(ScriptError.ParseError, "Invalid char: " + value + ".");
							}
						}

						lexemes.Add(new Lexeme(type, ConvertEscapeCodes(value.Replace("\\'", "'")), line, column));
					}
					else if(type == LexemeType.String)
					{
						lexemes.Add(new Lexeme(type, ConvertEscapeCodes(value.Replace("\\\"", "\"")), line, column));
					}

					output = false;
					type = LexemeType.Unknown;
					isFloat = false;
					value = "";
				}
			}

			if (type != LexemeType.Unknown)
				throw new ScriptException(ScriptError.ParseError, "Unexpected end of file.");

			// Add an EOF to mark the end of the script.
			lexemes.Add(new Lexeme(LexemeType.EOF, null, line, column));

			return lexemes;
		}
	}
}
