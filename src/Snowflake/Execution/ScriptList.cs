using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution
{
    public class ScriptList : List<dynamic>
    {
        public ScriptList()
        {
        }

        public ScriptList(IEnumerable<dynamic> collection)
            : base(collection)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.Count * 4 + 2);
            sb.Append("[");

            for (int i = 0; i < this.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(this[i].ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
