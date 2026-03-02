namespace Clua.Utilities;

class FastStack<T>
{
    const int MinimumSize = 8;
    const float GrowthFactor = 1.5f;
    const float ShrinkFactor = 0.5f;
    const float ShrinkThreshold = 0.25f;
    const int MinumumArrayCopyCallCount = 32;

    T[] _arr = new T[MinimumSize];

    public int Count { get; private set; }

    public void Push(T value)
    {
        if (Count == _arr.Length)
            ResizeArray((int)(_arr.Length * GrowthFactor));

        _arr[Count++] = value;
    }

    public T Pop()
    {
        if (Count == 0)
            throw new InvalidOperationException("Cannot pop from an empty stack.");

        var popped = _arr[--Count];

        if (_arr.Length == MinimumSize || Count > _arr.Length * ShrinkThreshold)
            return popped;

        var newSize = (int)(_arr.Length * ShrinkFactor);
        ResizeArray(newSize > MinimumSize ? newSize : MinimumSize);
        return popped;
    }

    void ResizeArray(int newSize)
    {
        var newValArr = new T[newSize];

        if (Count < MinumumArrayCopyCallCount)
            for (var i = 0; i < Count; i++)
                newValArr[i] = _arr[i];
        else
            Array.Copy(_arr, newValArr, Count);

        _arr = newValArr;
    }

    public bool Any() => Count != 0;
}
