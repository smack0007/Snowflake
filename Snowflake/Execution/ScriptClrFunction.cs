using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptClrFunction : ScriptObject
	{
		public override string TypeName
		{
			get { return "ClrFunc"; }
		}

		internal Delegate Function
		{
			get;
			set;
		}

		public ScriptClrFunction(Delegate function)
		{
			this.Function = function;
		}
	}
}
