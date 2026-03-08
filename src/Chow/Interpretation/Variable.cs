namespace Chow.Interpretation
{
    class Variable
    {
        public int Id { get; }
        public TaggedUnion Value { get; private set; }

        public Variable(int id, TaggedUnion val)
        {
            Id = id;
            Value = val;
        }

        public void Assign(TaggedUnion val) => Value = val;
    }
}