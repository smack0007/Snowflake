using System;

namespace Snowsoft.SnowflakeScript
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
	}
}
