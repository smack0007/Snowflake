using System;

namespace Snowflake.CodeGeneration
{
	public class CodeCompilationException : ScriptException
	{
		public string Code
		{
			get;
			private set;
		}

		public CodeCompilationException(string message, string code)
			: base(message)
		{
			this.Code = code;
		}
	}
}
