using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptLexeme
	{
		public ScriptLexemeType Type
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

		public ScriptLexeme(ScriptLexemeType type, string val)
			: this(type, val, 0, 0)
		{
		}

		public ScriptLexeme(ScriptLexemeType type, string val, int line, int column)
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

		public static List<ScriptLexeme> Parse(string text)
		{
			List<ScriptLexeme> lexemes = new List<ScriptLexeme>();

			text += '\n'; // Ensure new line at EOF

			bool output = false; // If true, write the lexeme.
			string value = ""; // The current value of the lexeme.
			ScriptLexemeType type = ScriptLexemeType.Unknown; 
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

				if(type == ScriptLexemeType.Unknown) // We're looking for a lexeme
				{
					if(char.IsWhiteSpace(text[i])) // Skip over white space
					{
						continue;
					}
					else if(text[i] == '\'') // Start of a string
					{
						line = curLine;
						column = curColumn;

						type = ScriptLexemeType.Char;
					}
					else if(text[i] == '"') // Start of a magic string
					{
						line = curLine;
						column = curColumn;

						type = ScriptLexemeType.String;
					}
					else if(text[i] == ';') // End Statement
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.EndStatement, null, curLine, curColumn));
					}
					else if(text[i] == '$') // Variable
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.Variable, null, curLine, curColumn));
					}
					else if(text[i] == '.') // Period
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.Period, null, curLine, curColumn));
					}
					else if(text[i] == ',') // Period
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.Comma, null, curLine, curColumn));
					}
					else if(text[i] == '(') // OpenParen
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.OpenParen, null, curLine, curColumn));
					}
					else if(text[i] == ')') // CloseParen
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.CloseParen, null, curLine, curColumn));
					}
					else if(text[i] == '[') // OpenBracket
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.OpenBracket, null, curLine, curColumn));
					}
					else if(text[i] == ']') // CloseBracket
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.CloseBracket, null, curLine, curColumn));
					}
					else if(text[i] == '{') // OpenBrace
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.OpenBrace, null, curLine, curColumn));
					}
					else if(text[i] == '}') // CloseBrace
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.CloseBrace, null, curLine, curColumn));
					}
					else if(text[i] == '=') // Gets or EqualTo or MapsTo
					{
						if(text[i + 1] == '=')
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.EqualTo, null, curLine, curColumn));
							i++;
						}
						else if(text[i + 1] == '>')
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.MapsTo, null, curLine, curColumn));
							i++;
						}
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Gets, null, curLine, curColumn));
						}
					}
					else if(text[i] == '!') // Not or NotEqualTo
					{
						if(text[i + 1] != '=')
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Not, null, curLine, curColumn));
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.NotEqualTo, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '+') // Plus or PlusGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Plus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.PlusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '-') // Minus or MinusGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Minus, null, curLine, curColumn));
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.MinusGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '*') // Multiply or MultiplyGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Multiply, null, curLine, curColumn));
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.MultiplyGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '/') // Multiply or MultiplyGets
					{
						if(text[i + 1] != '=')
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.Divide, null, curLine, curColumn));
						else
						{
							lexemes.Add(new ScriptLexeme(ScriptLexemeType.DivideGets, null, curLine, curColumn));
							i++;
						}
					}
					else if(text[i] == '&' && text[i + 1] == '&') // And
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.LogicalAnd, null, curLine, curColumn));
						i++;
					}
					else if(text[i] == '|' && text[i + 1] == '|') // Or
					{
						lexemes.Add(new ScriptLexeme(ScriptLexemeType.LogicalOr, null, curLine, curColumn));
						i++;
					}
					else if(char.IsLetter(text[i]) || text[i] == '_') // Identifier
					{
						type = ScriptLexemeType.Identifier;
						value += text[i];
					}
					else if(char.IsNumber(text[i])) // Numeric
					{
						type = ScriptLexemeType.Numeric;
						value += text[i];
					}
				}
				else if(type == ScriptLexemeType.Char) // We're inside a char
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
				else if(type == ScriptLexemeType.String) // We're inside a string
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
				else if(type == ScriptLexemeType.Identifier) // We're inside an identifier
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
				else if(type == ScriptLexemeType.Numeric) // We're inside a numeric
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
					if(type == ScriptLexemeType.Identifier)
					{
						string valueToUpper = value.ToUpper();

						switch (valueToUpper)
						{
							case "FUNC":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.Func, null, line, column));
								break;

							case "IF":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.If, null, line, column));
								break;

							case "ELSE":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.Else, null, line, column));
								break;

							case "WHILE":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.While, null, line, column));
								break;

							case "FOR":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.For, null, line, column));
								break;

							case "FOREACH":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.ForEach, null, line, column));
								break;

							case "AS":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.As, null, line, column));
								break;

							case "ECHO":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.Echo, null, line, column));
								break;

							case "NULL":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.Null, null, line, column));
								break;

							case "TRUE":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.True, null, line, column));
								break;

							case "FALSE":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.False, null, line, column));
								break;

							case "ARRAY":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.Array, null, line, column));
								break;

							case "LIST":
								lexemes.Add(new ScriptLexeme(ScriptLexemeType.List, null, line, column));
								break;

							default:
								lexemes.Add(new ScriptLexeme(type, value, line, column));
								break;
						}						
					}
					else if(type == ScriptLexemeType.Numeric)
					{
						if(isFloat)
							type = ScriptLexemeType.Float;
						else
							type = ScriptLexemeType.Integer;

						lexemes.Add(new ScriptLexeme(type, value));
					}
					else if(type == ScriptLexemeType.Char)
					{
						if (value.Length > 1)
						{
							if (value.Length > 2 || value[0] != '\\')
							{
								throw new ScriptParseException("Invalid char: " + value + ".");
							}
						}

						lexemes.Add(new ScriptLexeme(type, ConvertEscapeCodes(value.Replace("\\'", "'")), line, column));
					}
					else if(type == ScriptLexemeType.String)
					{
						lexemes.Add(new ScriptLexeme(type, ConvertEscapeCodes(value.Replace("\\\"", "\"")), line, column));
					}

					output = false;
					type = ScriptLexemeType.Unknown;
					isFloat = false;
					value = "";
				}
			}

			if (type != ScriptLexemeType.Unknown)
				throw new ScriptParseException("Unexpected end of file.");

			// Add an EOF to mark the end of the script.
			lexemes.Add(new ScriptLexeme(ScriptLexemeType.EOF, null, line, column));

			return lexemes;
		}
	}
}
