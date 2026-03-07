using System;
namespace Chow.Interpretation
{
    public class FastStack<T>
    {
        #region Constants

        const int GrowthFactor = 2;
        const int ShrinkFactor = 2;
        const float ShrinkThreshold = 0.32f;
        const int ArrayCopyThreshold = 32;
        const int MinCapacity = 16;

        #endregion

        T[] _vals = null;
        int _len = 0;

        public int Count => _len;

        public T Peek(int offset = 0)
        {
            if (_vals == null || _len == 0)
                throw new InvalidOperationException("Stack is empty");

            return _vals[_len - 1 - offset];
        }

        public void Push(T val)
        {
            if (_vals == null)
                _vals = new T[MinCapacity];

            if (_len == _vals.Length)
                Resize(_vals.Length * GrowthFactor);

            _vals[_len++] = val;
        }

        public T Pop()
        {
            if (_vals == null || _len == 0)
                throw new InvalidOperationException("Stack is empty");

            _len--;

            if (_len > _vals.Length * ShrinkThreshold)
                return _vals[_len];

            var poppedVal = _vals[_len];
            Resize(_vals.Length / ShrinkFactor);
            return poppedVal;
        }

        void Resize(int newCapacity)
        {
            if (_vals == null)
                throw new InvalidOperationException("Stack is not initialized");

            if (newCapacity < MinCapacity)
                newCapacity = MinCapacity;

            var newValArr = new T[newCapacity];

            if (_len < ArrayCopyThreshold)
                for (int i = 0; i < _len; i++)
                    newValArr[i] = _vals[i];
            else
                Array.Copy(_vals, newValArr, _len);

            _vals = newValArr;
        }
    }
}
