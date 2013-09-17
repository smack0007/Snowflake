using System;
using System.Globalization;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptFloat : ScriptObject<float>
	{
		public override string TypeName
		{
			get { return "float"; }
		}

		public ScriptFloat(float value)
			: base(value)
		{
		}

		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		public override ScriptObject Add(ScriptObject other)
		{
			if (other is ScriptFloat)
			{
				return new ScriptFloat(this.Value + ((ScriptFloat)other).Value);
			}
			else if (other is ScriptInteger)
			{
				return new ScriptFloat(this.Value + ((ScriptInteger)other).Value);
			}
			else if (other is ScriptString)
			{
				return new ScriptString(this.ToString() + ((ScriptString)other).Value);
			}
			else
			{
				this.ThrowOperationNotSupportedBetweenTypesException("Add", other);
				return null;
			}
		}

		public override ScriptObject Subtract(ScriptObject other)
		{
			if (other is ScriptFloat)
			{
				return new ScriptFloat(this.Value - ((ScriptFloat)other).Value);
			}
			else if (other is ScriptInteger)
			{
				return new ScriptFloat(this.Value - ((ScriptInteger)other).Value);
			}
			else
			{
				this.ThrowOperationNotSupportedBetweenTypesException("Subtract", other);
				return null;
			}
		}
	}
}
