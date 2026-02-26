namespace Cemble;

public class CharStream
{
    const int InitialIndex = 0;
    const char InvalidChar = '\0';
    
    readonly char[] _chars;
    private int _currIndex;

    public CharStream(string srcCode, char[]? extraAllowedChars = null)
    {
        _currIndex = InitialIndex;
        _chars = srcCode.ToCharArray();
        

        while (IsCharInStream())
        {
            if (extraAllowedChars?.Contains(_chars[_currIndex]) == true || IsCharNumeric() || IsCharAlpha() || IsCharWhitespace())
            {
                // Ignore return value since all we need to know is if it is valid
                ReadNextChar();
                continue;
            }
            
            throw new ArgumentException($"Invalid character '{_chars[_currIndex]}' at index {_currIndex}");
        }

        _currIndex = InitialIndex;
    }

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

    public bool IsCharWhitespace()
    {
        if (!TryPeekChar(out char c))
            return false;

        return c switch
        {
            ' ' or '\t' or '\n' or '\r' or '\v' or '\f' => true,
            _ => false
        };
    }
    
    public bool IsCharNumeric() => TryPeekChar(out var c) && (c >= '0' && c <= '9');
    
    public bool IsCharAlpha() => TryPeekChar(out var c) && (c  >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z');
    
    public bool IsCharInStream() => _currIndex < _chars.Length;
}