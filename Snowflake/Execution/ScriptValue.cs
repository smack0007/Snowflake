using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public abstract class ScriptValue<T> : ScriptObject
		where T : struct
	{
		public T Value
		{
			get;
			private set;
		}

		protected ScriptValue()
			: base()
		{
		}

		protected ScriptValue(T value)
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

			if (other is ScriptValue<T>)
			{
				result = this.Value.Equals(((ScriptValue<T>)other).Value);
			}

			return result ? ScriptBoolean.True : ScriptBoolean.False;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
