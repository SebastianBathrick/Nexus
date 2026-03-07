using Chow.Values;
namespace Chow.Interpretation
{
    class Variable
    {
        public int Id { get; }
        public ChowValue Value { get; private set; }

        public Variable(int id, ChowValue val)
        {
            Id = id;
            Value = val;
        }

        public void Assign(ChowValue val) => Value = val;
    }
}