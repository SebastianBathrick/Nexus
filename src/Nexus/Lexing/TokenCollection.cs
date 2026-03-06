using System;
using System.Collections.Generic;

namespace Nexus.Lexing
{
    class TokenCollection
    {
        readonly List<Token> _tokens = new();
        int _index;

        public IReadOnlyList<Token> Tokens => _tokens;

        public bool IsEmpty => _index >= _tokens.Count;

        public void Add(Token token)
        {
            _tokens.Add(token);
        }

        public Token Read()
        {
            if (_index >= _tokens.Count)
                throw new InvalidOperationException("No tokens remaining.");

            return _tokens[_index++];
        }

        public TokenType ReadType() => Read().Type;

        public TokenType PeekType()
        {
            if (_index >= _tokens.Count)
                throw new InvalidOperationException("No tokens remaining.");

            return _tokens[_index].Type;
        }

        public bool IsOfTypeAndConsume(TokenType tokenType)
        {
            if (_index >= _tokens.Count || _tokens[_index].Type != tokenType)
                return false;

            _index++;
            return true;
        }

        public void Consume() => Read();

        public bool IsOfType(TokenType tokenType) => _index < _tokens.Count && _tokens[_index].Type == tokenType;

        public bool IsOfType(params TokenType[] tokenTypes)
        {
            foreach (var t in tokenTypes)
                if (IsOfType(t))
                    return true;

            return false;
        }

        public IReadOnlyList<Token> ToList() =>
            // Ensure to copy the list to prevent external modification of the internal list
            new List<Token>(_tokens);
    }
}
