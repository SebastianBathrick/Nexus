namespace Nexus.SyntaxAnalysis.Statements
{
    class ReturnNode : StatementNode
    {
        public ReturnNode(Node expression)
        {
            Expression = expression;
        }

        public Node Expression { get; }

        /* Just write "return" instead of getting the keyword from LanguageSpecifications, as ToString
         * because Node.ToString() is only for debugging purposes. */
        public override string ToString() => $"return {Expression}";
    }
}
