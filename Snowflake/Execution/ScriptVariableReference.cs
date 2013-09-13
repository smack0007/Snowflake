using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptVariableReference : ScriptObject
	{
		public override string TypeName
		{
			get
			{
				if (this.Value != null)
				{
					return this.Value.TypeName;
				}
				else
				{
					return "null";
				}
			}
		}

		public ScriptObject Value
		{
			get;
			set;
		}

		public ScriptVariableReference(ScriptObject value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			this.Value = value;
		}

		public override void Gets(ScriptObject other)
		{
			this.Value = other;
		}
	}
}
