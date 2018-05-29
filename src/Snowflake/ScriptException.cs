using System;

namespace Snowflake
{
	public class ScriptException : Exception
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		public ScriptException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ScriptException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
