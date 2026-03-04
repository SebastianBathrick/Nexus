namespace Clua.SyntaxAnalysis
{
    class ReturnNode(Node expression) : StatementNode
    {
        public Node Expression => expression;

        /* Just write "return" instead of getting the keyword from LanguageSpecifications, as ToString
         * because Node.ToString() is only for debugging purposes. */
        public override string ToString() => $"return {Expression}";
    }
}
