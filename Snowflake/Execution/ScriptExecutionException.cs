using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptExecutionException : ScriptException
	{
		public ScriptExecutionException(string message)
			: base(message)
		{
		}
	}
}
