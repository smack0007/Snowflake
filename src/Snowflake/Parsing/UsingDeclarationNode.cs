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
