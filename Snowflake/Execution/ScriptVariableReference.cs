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

			this.Value = Unbox(value);
		}

		private static ScriptObject Unbox(ScriptObject other)
		{
			if (other is ScriptVariableReference)
			{
				return ((ScriptVariableReference)other).Value;
			}

			return other;
		}

		public override object Unbox()
		{
			return this.Value.Unbox();
		}

		public override void Gets(ScriptObject other)
		{
			this.Value = Unbox(other);
		}

		public override ScriptObject Add(ScriptObject other)
		{
			return this.Value.Add(Unbox(other));
		}

		public override ScriptObject Subtract(ScriptObject other)
		{
			return this.Value.Subtract(Unbox(other));
		}
	}
}
