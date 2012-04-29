using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ExecutionException : ScriptException
	{
		public ExecutionException(string message)
			: base(message)
		{
		}
	}
}
