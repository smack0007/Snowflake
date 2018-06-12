using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution
{
    public sealed class ScriptList : List<object>
    {
        public ScriptList()
        {
        }

        public ScriptList(int capacity)
            : base(capacity > 4 ? capacity : 4)
        {
        }

        public ScriptList(IEnumerable<object> collection)
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

        public T ElementAt<T>(int index)
        {
            return (T)this[index];
        }
    }
}
