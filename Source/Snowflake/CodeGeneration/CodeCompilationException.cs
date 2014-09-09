using System;

namespace Snowflake.CodeGeneration
{
	public class CodeCompilationException : ScriptException
	{
		public string ScriptId
		{
			get;
			private set;
		}

		public string Code
		{
			get;
			private set;
		}

		public CodeCompilationException(string message, string scriptId, string code)
			: base(message)
		{
			this.ScriptId = scriptId;
			this.Code = code;
		}
	}
}
