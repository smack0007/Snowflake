using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public sealed class ScriptString : ScriptObject
	{
		public override string TypeName
		{
			get { return "string"; }
		}

		public string Value
		{
			get;
			private set;
		}

		public ScriptString(string value)
			: base()
		{
			this.Value = value;
		}

		public override object Unbox()
		{
			return this.Value;
		}

		public override ScriptBoolean EqualTo(ScriptObject other)
		{
			bool result = false;

			if (other is ScriptString)
			{
				string otherValue = ((ScriptString)other).Value;

				if (this.Value != null)
				{
					result = this.Value.Equals(otherValue);
				}
				else
				{
					result = otherValue == null;
				}
			}

			return result ? ScriptBoolean.True : ScriptBoolean.False;
		}

		public override ScriptObject Add(ScriptObject other)
		{
			return new ScriptString(this.Value + other.ToString());
		}

		public override string ToString()
		{
			return this.Value;
		}
	}
}
