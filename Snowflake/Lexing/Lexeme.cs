using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Lexing
{
	public class Lexeme
	{
		public LexemeType Type
		{
			get;
			private set;
		}

		public string Value
		{
			get;
			private set;
		}

		public int Line
		{
			get;
			private set;
		}

		public int Column
		{
			get;
			private set;
		}

		public Lexeme(LexemeType type, string val, int line, int column)
		{
			this.Type = type;

			if(val != null)
				this.Value = val;
			else
				this.Value = null;

			this.Line = line;
			this.Column = column;
		}

		public override string ToString()
		{
			return "{" + this.Type + ", " + this.Value + ", " + this.Line + ", " + this.Column + "}";
		}
	}
}
