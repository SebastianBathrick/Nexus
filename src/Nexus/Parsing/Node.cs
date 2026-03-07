namespace Nexus.Parsing
{
    abstract class Node
    {
        protected const string IndentUnit = "  ";

        internal abstract string ToDebugString(int depth);

        public override sealed string ToString() => ToDebugString(0);

        protected static string Pad(int depth)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < depth; i++) sb.Append(IndentUnit);
            return sb.ToString();
        }
    }
}
