using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptVariableReference : ScriptObject
	{
		public override string TypeName
		{
			get { return "var"; }
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

		public override object GetValue()
		{
			return this.Value.GetValue();
		}

		public override void Gets(ScriptObject other)
		{
			this.Value = (other is ScriptVariableReference ? ((ScriptVariableReference)other).Value : other);
		}

		public override ScriptObject Add(ScriptObject other)
		{
			return this.Value.Add((other is ScriptVariableReference ? ((ScriptVariableReference)other).Value : other));
		}
	}
}
