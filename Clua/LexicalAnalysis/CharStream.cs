namespace Clua;

class CharStream
{
    const int InitialIndex = 0;
    public const char InvalidChar = '\0';
    
    readonly char[] _chars;
    int _currIndex;

    public CharStream(string srcCode)
    {
        _currIndex = InitialIndex;
        _chars = srcCode.ToCharArray();

        while (IsCharInStream())
        {
            if (GetCharType() != CharType.Invalid)
            {
                // Ignore return value since all we need to know is if it is valid
                ReadNextChar();
                continue;
            }

            throw new ArgumentException($"Invalid character '{_chars[_currIndex]}' at index {_currIndex}");
        }

        _currIndex = InitialIndex;
    }

    public void IgnoreChar() => ReadNextChar();
    
    public char ReadNextChar()
    {
        if (_currIndex >= _chars.Length)
            return InvalidChar;

        return _chars[_currIndex++];
    }
    
    public bool TryPeekChar(out char c)
    {
        if (_currIndex >= _chars.Length)
        {
            c = InvalidChar;
            return false;
        }
        
        c = _chars[_currIndex];
        return true;
    }

    public CharType GetCharType()
    {
        if (!TryPeekChar(out var c))
            return CharType.Invalid;
        return LanguageSpecifications.GetCharType(c);
    }

    #region Char Type Flag Methods

    public bool IsCharWhitespace() => GetCharType() == CharType.Whitespace;
    
    public bool IsCharNumeric() => GetCharType() == CharType.Numeric;

    public bool IsCharAlpha() => GetCharType() == CharType.Alpha;

    public bool IsCharUnderscore() => GetCharType() == CharType.Underscore;

    public bool IsCharDot() => GetCharType() == CharType.Dot;

    public bool IsCharOperator() => GetCharType() == CharType.Operator;

    public bool IsCharInStream() => _currIndex < _chars.Length;

    #endregion
}

    public enum CharType { Numeric, Alpha, Underscore, Whitespace, Dot, Operator, OpenParen, CloseParen, Invalid}