using System.Text;
namespace Chow.Parsing
{
    abstract class Node
    {
        protected const string IndentUnit = "  ";

        internal abstract string ToDebugString(int depth);

        public sealed override string ToString() => ToDebugString(0);

        protected static string Pad(int depth)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < depth; i++) sb.Append(IndentUnit);
            return sb.ToString();
        }
    }
}
