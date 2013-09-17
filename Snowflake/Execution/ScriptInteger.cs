using System;
using System.Globalization;

namespace Snowsoft.SnowflakeScript.Execution
{
	public class ScriptInteger : ScriptObject<int>
	{
		public override string TypeName
		{
			get { return "int"; }
		}
				
		public ScriptInteger(int value)
			: base(value)
		{
		}

		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		public override ScriptObject Add(ScriptObject other)
		{
			if (other is ScriptInteger)
			{
				return new ScriptInteger(this.Value + ((ScriptInteger)other).Value);
			}
			else if (other is ScriptFloat)
			{
				return new ScriptFloat(this.Value + ((ScriptFloat)other).Value);
			}
			else if (other is ScriptString)
			{
				return new ScriptString(this.Value.ToString() + ((ScriptString)other).Value);
			}
			else
			{
				throw new ScriptExecutionException(string.Format("Add operation not supported for type {0} and {1}.", this.TypeName, other.TypeName));
			}
		}
	}
}
