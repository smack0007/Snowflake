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

		public static List<Lexeme> Parse(ref string text)
		{
			List<Lexeme> lexemes = new List<Lexeme>();

			text += '\n'; // Ensure new line at EOF

			bool output = false;
			string value = "";
			LexemeType type = LexemeType.Unknown;
			bool isFloat = false; // For parsing numbers

			int line = 1, column = 1;
			int curLine = 1, curColumn = 1;

			for(int i = 0; i < text.Length - 1; i++, curColumn++)
			{
				if(text[i] == '\n')
				{
					curLine++;
					curColumn = 1;
				}

				if(type == LexemeType.Unknown) // We're looking for a lexeme
				{
					if(Char.IsWhiteSpace(text[i])) // Skip over white space
					{
						continue;
					}
					else if(text[i] == '\'') // Start of a string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.String;
					}
					else if(text[i] == '"') // Start of a magic string
					{
						line = curLine;
						column = curColumn;

						type = LexemeType.MagicString;
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
						lexemes.Add(new Lexeme(LexemeType.And, null, curLine, curColumn));
						i++;
					}
					else if(text[i] == '|' && text[i + 1] == '|') // Or
					{
						lexemes.Add(new Lexeme(LexemeType.And, null, curLine, curColumn));
						i++;
					}
					else if(Char.IsLetter(text[i]) || text[i] == '_') // Identifier
					{
						type = LexemeType.Identifier;
						value += text[i];
					}
					else if(Char.IsNumber(text[i])) // Numeric
					{
						type = LexemeType.Numeric;
						value += text[i];
					}
				}
				else if(type == LexemeType.String) // We're inside a string
				{
					if(text[i] != '\'')
						value += text[i];
					else if(text[i - 1] == '\\')
						value += text[i];
					else
						output = true;
				}
				else if(type == LexemeType.MagicString) // We're inside a magic string
				{
					if(text[i] != '"')
						value += text[i];
					else if(text[i - 1] == '\\')
						value += text[i];
					else
						output = true;
				}
				else if(type == LexemeType.Identifier) // We're inside an identifier
				{
					if(Char.IsLetter(text[i]) || Char.IsNumber(text[i]) || text[i] == '_')
						value += text[i];
					else
					{
						output = true;
						i--;
					}
				}
				else if(type == LexemeType.Numeric) // We're inside a numeric
				{
					if(Char.IsNumber(text[i]))
						value += text[i];
					else if(text[i] == '.')
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
						string valueUpper = value.ToUpper();

						// Check for keywords first, otherwise it's an identifier
						if(valueUpper == "IF")
							lexemes.Add(new Lexeme(LexemeType.If, null, line, column));
						else if(valueUpper == "ELSE")
							lexemes.Add(new Lexeme(LexemeType.Else, null, line, column));
						else if(valueUpper == "WHILE")
							lexemes.Add(new Lexeme(LexemeType.While, null, line, column));
						else if(valueUpper == "FOR")
							lexemes.Add(new Lexeme(LexemeType.For, null, line, column));
						else if(valueUpper == "FOREACH")
							lexemes.Add(new Lexeme(LexemeType.ForEach, null, line, column));
						else if(valueUpper == "AS")
							lexemes.Add(new Lexeme(LexemeType.As, null, line, column));
						else if(valueUpper == "ECHO")
							lexemes.Add(new Lexeme(LexemeType.Echo, null, line, column));
						else if(valueUpper == "NULL")
							lexemes.Add(new Lexeme(LexemeType.Null, null, line, column));
						else if(valueUpper == "TRUE")
							lexemes.Add(new Lexeme(LexemeType.True, null, line, column));
						else if(valueUpper == "FALSE")
							lexemes.Add(new Lexeme(LexemeType.False, null, line, column));
						else if(valueUpper == "ARRAY")
							lexemes.Add(new Lexeme(LexemeType.Array, null, line, column));
						else // It's an identifier
							lexemes.Add(new Lexeme(type, value, line, column));
					}
					else if(type == LexemeType.Numeric)
					{
						if(isFloat)
							type = LexemeType.Float;
						else
							type = LexemeType.Integer;

						lexemes.Add(new Lexeme(type, value));
					}
					else if(type == LexemeType.String)
					{
						lexemes.Add(new Lexeme(type, value.Replace("\\'", "'"), line, column));
					}
					else if(type == LexemeType.MagicString)
					{
						lexemes.Add(new Lexeme(type, value.Replace("\\\"", "\"").Replace("\\n", "\n"), line, column));
					}

					output = false;
					type = LexemeType.Unknown;
					isFloat = false;
					value = "";
				}
			}

			lexemes.Add(new Lexeme(LexemeType.EOF, null, line, column));

			return lexemes;
		}
	}
}
