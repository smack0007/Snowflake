using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Parsing
{
    public class UsingDeclarationNode : StatementNode
    {
        public string NamespaceName { get; set; }

        public UsingDeclarationNode()
            : base()
        {
        }
    }
}
