using System;

namespace Snowflake.Parsing
{
	public class SyntaxException : ScriptException
	{
		public SyntaxException(string message)
			: base(message)
		{
		}
	}
}