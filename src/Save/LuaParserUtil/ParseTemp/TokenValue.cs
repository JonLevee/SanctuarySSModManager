namespace LuaParserUtil.ParseTemp
{
    public class TokenValue : Token
    {
        public readonly static TokenValue Null = new TokenValue();
        public TokenType Type { get; set; }
        public bool IsEditable => (TokenType.Editable & Type) != 0;
        private string? updateValue;

        private TokenValue() { }

        public TokenValue(ParsingState state, TokenType type) : base(state)
        {
            Type = type;
            updateValue = null;
        }

        public virtual string Value
        {
            get => updateValue ?? sb.ToString(StartIndex, Length);
            set
            {
                if (!IsEditable)
                    throw new InvalidOperationException("Token is not editable");
                updateValue = value;
            }
        }
    }
}
