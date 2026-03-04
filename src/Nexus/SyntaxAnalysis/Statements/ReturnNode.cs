namespace Nexus.SyntaxAnalysis
{
    class ReturnNode : StatementNode
    {
        public Node Expression { get; }

        public ReturnNode(Node expression)
        {
            Expression = expression;
        }

        /* Just write "return" instead of getting the keyword from LanguageSpecifications, as ToString
         * because Node.ToString() is only for debugging purposes. */
        public override string ToString() => $"return {Expression}";
    }
}
