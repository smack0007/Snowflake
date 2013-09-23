using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptClrMethod : ScriptObject
	{
		public override string TypeName
		{
			get { return "ClrMethod"; }
		}

		internal Delegate Function
		{
			get;
			set;
		}

		public ScriptClrMethod(Delegate function)
		{
			this.Function = function;
		}
	}
}
