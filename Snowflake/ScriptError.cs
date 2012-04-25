using System;

namespace Snowsoft.SnowflakeScript
{
	public enum ScriptError
	{
		/// <summary>
		/// An error occured while parsing.
		/// </summary>
		ParseError,

		/// <summary>
		/// An error exists in the syntax of a script.
		/// </summary>
		SyntaxError,

		/// <summary>
		/// The given operation can not be applied to a variable.
		/// </summary>
		OperationNotAvailable,

		/// <summary>
		/// An error occured related to the stack.
		/// </summary>
		VariableStackError,

		/// <summary>
		/// Failed to call a function.
		/// </summary>
		InvalidFunctionCall
	}
}
