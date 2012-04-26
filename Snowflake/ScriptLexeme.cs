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
	}
}
