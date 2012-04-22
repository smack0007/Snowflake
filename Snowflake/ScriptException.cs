using System;

namespace Snowsoft.SnowflakeScript
{
	public class ScriptException : Exception
	{
		/// <summary>
		/// Indicates what kind of error occured.
		/// </summary>
		public ScriptError Error
		{
			get;
			private set;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="error"></param>
		/// <param name="message"></param>
		public ScriptException(ScriptError error, string message)
			: base(message)
		{
		}
	}
}
