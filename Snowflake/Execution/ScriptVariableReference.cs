using System;
using System.Collections.Generic;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptVariableReference : ScriptObject
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

		public bool IsConst
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
				
		public override object Unbox()
		{
			return this.Value.Unbox();
		}

		private static ScriptObject Unbox(ScriptObject other)
		{
			if (other is ScriptVariableReference)
			{
				return ((ScriptVariableReference)other).Value;
			}

			return other;
		}

		public override void Gets(ScriptObject other)
		{
			this.Value = Unbox(other);
		}
	}
}
