using System;
using System.Collections.Generic;
using System.IO;

namespace Snowsoft.Scripting
{
	public class ScriptException : ApplicationException
	{
		public ScriptException(string message)
			: base(message)
		{
		}
	}

	public class Script
	{
		private List<Lexeme> lexemes;

		bool debug = false;

		/// <summary>
		/// If debug is true, debug message will be output.
		/// </summary>
		public bool Debug
		{
			get { return debug; }
			set { debug = true; }
		}

		VariableStack stack;

		/// <summary>
		/// Private constructor. Use static methods to construct instances.
		/// </summary>
		private Script(List<Lexeme> lexemes)
		{
			this.lexemes = lexemes;
			stack = new VariableStack();
		}

		/// <summary>
		/// Loads a script from a file.
		/// </summary>
		/// <param name="filename">The filename to read from.</param>
		/// <returns>Script</returns>
		public static Script FromFile(string filename)
		{
			StreamReader sr = File.OpenText(filename);
			string text = sr.ReadToEnd();
			
			return new Script(Lexeme.Parse(ref text));
		}	

		public void DisplayLexemes()
		{
			for(int i = 0; i < lexemes.Count; i++)
				OutputLine(i + " => " + lexemes[i]);
		}

		/// <summary>
		/// Execute the script.
		/// </summary>
		public void Execute()
		{
			int pos = 0;

			while(lexemes[pos].Type != LexemeType.EOF)
			{
				if(debug)
					OutputLine("Executing " + lexemes[pos]);
				
				Statement(ref pos);

				if(lexemes[pos - 1].Type != LexemeType.CloseBrace)
				{
					if(lexemes[pos].Type != LexemeType.EndStatement) // Check for ;
						throw new ScriptException("; was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

					pos++;
				}
			}
		}

		private Variable Statement(ref int pos)
		{
			if(debug)
				OutputLine("Statement at " + pos);

			Variable variable = null;

			if(lexemes[pos].Type == LexemeType.Echo) // Echo keyword
			{
				Echo(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.If)
			{
				DoIf(ref pos);
			}
			/*
			else if(lexemes[pos].Type == LexemeType.Identifier) // Statement begins with an identifier so it must be a function
			{
				string function = lexemes[pos].Val;

				pos++;
				if(lexemes[pos].Type != LexemeType.OpenParen)
					throw new ScriptException("( was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

				int pos2 = pos + 1;
				while(lexemes[pos2].Type != LexemeType.CloseParen && lexemes[pos2].Type != LexemeType.EOF)
					pos2++;

				if(lexemes[pos2].Type != LexemeType.CloseParen)
					throw new ScriptException(") was expected at Line " + lexemes[pos2].Line + " Column " + lexemes[pos2].Column);

				object obj = Function(function, FunctionArgs(pos, pos2));

				pos = pos2 + 1;

				return obj;
			}
			*/
			else // Must be an expression
			{
				variable = Expression(ref pos);
			}

			return variable;
		}

		private void Echo(ref int pos)
		{
			if(debug)
				OutputLine("Echo at " + pos);

			//if(lexemes[pos].Type != LexemeType.Echo)
			//	throw new ScriptException("'echo' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;

			Variable variable = Expression(ref pos);

			if(variable != null)
				OutputLine(variable.ToString());
		}

		private void DoIf(ref int pos)
		{
			DoIf(ref pos, false);
		}

		/// <summary>
		/// Executes or skips an if statement.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="skip">Should the if statement be skipped?</param>
		private void DoIf(ref int pos, bool skip)
		{
			pos++;
			if(lexemes[pos].Type != LexemeType.OpenParen)
				throw new ScriptException("'(' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			Variable variable = Expression(ref pos);

			if(lexemes[pos].Type != LexemeType.CloseParen)
				throw new ScriptException("')' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			if(lexemes[pos].Type != LexemeType.OpenBrace)
				throw new ScriptException("'{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			if(variable.ToBoolean() && !skip)
			{
				stack.Push();

				while(lexemes[pos].Type != LexemeType.CloseBrace)
				{
					Statement(ref pos);

					if(lexemes[pos].Type != LexemeType.EndStatement) // Check for ;
						throw new ScriptException("; was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

					pos++;
				}

				stack.Pop();

				pos++;

				if(lexemes[pos].Type == LexemeType.Else)
				{
					pos++;

					if(lexemes[pos].Type == LexemeType.If)
					{
						DoIf(ref pos, true);
					}
					else if(lexemes[pos].Type == LexemeType.OpenBrace)
					{
						pos++;

						while(lexemes[pos].Type != LexemeType.CloseBrace)
							pos++;

						pos++;
					}
					else
						throw new ScriptException("'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
				}
			}
			else
			{
				while(lexemes[pos].Type != LexemeType.CloseBrace)
					pos++;

				pos++;

				if(lexemes[pos].Type == LexemeType.Else)
				{
					pos++;

					if(lexemes[pos].Type == LexemeType.If)
					{
						DoIf(ref pos, skip);
					}
					else if(lexemes[pos].Type == LexemeType.OpenBrace)
					{
						pos++;

						if(!skip)
						{
							stack.Push();

							while(lexemes[pos].Type != LexemeType.CloseBrace)
							{
								Statement(ref pos);

								if(lexemes[pos].Type != LexemeType.EndStatement) // Check for ;
									throw new ScriptException("; was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

								pos++;
							}

							stack.Pop();
						}
						else
						{
							while(lexemes[pos].Type != LexemeType.CloseBrace)
								pos++;
						}

						pos++;
					}
					else
						throw new ScriptException("'if' or '{' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);
				}
			}
		}

		/// <summary>
		/// Skips over an if statement.
		/// </summary>
		/// <param name="pos"></param>
		private void SkipIf(ref int pos)
		{
		}

		private Variable Expression(ref int pos)
		{
			if(debug)
				OutputLine("Expression at " + pos);

			Variable variable = null;

			if(lexemes[pos].Type == LexemeType.Variable)
			{
				variable = Variable(ref pos);

				if(lexemes[pos].Type == LexemeType.OpenBracket)
				{
					pos++;

					if(lexemes[pos].Type != LexemeType.CloseBracket)
						variable = variable.AtIndex(Statement(ref pos)); // Key specified
					else
						variable = variable.AtIndex(null); // Request next key

					if(lexemes[pos].Type != LexemeType.CloseBracket)
						throw new ScriptException("']' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

					pos++;
				}
			}
			else if(lexemes[pos].Type == LexemeType.Null)
			{
				variable = Null(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.True || lexemes[pos].Type == LexemeType.False)
			{
				variable = Boolean(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.String)
			{
				variable = String(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.MagicString)
			{
				variable = MagicString(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.Integer)
			{
				variable = Integer(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.Float)
			{
				variable = Float(ref pos);
			}
			else if(lexemes[pos].Type == LexemeType.Array)
			{
				variable = Array(ref pos);
			}

			if(lexemes[pos].Type == LexemeType.Gets)
			{
				pos++;
				variable.Gets(Expression(ref pos));
			}
			else if(lexemes[pos].Type == LexemeType.Plus)
			{
				pos++;
				variable = variable.Add(Expression(ref pos));
			}
			else if(lexemes[pos].Type == LexemeType.PlusGets)
			{
				pos++;
				variable.Gets(variable.Add(Expression(ref pos)));
			}
			else if(lexemes[pos].Type == LexemeType.Minus)
			{
				pos++;
				variable = variable.Subtract(Expression(ref pos));
			}
			else if(lexemes[pos].Type == LexemeType.MinusGets)
			{
				pos++;
				variable.Gets(variable.Subtract(Expression(ref pos)));
			}
			else if(lexemes[pos].Type == LexemeType.EqualTo)
			{
				pos++;
				variable = variable.EqualTo(Expression(ref pos));
			}

			return variable;
		}

		/// <summary>
		/// Returns the SciptVariable associated with the identifier at pos.
		/// </summary>
		/// <param name="pos">The lexeme to read.</param>
		/// <returns>ScriptVariable</returns>
		private Variable Variable(ref int pos)
		{
			if(debug)
				OutputLine("Variable at " + pos);

			//if(lexemes[pos].Type != LexemeType.Variable)
			//	throw new ScriptException("'$' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;

			Variable variable = null;

			if(lexemes[pos].Type == LexemeType.Identifier)
			{
				variable = stack[lexemes[pos].Val];
			}

			pos++;
			return variable;
		}

		private Variable Null(ref int pos)
		{
			if(debug)
				OutputLine("Null at " + pos);

			pos++;
			return new Variable();
		}

		private Variable Boolean(ref int pos)
		{
			if(debug)
				OutputLine("Boolean at " + pos);

			pos++;
			if(lexemes[pos - 1].Type == LexemeType.True)
				return new Variable(true);
			else
				return new Variable(false);
		}

		private Variable String(ref int pos)
		{
			if(debug)
				OutputLine("String at " + pos);

			return new Variable(lexemes[pos++].Val);
		}

		private Variable MagicString(ref int pos)
		{
			if(debug)
				OutputLine("MagicString at " + pos);

			string output = lexemes[pos++].Val;

			return new Variable(output);
		}

		private Variable Integer(ref int pos)
		{
			if(debug)
				OutputLine("Integer at " + pos);
						
			return new Variable(Int32.Parse(lexemes[pos++].Val));
		}

		private Variable Float(ref int pos)
		{
			if(debug)
				OutputLine("Float at " + pos);

			return new Variable(Double.Parse(lexemes[pos++].Val));
		}

		private Variable Array(ref int pos)
		{
			if(debug)
				OutputLine("Array at " + pos);

			pos++;
			if(lexemes[pos].Type != LexemeType.OpenParen)
				throw new ScriptException("'(' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			Dictionary<string, Variable> values = new Dictionary<string, Variable>();
			pos++;

			int i = 0;
			while(lexemes[pos].Type != LexemeType.CloseParen && lexemes[pos].Type != LexemeType.EOF)
			{
				if(lexemes[pos + 1].Type != LexemeType.MapsTo) // No key specified
				{
					values.Add((i++).ToString(), Expression(ref pos));
				}
				else
				{
					Variable key = Expression(ref pos);

					pos++; // =>

					values.Add(key.ToString(), Expression(ref pos));
				}

				if(lexemes[pos].Type == LexemeType.Comma)
					pos++;
			}

			if(lexemes[pos].Type != LexemeType.CloseParen)
				throw new ScriptException("')' was expected at Line " + lexemes[pos].Line + " Column " + lexemes[pos].Column);

			pos++;
			return new Variable(values);
		}

		/*
		public object Function(string name, List<object> args)
		{
			return null;
		}

		public List<object> FunctionArgs(int left, int right)
		{
			OutputLine("FunctionArgs " + left + " to " + right);

			return new List<object>();
		}
		*/

		/// <summary>
		/// Output function. By default uses Console.Write().
		/// </summary>
		/// <param name="s"></param>
		public virtual void Output(string s)
		{
			Console.Write(s);
		}

		/// <summary>
		/// Output line function.
		/// </summary>
		/// <param name="s"></param>
		public void OutputLine(string s)
		{
			Output(s + '\n');
		}
	}
}
