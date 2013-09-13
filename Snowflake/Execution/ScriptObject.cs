using System;

namespace Snowsoft.SnowflakeScript.Execution
{
	public abstract class ScriptObject
	{
		public abstract string TypeName { get; }

		private void ThrowOperationNotSupportedException(string operation)
		{
			throw new ScriptExecutionException(string.Format("Type \"{0}\" does not support the {1} operation.", this.TypeName, operation));
		}

		public virtual object GetValue()
		{
			this.ThrowOperationNotSupportedException("GetValue");
			return null;
		}

		public virtual void Gets(ScriptObject other)
		{
			this.ThrowOperationNotSupportedException("Gets");
		}

		public virtual ScriptObject Add(ScriptObject other)
		{
			this.ThrowOperationNotSupportedException("Add");
			return null;
		}
	}

	public abstract class ScriptObject<T> : ScriptObject
	{
		public T Value
		{
			get;
			set;
		}

		protected ScriptObject()
			: base()
		{
		}

		protected ScriptObject(T value)
			: base()
		{
			this.Value = value;
		}

		public override object GetValue()
		{
			return this.Value;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
