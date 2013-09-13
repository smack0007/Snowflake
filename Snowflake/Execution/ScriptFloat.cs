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
	}
}
