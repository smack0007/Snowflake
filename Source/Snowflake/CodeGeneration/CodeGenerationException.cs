using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.CodeGeneration
{
	public class CodeGenerationException : ScriptException
	{
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
		        
		public CodeGenerationException(string message, int line, int column)
			: base(message)
		{
            this.Line = line;
            this.Column = column;
		}
	}
}
