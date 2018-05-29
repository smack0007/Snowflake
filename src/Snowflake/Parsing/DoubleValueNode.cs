using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
	public class DoubleValueNode : ValueNode
	{
		public override Type ValueType
		{
			get { return typeof(float); }
		}

		public double Value
		{
			get;
			set;
		}

		public DoubleValueNode()
			: base()
		{
		}

		public override object GetValue()
		{
			return this.Value;
		}
	}
}
