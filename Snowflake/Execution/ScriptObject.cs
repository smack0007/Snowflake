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

		protected void ThrowOperationNotSupportedBetweenTypesException(string operation, ScriptObject other)
		{
			throw new ScriptExecutionException(string.Format("{0} operation not supported for type {1} and {2}.", operation, this.TypeName, other.TypeName));
		}

		public virtual object Unbox()
		{
			this.ThrowOperationNotSupportedException("Unbox");
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

		public virtual ScriptObject Subtract(ScriptObject other)
		{
			this.ThrowOperationNotSupportedException("Subtract");
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

		public override object Unbox()
		{
			return this.Value;
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
