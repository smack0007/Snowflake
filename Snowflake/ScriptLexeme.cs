using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptLexeme
	{
		public ScriptLexemeType Type
		{
			get;
			private set;
		}

		public string Val
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
			return "{" + this.Type + ", " + this.Val + ", " + this.Line + ", " + this.Column + "}";
		}
	}
}
