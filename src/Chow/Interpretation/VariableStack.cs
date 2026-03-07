using Chow.Interpretation;

namespace Chow.Interpretation
{
    class VariableStack : FastStack<Variable>
    {
        public VariableStack() : base() {}

        public Variable? LookUp(int id)
        {
            var peekDepth = 0;

            while (peekDepth < Count)
            {
                if (Peek(peekDepth).Id == id)
                    return Peek(peekDepth);

                peekDepth++;
            }

            return null;
        }
    }
}